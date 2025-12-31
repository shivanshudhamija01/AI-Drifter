using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using Cinemachine;
using TMPro;


public class LevelGenerator : MonoBehaviour
{
    [Header("Virtual Camera")]
    [SerializeField] private CinemachineVirtualCamera vCam;

    [Header("Level Generation")]
    [SerializeField] private List<GameObject> tile;
    [SerializeField] private List<LevelSO> levels;
    [SerializeField] private Transform Enviroment;
    [SerializeField] private string spawnPoint = "SpawnPoint";
    [SerializeField] private int width = 30;
    [SerializeField] private int height = 30;
    [SerializeField] private int levelNumber = 0;

    [Header("Player Cars")]
    [SerializeField] private List<CarSO> allAvailableCars;

    private int[][] matrix;
    private List<GameObject> tileInCurrentScene = new List<GameObject>();
    private List<Transform> vacantTiles = new List<Transform>();

    // THESE WILL NOW BE LOADED FROM LevelSO :)
    private GameObject collectible;
    private int collectibleCount;
    private List<GameObject> powerUpsList;
    private int powerUpsPerWave = 5;
    private float minLifeTime = 4f;
    private float maxLifeTime = 8f;
    private float respawnDelay = 10f;
    private int powerUpPoolSize = 0;
    private int rewardValue = 0;

    // Enemy counts from LevelSO
    private int smartAICount;
    private int madAICount;
    private int aggressiveAICount;

    // Enemy prefabs from LevelSO
    private List<GameObject> smartAIPrefabs;
    private List<GameObject> madAIPrefabs;
    private List<GameObject> aggressiveAIPrefabs;

    // Pooling
    private Queue<GameObject> powerUpsPool = new Queue<GameObject>();
    private Dictionary<GameObject, Transform> gameObjectToSpawnPoint = new Dictionary<GameObject, Transform>();
    private List<GameObject> powerUpsInScene = new List<GameObject>();
    private int activePowerUpsCount;

    //--------------- GameObjects Active In Scene--------------- //
    // Player
    private GameObject playerInScene;
    private Transform playerSpawnPoint;
    private GameObject directionArrow;

    //-----------------------------------------------------------//
    // Enemy
    private List<GameObject> enemiesInScene = new List<GameObject>();

    // Collectible Coins
    private List<GameObject> coinsActiveInScene = new List<GameObject>();
    private List<GameObject> coinsCollectedInScene = new List<GameObject>();

    [SerializeField] private TMPro.TextMeshProUGUI countdownText;
    [SerializeField] private float countdownStart = 3f;
    [SerializeField] private TextMeshProUGUI levelRewardValue;
    // Track running coroutines
    private Coroutine spawnSceneRoutine;
    private Coroutine powerUpsLoopRoutine;
    private Coroutine enemySpawnRoutine;
    private Coroutine countdownRoutine;
    private bool isRestarting = false;



    void OnEnable()
    {
        PlayerServices.Instance.OnCoinPickedUp.AddListener(DeactivateCoinFromScene);
        PlayerServices.Instance.OnGhostCollected.AddListener(MultiplyEnemiesInScene);
        PlayerServices.Instance.OnPlayerDead.AddListener(PlayerDeadAndStopGame);
        LevelServices.Instance.OnLevelCompleted.AddListener(PlayerDeadAndStopGame);
        LevelServices.Instance.LoadNextLevel.AddListener(LoadNextLevel);
        PlayerServices.Instance.OnPlayerDead.AddListener(PlayerDeadAndStopGame);
        LevelServices.Instance.LoadLevel.AddListener(LoadLevelBaseOnLevelNumber);
        LevelServices.Instance.LevelRestart.AddListener(RestartLevel);
        LevelServices.Instance.ResetLevel.AddListener(ResetLevel);
    }
    void OnDisable()
    {
        PlayerServices.Instance.OnCoinPickedUp.RemoveListener(DeactivateCoinFromScene);
        PlayerServices.Instance.OnGhostCollected.RemoveListener(MultiplyEnemiesInScene);
        PlayerServices.Instance.OnPlayerDead.RemoveListener(PlayerDeadAndStopGame);
        LevelServices.Instance.OnLevelCompleted.RemoveListener(PlayerDeadAndStopGame);
        LevelServices.Instance.LoadNextLevel.RemoveListener(LoadNextLevel);
        PlayerServices.Instance.OnPlayerDead.RemoveListener(PlayerDeadAndStopGame);
        LevelServices.Instance.LoadLevel.RemoveListener(LoadLevelBaseOnLevelNumber);
        LevelServices.Instance.LevelRestart.RemoveListener(RestartLevel);
        LevelServices.Instance.ResetLevel.RemoveListener(ResetLevel);
    }
    void Start()
    {
        // LoadLevelData();
        // GameManager.Instance.SetTotalCollectibles(10);
        // GetMapFromLevelLoader();
        // spawnSceneRoutine = StartCoroutine(SpawnScene());
        // powerUpsLoopRoutine = StartCoroutine(PowerUpsWaveLoop());
        // InitPowerUpsPool();
    }

    void LoadLevelBaseOnLevelNumber(int lvlNumber)
    {
        levelNumber = lvlNumber;
        LoadLevelData();
        spawnSceneRoutine = StartCoroutine(SpawnScene());
        powerUpsLoopRoutine = StartCoroutine(PowerUpsWaveLoop());
        InitPowerUpsPool();
    }
    void Update()
    {
        if (directionArrow == null) return;

        PointTowardsNearbyCoin();
    }

    #region Load Level Data from LevelSO

    void LoadLevelData()
    {
        if (levels == null || levels.Count == 0)
        {
            Debug.LogError("No levels assigned in LevelGenerator!");
            return;
        }

        if (levelNumber >= levels.Count)
        {
            Debug.LogWarning($"Level {levelNumber} doesn't exist! Loading last level.");
            levelNumber = levels.Count - 1;
        }

        LevelSO currentLevel = levels[levelNumber];

        // Load Enemy Data
        smartAICount = currentLevel.smartAI;
        madAICount = currentLevel.madAI;
        aggressiveAICount = currentLevel.aggressiveAI;

        smartAIPrefabs = currentLevel.SmartAI;
        madAIPrefabs = currentLevel.MadAI;
        aggressiveAIPrefabs = currentLevel.AggressiveAI;

        // Load Collectible Data
        collectible = currentLevel.CollectiblePrefab;
        collectibleCount = currentLevel.CollectibleCount;

        // Load PowerUp Data
        powerUpsList = currentLevel.PowerUps;
        powerUpPoolSize = currentLevel.PowerUpPoolSize;
        powerUpsPerWave = currentLevel.PowerUpPerWave;
        minLifeTime = currentLevel.MinLifeTime;
        maxLifeTime = currentLevel.MaxLiftTime;
        respawnDelay = currentLevel.RespawnDelay;

        // Reward Value 
        rewardValue = currentLevel.RewardValue;
        levelRewardValue.text = rewardValue.ToString();

        GameManager.Instance.SetTotalCollectibles(collectibleCount);
        GameManager.Instance.SetLevelNumber(levelNumber);
        GameManager.Instance.SetRewardValue(rewardValue);
        // Update the game manager with the reward value and level number

        Debug.Log($"Loaded Level {levelNumber} Data - Enemies: Smart({smartAICount}), Mad({madAICount}), Aggressive({aggressiveAICount})");
        Debug.Log($"Collectibles: {collectibleCount}, PowerUps per Wave: {powerUpsPerWave}");
    }

    #endregion

    #region Level Generation

    void GetMapFromLevelLoader()
    {
        matrix = LevelDataLoader.Instance.GetLevel(levelNumber);
        GenerateGround();
    }

    private void PointTowardsNearbyCoin()
    {
        if (playerInScene == null || coinsActiveInScene == null || coinsActiveInScene.Count == 0)
        {
            directionArrow.SetActive(false);
            return;
        }

        GameObject nearestCoin = null;
        float shortestDistance = float.MaxValue;

        Vector3 playerPos = playerInScene.transform.position;

        // Find nearest active coin
        for (int i = coinsActiveInScene.Count - 1; i >= 0; i--)
        {
            GameObject coin = coinsActiveInScene[i];

            // Remove destroyed / inactive coins safely
            if (coin == null || !coin.activeInHierarchy)
            {
                coinsActiveInScene.RemoveAt(i);
                continue;
            }

            float distance = Vector3.SqrMagnitude(coin.transform.position - playerPos);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestCoin = coin;
            }
        }

        if (nearestCoin == null)
        {
            directionArrow.SetActive(false);
            return;
        }

        directionArrow.SetActive(true);

        // Direction (ignore Y so arrow stays flat)
        Vector3 dir = nearestCoin.transform.position - playerPos;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(dir.normalized);
        directionArrow.transform.rotation =
            Quaternion.Slerp(directionArrow.transform.rotation, targetRotation, Time.deltaTime * 8f);
    }

    void GenerateGround()
    {
        for (int i = 0; i < matrix.Length; i++)
        {
            for (int j = 0; j < matrix[i].Length; j++)
            {
                Vector3 spawnPos = new Vector3(i * width, 0, j * height);
                GameObject obj = Instantiate(tile[matrix[i][j]], spawnPos, Quaternion.identity);
                if (i == 0)
                {
                    obj.transform.rotation = Quaternion.Euler(0, -90, 0);
                }
                else if (j == 0)
                {
                    obj.transform.rotation = Quaternion.Euler(0, -180, 0);
                }
                else if (i == matrix.Length - 1)
                {
                    obj.transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                Transform point = obj.transform.Find(spawnPoint);
                if (point != null)
                {
                    if (i == (int)(matrix.Length / 2) && j == (int)(matrix[i].Length / 2))
                    {
                        // This point represent the center tile in a matrix
                        playerSpawnPoint = point;
                        playerSpawnPoint.position = new Vector3(playerSpawnPoint.position.x, 0, playerSpawnPoint.position.z);
                        // Dont need to add a spawn point in the list (Vacant tiles)
                    }
                    else
                    {
                        vacantTiles.Add(point);
                    }
                }

                obj.transform.SetParent(Enviroment);
                tileInCurrentScene.Add(obj);
            }
        }

        NavMeshSurface navMesh = Enviroment.GetComponent<NavMeshSurface>();
        if (navMesh != null)
        {
            navMesh.RemoveData();
            navMesh.BuildNavMesh();
        }
    }

    #endregion

    #region PowerUps Pool

    void InitPowerUpsPool()
    {
        if (powerUpsList == null || powerUpsList.Count == 0)
        {
            Debug.LogWarning("No PowerUps assigned in current level!");
            return;
        }
        powerUpsPool.Clear();
        int totalTypesOfPowerUps = powerUpsList.Count;
        for (int i = 0; i < powerUpPoolSize; i++)
        {
            int index = i % totalTypesOfPowerUps;
            GameObject obj = Instantiate(powerUpsList[index]);
            obj.SetActive(false);
            powerUpsPool.Enqueue(obj);
        }
    }

    #endregion

    #region PowerUps Wave System

    IEnumerator PowerUpsWaveLoop()
    {
        // Don't spawn powerups if powerUpsPerWave is 0
        if (powerUpsPerWave <= 0)
        {
            Debug.Log("PowerUps disabled for this level (PowerUpPerWave = 0)");
            yield break;
        }

        while (true)
        {
            yield return new WaitUntil(() => vacantTiles.Count >= powerUpsPerWave);

            SpawnPowerUpsWave();

            yield return new WaitUntil(() => activePowerUpsCount == 0);

            yield return new WaitForSeconds(respawnDelay);
        }
    }

    void SpawnPowerUpsWave()
    {
        activePowerUpsCount = 0;

        for (int i = 0; i < powerUpsPerWave; i++)
        {
            if (vacantTiles.Count == 0 || powerUpsPool.Count == 0)
                break;

            int tileIndex = Random.Range(0, vacantTiles.Count);
            Transform spawnPoint = vacantTiles[tileIndex];

            GameObject powerUp = powerUpsPool.Dequeue();
            powerUp.transform.position = spawnPoint.position;
            powerUp.transform.rotation = Quaternion.identity;
            powerUp.SetActive(true);

            vacantTiles.RemoveAt(tileIndex);

            gameObjectToSpawnPoint.Add(powerUp, spawnPoint);
            powerUpsInScene.Add(powerUp);

            activePowerUpsCount++;

            float lifeTime = Random.Range(minLifeTime, maxLifeTime);
            StartCoroutine(DespawnPowerUpsAfterTime(powerUp, lifeTime));
        }
    }

    IEnumerator DespawnPowerUpsAfterTime(GameObject powerUp, float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);

        if (powerUp == null)
            yield break;

        ReturnPowerUpsToPool(powerUp);
    }

    public void ReturnPowerUpsToPool(GameObject powerUp)
    {
        if (!gameObjectToSpawnPoint.TryGetValue(powerUp, out Transform spawnPoint))
            return;

        powerUp.SetActive(false);

        vacantTiles.Add(spawnPoint);
        gameObjectToSpawnPoint.Remove(powerUp);
        powerUpsInScene.Remove(powerUp);

        powerUpsPool.Enqueue(powerUp);

        activePowerUpsCount--;
    }

    #endregion

    IEnumerator SpawnScene()
    {
        yield return StartCoroutine(SpawnEnviroment());
        yield return StartCoroutine(SpawnCollectibles());
        yield return StartCoroutine(SpawnPlayer());
        enemySpawnRoutine = StartCoroutine(SpawnEnemy());
    }
    IEnumerator SpawnEnviroment()
    {
        GetMapFromLevelLoader();
        Debug.Log("Spawn Enviroment");
        yield return null;
    }
    IEnumerator SpawnCollectibles()
    {
        coinsActiveInScene = SpawnPrefab(collectibleCount, collectible, gameObjectToSpawnPoint);
        yield return null;
    }


    IEnumerator SpawnPlayer()
    {
        GameObject selectedCarPrefab = GetSelectedCarPrefab();

        if (selectedCarPrefab == null)
        {
            Debug.LogError("No car selected!");
            yield break;
        }

        playerInScene = Instantiate(selectedCarPrefab, playerSpawnPoint.position, Quaternion.identity);
        AssignCameraTarget(playerInScene.transform);

        // Disable movement until countdown finishes
        if (playerInScene.TryGetComponent<PlayerDrifter>(out var drifter))
            drifter.enabled = false;

        DirectionArrow component = playerInScene.GetComponentInChildren<DirectionArrow>();
        if (component != null)
            directionArrow = component.gameObject;

        Debug.Log("Spawned Player with car: " + selectedCarPrefab.name);

        // ⏳ Start countdown after player spawn
        if (countdownRoutine != null)
            StopCoroutine(countdownRoutine);

        countdownRoutine = StartCoroutine(StartLevelCountdown());

        yield return null;
    }

    IEnumerator StartLevelCountdown()
    {
        float time = countdownStart;

        while (time > 0)
        {
            countdownText.text = Mathf.Ceil(time).ToString();
            countdownText.gameObject.SetActive(true);

            yield return new WaitForSeconds(1f);
            time--;
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(0.5f);
        countdownText.gameObject.SetActive(false);

        // ✅ Enable player movement now
        if (playerInScene != null && playerInScene.TryGetComponent<PlayerDrifter>(out var drifter))
            drifter.enabled = true;

        countdownRoutine = null;
    }

    private GameObject GetSelectedCarPrefab()
    {
        if (allAvailableCars == null || allAvailableCars.Count == 0)
        {
            Debug.LogWarning("No available cars assigned in LevelGenerator!");
            return null;
        }

        int selectedCarID = PlayerPrefs.GetInt("SelectedCarID", 1);
        Debug.Log($"Loading car with ID: {selectedCarID}");

        foreach (var carSO in allAvailableCars)
        {
            if (carSO.carID == selectedCarID)
            {
                Debug.Log($"Found selected car: {carSO.CarName}");
                return carSO.CarModel;
            }
        }

        Debug.LogWarning($"Car ID {selectedCarID} not found. Using fallback.");

        if (allAvailableCars.Count > 0 && allAvailableCars[0].CarModel != null)
            return allAvailableCars[0].CarModel;

        return null;
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(10f);

        // Smart AI
        List<GameObject> smartAIInScene = SpawnMultipleEnemyTypes(smartAICount, smartAIPrefabs);

        // Mad AI
        List<GameObject> madAIInScene = SpawnMultipleEnemyTypes(madAICount, madAIPrefabs);

        // Aggressive AI
        List<GameObject> aggressiveAIInScene = SpawnMultipleEnemyTypes(aggressiveAICount, aggressiveAIPrefabs);

        // Combine all into one list
        enemiesInScene.AddRange(smartAIInScene);
        enemiesInScene.AddRange(madAIInScene);
        enemiesInScene.AddRange(aggressiveAIInScene);

        // Set target and enable
        foreach (var enemy in enemiesInScene)
        {
            if (enemy.TryGetComponent(out AIDrift aiDrift))
            {
                aiDrift.target = playerInScene.transform;
            }
            enemy.SetActive(true);
        }

        Debug.Log($"Spawned {enemiesInScene.Count} Enemies");
        yield return null;
    }

    List<GameObject> SpawnMultipleEnemyTypes(int count, List<GameObject> prefabList)
    {
        List<GameObject> spawned = new List<GameObject>();

        if (prefabList == null || prefabList.Count == 0)
        {
            Debug.LogWarning("Enemy prefab list is empty!");
            return spawned;
        }

        for (int i = 0; i < count; i++)
        {
            GameObject randomPrefab = prefabList[Random.Range(0, prefabList.Count)];
            List<GameObject> temp = SpawnPrefab(1, randomPrefab, gameObjectToSpawnPoint);
            spawned.AddRange(temp);
        }

        return spawned;
    }

    void AssignCameraTarget(Transform target)
    {
        vCam.Follow = target;
        vCam.LookAt = target;
    }

    #region  HELPER_METHOD
    public List<GameObject> SpawnPrefab(int count, GameObject prefab, Dictionary<GameObject, Transform> objToPosition)
    {
        List<GameObject> spawned = new List<GameObject>();

        if (vacantTiles.Count == 0)
            return spawned;

        count = Mathf.Min(count, vacantTiles.Count);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, vacantTiles.Count);

            Transform spawnPoint = vacantTiles[index];
            GameObject obj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

            spawned.Add(obj);
            objToPosition.Add(obj, spawnPoint);
            vacantTiles.RemoveAt(index);
        }

        return spawned;
    }

    void DisableEnemy(GameObject enemy)
    {
        if (enemy == null) return;

        if (enemy.TryGetComponent<AIDrift>(out var ai))
            ai.enabled = false;

        if (enemy.TryGetComponent<CollisionHandler>(out var col))
            col.enabled = false;

        if (enemy.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }

    void StopActiveCoroutines()
    {
        if (spawnSceneRoutine != null) StopCoroutine(spawnSceneRoutine);
        if (powerUpsLoopRoutine != null) StopCoroutine(powerUpsLoopRoutine);
        if (enemySpawnRoutine != null) StopCoroutine(enemySpawnRoutine);

        spawnSceneRoutine = null;
        powerUpsLoopRoutine = null;
        enemySpawnRoutine = null;
    }

    #endregion

    void DeactivateCoinFromScene(GameObject targetObject)
    {
        int index = coinsActiveInScene.FindIndex(obj => obj == targetObject);
        if (index != -1)
        {
            coinsCollectedInScene.Add(targetObject);
            coinsActiveInScene.RemoveAt(index);
            Debug.Log("Coin in active scene is : " + coinsActiveInScene.Count);
        }
    }

    void MultiplyEnemiesInScene()
    {
        int randomType = Random.Range(0, 3);
        List<GameObject> temp = new List<GameObject>();

        if (randomType == 0 && smartAIPrefabs != null && smartAIPrefabs.Count > 0)
        {
            temp = SpawnMultipleEnemyTypes(1, smartAIPrefabs);
        }
        else if (randomType == 1 && madAIPrefabs != null && madAIPrefabs.Count > 0)
        {
            temp = SpawnMultipleEnemyTypes(1, madAIPrefabs);
        }
        else if (aggressiveAIPrefabs != null && aggressiveAIPrefabs.Count > 0)
        {
            temp = SpawnMultipleEnemyTypes(1, aggressiveAIPrefabs);
        }

        foreach (var enemySpawned in temp)
        {
            if (enemySpawned.TryGetComponent(out AIDrift aiDrift))
            {
                aiDrift.target = playerInScene.transform;
            }
            enemySpawned.SetActive(true);
        }

        enemiesInScene.AddRange(temp);
    }

    // Load Next Level 
    public void LoadNextLevel()
    {
        StartCoroutine(LNL());
    }

    IEnumerator LNL()
    {
        StopActiveCoroutines();
        yield return StartCoroutine(ResetLevelData());
        levelNumber++;
        LoadLevelData();
        GameManager.Instance.SetTotalCollectibles(collectibleCount);

        InitPowerUpsPool();

        yield return StartCoroutine(SpawnScene());
        powerUpsLoopRoutine = StartCoroutine(PowerUpsWaveLoop());
    }

    // Restart Level 
    public void RestartLevel()
    {
        if (isRestarting) return;
        isRestarting = true;

        StartCoroutine(RL());
    }

    IEnumerator RL()
    {
        StopActiveCoroutines();
        yield return StartCoroutine(ResetLevelData());

        LoadLevelData(); // Reload current level data
        GameManager.Instance.SetTotalCollectibles(collectibleCount);

        InitPowerUpsPool();

        yield return StartCoroutine(SpawnScene());
        powerUpsLoopRoutine = StartCoroutine(PowerUpsWaveLoop());

        isRestarting = false;
    }

    #region  RESETLEVEL
    public void ResetLevel()
    {
        StopActiveCoroutines();
        StartCoroutine(ResetLevelData());
    }

    // IEnumerator ResetLevelData()
    // {
    //     StopActiveCoroutines();

    //     // ----- Clear Tiles -----
    //     foreach (var tile in tileInCurrentScene)
    //         Destroy(tile);
    //     tileInCurrentScene.Clear();

    //     vacantTiles.Clear();

    //     // ----- Clear Enemies -----
    //     foreach (var enemy in enemiesInScene)
    //         Destroy(enemy);
    //     enemiesInScene.Clear();

    //     // ----- Clear Player -----
    //     if (playerInScene != null)
    //         Destroy(playerInScene);

    //     // ----- Clear Coins -----
    //     foreach (var coin in coinsCollectedInScene)
    //         Destroy(coin);
    //     coinsCollectedInScene.Clear();

    //     foreach (var coin in coinsActiveInScene)
    //         Destroy(coin);
    //     coinsActiveInScene.Clear();

    //     // ----- Clear PowerUps -----
    //     foreach (var powerUp in powerUpsInScene)
    //         Destroy(powerUp);

    //     foreach (var powerUp in powerUpsPool)
    //         Destroy(powerUp);

    //     powerUpsInScene.Clear();
    //     powerUpsPool.Clear();

    //     gameObjectToSpawnPoint.Clear();

    //     // ----- Reset GameManager -----
    //     InitPowerUpsPool();

    //     GameManager.Instance.ResetLevelData();
    //     yield return null;
    // }
    IEnumerator ResetLevelData()
    {
        // 🔹 Stop anything that may still be spawning or despawning objects
        StopActiveCoroutines();

        Debug.Log("RESET LEVEL — Clearing scene...");

        // ---------- TILES ----------
        foreach (var t in tileInCurrentScene)
            if (t != null) Destroy(t);
        tileInCurrentScene.Clear();

        vacantTiles.Clear();


        // ---------- ENEMIES ----------
        foreach (var e in enemiesInScene)
            if (e != null) Destroy(e);
        enemiesInScene.Clear();


        // ---------- PLAYER ----------
        if (playerInScene != null)
            Destroy(playerInScene);
        playerInScene = null;
        playerSpawnPoint = null;
        directionArrow = null;


        // ---------- COINS ----------
        foreach (var c in coinsActiveInScene)
            if (c != null) Destroy(c);
        coinsActiveInScene.Clear();

        foreach (var c in coinsCollectedInScene)
            if (c != null) Destroy(c);
        coinsCollectedInScene.Clear();


        // ---------- POWERUPS IN SCENE ----------
        foreach (var p in powerUpsInScene)
            if (p != null) Destroy(p);
        powerUpsInScene.Clear();


        // ---------- POWERUP POOL ----------
        while (powerUpsPool.Count > 0)
        {
            var pooled = powerUpsPool.Dequeue();
            if (pooled != null) Destroy(pooled);
        }
        powerUpsPool.Clear();

        gameObjectToSpawnPoint.Clear();
        activePowerUpsCount = 0;


        // ---------- GAME MANAGER / STATE ----------
        GameManager.Instance.ResetLevelData();

        // ⚠️ Do NOT auto-spawn here — Restart / LoadNextLevel will handle spawning
        // If you WANT reset to also rebuild level, call SpawnScene() AFTER this coroutine externally.

        Debug.Log("RESET LEVEL — Done");
        yield return null;
    }

    #endregion

    void PlayerDeadAndStopGame()
    {
        StopActiveCoroutines();

        // ----- STOP PLAYER -----
        if (playerInScene != null)
        {
            if (playerInScene.TryGetComponent<PlayerDrifter>(out var drifter))
                drifter.enabled = false;

            if (playerInScene.TryGetComponent<CollisionDetection>(out var detection))
                detection.enabled = false;

            if (playerInScene.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            var audio = playerInScene.GetComponent<AudioSource>();
            if (audio) audio.Stop();
        }

        // ----- REMOVE ALL ENEMIES FROM SCENE -----
        foreach (var enemy in enemiesInScene)
        {
            if (enemy != null)
                Destroy(enemy);
        }
        enemiesInScene.Clear();
    }
}