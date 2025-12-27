using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using Cinemachine;


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

    private int[][] matrix;
    private List<GameObject> tileInCurrentScene = new List<GameObject>();
    private List<Transform> vacantTiles = new List<Transform>();

    [Header("Collectible")]
    [SerializeField] private GameObject collectible;
    [SerializeField] private int collectibleCount;

    [Header("PowerUps Wave Settings")]
    [SerializeField] private List<GameObject> powerUpsList;
    [SerializeField] private int powerUpsPerWave = 5;
    [SerializeField] private float minLifeTime = 4f;
    [SerializeField] private float maxLifeTime = 8f;
    [SerializeField] private float respawnDelay = 10f;

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

    #region  Temp
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject smartAIPrefab;
    [SerializeField] private GameObject madAIPrefab;
    [SerializeField] private GameObject aggressiveAIPrefab;
    #endregion


    // Track running coroutines
    private Coroutine spawnSceneRoutine;
    private Coroutine powerUpsLoopRoutine;


    void OnEnable()
    {
        PlayerServices.Instance.OnCoinPickedUp.AddListener(DeactivateCoinFromScene);
        PlayerServices.Instance.OnGhostCollected.AddListener(MultiplyEnemiesInScene);
        LevelServices.Instance.OnLevelCompleted.AddListener(ResetLevel);
        LevelServices.Instance.OnLevelRestarted.AddListener(ResetLevel);
    }
    void OnDisable()
    {
        PlayerServices.Instance.OnCoinPickedUp.RemoveListener(DeactivateCoinFromScene);
        PlayerServices.Instance.OnGhostCollected.RemoveListener(MultiplyEnemiesInScene);
        LevelServices.Instance.OnLevelCompleted.RemoveListener(ResetLevel);
        LevelServices.Instance.OnLevelRestarted.RemoveListener(ResetLevel);
    }
    void Start()
    {
        GameManager.Instance.SetTotalCollectibles(10);
        // GetMapFromLevelLoader();
        spawnSceneRoutine = StartCoroutine(SpawnScene());
        powerUpsLoopRoutine = StartCoroutine(PowerUpsWaveLoop());
        InitPowerUpsPool();
    }
    void Update()
    {
        if (directionArrow == null) return;

        PointTowardsNearbyCoin();
    }

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
        for (int i = 0; i < powerUpsList.Count; i++)
        {
            GameObject obj = Instantiate(powerUpsList[i]);
            obj.SetActive(false);
            powerUpsPool.Enqueue(obj);
        }
    }

    #endregion

    #region PowerUps Wave System

    IEnumerator PowerUpsWaveLoop()
    {
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
        yield return StartCoroutine(SpawnEnemy());
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
        // Take a center point and always try to spawn the player at the center of the enviroment 

        playerInScene = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
        AssignCameraTarget(playerInScene.transform);
        DirectionArrow component = playerInScene.GetComponentInChildren<DirectionArrow>();
        if (component != null)
        {
            directionArrow = component.gameObject;
        }
        Debug.Log("Spawn Player");
        yield return null;
    }
    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(10f);
        // Smart AI
        List<GameObject> smartAIInScene = SpawnPrefab(1, smartAIPrefab, gameObjectToSpawnPoint);
        // Mad AI
        List<GameObject> madAIInScene = SpawnPrefab(1, madAIPrefab, gameObjectToSpawnPoint);
        // Aggressive AI
        List<GameObject> aggressiveAIInScene = SpawnPrefab(1, aggressiveAIPrefab, gameObjectToSpawnPoint);

        // Now need to combine all three list in a single list 
        enemiesInScene.AddRange(smartAIInScene);
        enemiesInScene.AddRange(madAIInScene);
        enemiesInScene.AddRange(aggressiveAIInScene);


        // Traverse the list and set the target reference to the player and also enable them 
        for (int i = 0; i < enemiesInScene.Count; i++)
        {
            GameObject temp = enemiesInScene[i];
            if (temp.TryGetComponent(out AIDrift aiDrift))
            {
                aiDrift.target = playerInScene.transform;
            }
            temp.SetActive(true);
        }

        Debug.Log("Spawn Enemy");
        yield return null;
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
            vacantTiles.RemoveAt(index);
        }

        return spawned;
    }

    void StopActiveCoroutines()
    {
        if (spawnSceneRoutine != null)
            StopCoroutine(spawnSceneRoutine);

        if (powerUpsLoopRoutine != null)
            StopCoroutine(powerUpsLoopRoutine);
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
        List<GameObject> temp;
        if (randomType == 0)
        {
            temp = SpawnPrefab(1, smartAIPrefab, gameObjectToSpawnPoint);
        }
        else if (randomType == 1)
        {
            temp = SpawnPrefab(1, madAIPrefab, gameObjectToSpawnPoint);
        }
        else
        {
            temp = SpawnPrefab(1, aggressiveAIPrefab, gameObjectToSpawnPoint);
        }
        for (int i = 0; i < temp.Count; i++)
        {
            GameObject enemySpawned = temp[i];
            if (enemySpawned.TryGetComponent(out AIDrift aiDrift))
            {
                aiDrift.target = playerInScene.transform;
            }
            enemySpawned.SetActive(true);
        }
        enemiesInScene.AddRange(temp);
        return;
    }

    // Load Next Level 
    public void LoadNextLevel()
    {
        // StopAllCoroutines();
        StartCoroutine(LNL());
    }
    IEnumerator LNL()
    {
        StopActiveCoroutines();
        yield return StartCoroutine(ResetLevelData());
        levelNumber++;
        yield return StartCoroutine(SpawnScene());
        yield return StartCoroutine(PowerUpsWaveLoop());
    }
    // Restart Level 
    public void RestartLevel()
    {
        // StopAllCoroutines();
        StartCoroutine(RL());
    }
    IEnumerator RL()
    {
        StopActiveCoroutines();
        yield return StartCoroutine(ResetLevelData());
        yield return StartCoroutine(SpawnScene());
        yield return StartCoroutine(PowerUpsWaveLoop());
    }
    #region  RESETLEVEL
    public void ResetLevel()
    {
        StartCoroutine(ResetLevelData());

        // ----- Restart Level -----
        // StartCoroutine(SpawnScene());
        // StartCoroutine(PowerUpsWaveLoop());
    }
    IEnumerator ResetLevelData()
    {
        yield return new WaitForSeconds(1f);
        // StopAllCoroutines();

        // ----- Clear Tiles -----
        foreach (var tile in tileInCurrentScene)
            Destroy(tile);
        tileInCurrentScene.Clear();

        vacantTiles.Clear();

        // ----- Clear Enemies -----
        foreach (var enemy in enemiesInScene)
            Destroy(enemy);
        enemiesInScene.Clear();

        // ----- Clear Player -----
        if (playerInScene != null)
            Destroy(playerInScene);


        // ----- Clear Coins -----
        foreach (var coin in coinsCollectedInScene)
            Destroy(coin);
        coinsCollectedInScene.Clear();

        // Need to add a loop that can clear all the coin whether collected or not 
        foreach (var coin in coinsActiveInScene)
            Destroy(coin);
        coinsActiveInScene.Clear();

        // ----- Clear PowerUps -----
        foreach (var powerUp in powerUpsInScene)
            Destroy(powerUp);

        foreach (var powerUp in powerUpsPool)
            Destroy(powerUp);

        powerUpsInScene.Clear();
        powerUpsPool.Clear();

        // ReturnPowerUpsToPool(powerUp);
        // powerUpsInScene.Clear();
        // gameObjectToSpawnPoint.Clear();
        // activePowerUpsCount = 0; 

        // ----- Reset GameManager -----
        // InitPowerUpsPool();

        GameManager.Instance.ResetLevelData();
    }
    #endregion
}
