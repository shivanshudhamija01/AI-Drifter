using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> tile;
    [SerializeField] private Transform Enviroment;
    [SerializeField] private GameObject gola;
    private int[][] matrix;
    private List<GameObject> tileInCurrentScene = new List<GameObject>();
    private List<Transform> vacantTiles;
    private List<GameObject> collectibleInScene;
    private List<GameObject> powerUpsInScene;
    private List<GameObject> playerInScene;
    private Dictionary<GameObject,Transform> gameObjectToPosition;
    private string spawnPoint = "SpawnPoint";
    private int width = 30;
    private int height = 30;
    [SerializeField] private GameObject collectiblePrefab;
[SerializeField] private int collectiblePoolSize = 10;

[SerializeField] private int maxCollectiblesInScene = 5;
[SerializeField] private float minSpawnInterval = 3f;
[SerializeField] private float maxSpawnInterval = 6f;
[SerializeField] private float collectibleLifeTime = 8f;

private Queue<GameObject> collectiblePool = new Queue<GameObject>();
private List<GameObject> collectiblesInScene = new List<GameObject>();
private Dictionary<GameObject, Transform> collectibleToSpawnPoint = new Dictionary<GameObject, Transform>();


    void Awake()
    {
        vacantTiles = new List<Transform>();
    }
    void Start()
    {
        // GenerateGround();
        GetMapFromLevelLoader();
        // SpawnScene();
        InitCollectiblePool();
        StartCoroutine(SpawnScene());
        Debug.Log(tile.Count);
    }

    [ContextMenu("Generate Ground")]
    void GenerateGround()
    {
        for(int i = 0 ; i < 10 ;i++)
        {
            for(int j = 0 ; j < 10 ; j++)
            {
                float spawnX = i * width;
                float spawnZ = j * height;
                Vector3 spawnPos = new Vector3(spawnX,0,spawnZ);

                GameObject obj = Instantiate(tile[matrix[i][j]],spawnPos,Quaternion.identity);
                Transform point = obj.transform.Find(spawnPoint);
                if(point != null)
                {
                    vacantTiles.Add(point);
                }
                if(i==0)
                {
                    obj.transform.rotation = Quaternion.Euler(0,-90,0);
                }
                else if(j==0)
                {
                    obj.transform.rotation = Quaternion.Euler(0,-180,0);
                }
                else if(i==9)
                {
                    obj.transform.rotation = Quaternion.Euler(0,90,0);
                }
                tileInCurrentScene.Add(obj);
                obj.transform.SetParent(Enviroment);
            }
        }
        NavMeshSurface navMeshSurface = Enviroment.GetComponent<NavMeshSurface>();
        if(navMeshSurface!=null)
        {
            navMeshSurface.RemoveData();
        }
        navMeshSurface.BuildNavMesh();
        //GetMapFromLevelLoader();
        StartCoroutine(SpawnCollectible());
    }

    private void GetMapFromLevelLoader()
    {
        int[][] map = LevelDataLoader.Instance.GetLevel(0);
        // for(int i = 0;i<map.Length;i++)
        // {
        //     for(int j=0;j<map[0].Length;j++)
        //     {
        //         Debug.Log(map[i][j]);
        //     }
        // }
        matrix = map;
        GenerateGround();
    }

    [ContextMenu("Clear Ground")]
    void ClearGround()
    {
        for (int i = tileInCurrentScene.Count - 1; i >= 0; i--)
        {
    // Add your specific condition here (e.g., check distance, health, etc.)
            if (tileInCurrentScene[i] != null)
            {
            DestroyImmediate(tileInCurrentScene[i]);
            tileInCurrentScene.RemoveAt(i);
            }
        }
    }
    // void SpawnCollectible()
    // {
    //     // Tranverse the count of collectible from the LevelSO and place it according to the spawning point 
    //     // After spawning it , remove that spawn point from the list 

    //     // When it gets collected then put back this spawn point to the list back
    //     collectibleInScene = SpawnPrefab(5,gola,gameObjectToPosition);
        
    // }
    void SpanwPowerUps()
    {
        // From the updated list , spawn the power ups in the scene and after 
    }

    public List<GameObject> SpawnPrefab(int Count, GameObject prefab, Dictionary<GameObject,Transform> objToPosition)
    {
        Debug.Log(vacantTiles.Count);
        List<GameObject> objSpawnedInScene = new List<GameObject>();
        int gap = Random.Range(1,vacantTiles.Count);
        int startingIndex = Random.Range(2,vacantTiles.Count);
        int totalvacantTiles = vacantTiles.Count;
        for(int i=0;i<Count;i++)
        {
            int index = (startingIndex + i * gap) % totalvacantTiles;
            Vector3 spawnPosition = vacantTiles[index].position;
            GameObject temp = Instantiate(prefab, spawnPosition, Quaternion.identity);
            objSpawnedInScene.Add(temp);
            vacantTiles.RemoveAt(index);
        }
        Debug.Log(vacantTiles.Count);
        return objSpawnedInScene;

    }


    IEnumerator SpawnScene()
    {
        yield return null;
    }
    IEnumerator SpawnEnviroment()
    {
        // This is Done 
        yield return null;
    }
    IEnumerator SpawnCollectible()
    {
        // Spawn collectibles 
        // in a spawn collectible, i will write it like this , that
        // in a cycle , will spawn the  
        // yield return null;
        while (true)
    {
        yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

        if (vacantTiles.Count == 0)
            continue;

        if (collectiblesInScene.Count >= maxCollectiblesInScene)
            continue;

        if (collectiblePool.Count == 0)
            continue;

        int index = Random.Range(0, vacantTiles.Count);
        Transform spawnPoint = vacantTiles[index];

        GameObject collectible = collectiblePool.Dequeue();

        collectible.transform.position = spawnPoint.position;
        collectible.transform.rotation = Quaternion.identity;
        collectible.SetActive(true);

        collectiblesInScene.Add(collectible);
        collectibleToSpawnPoint[collectible] = spawnPoint;

        vacantTiles.RemoveAt(index);

        StartCoroutine(DespawnCollectibleAfterTime(collectible));
    }
    }
    void InitCollectiblePool()
    {
        for (int i = 0; i < collectiblePoolSize; i++)
        {
            GameObject obj = Instantiate(collectiblePrefab);
            obj.SetActive(false);
            collectiblePool.Enqueue(obj);
        }
    }
    IEnumerator DespawnCollectibleAfterTime(GameObject collectible)
    {
        yield return new WaitForSeconds(collectibleLifeTime);

        if (collectible == null || !collectible.activeSelf)
            yield break;
        ReturnCollectibleToPool(collectible);
    }
    public void ReturnCollectibleToPool(GameObject collectible)
    {
        if (!collectibleToSpawnPoint.TryGetValue(collectible, out Transform spawnPoint))
            return;

        collectible.SetActive(false);

        vacantTiles.Add(spawnPoint);

        collectiblesInScene.Remove(collectible);
        collectibleToSpawnPoint.Remove(collectible);

        if (!collectiblePool.Contains(collectible))
            collectiblePool.Enqueue(collectible);
    }
    IEnumerator SpawnPowerUps()
    {
        yield return null;
    }
    IEnumerator SpawnPlayer()
    {
        // Spawn player is just a single task ,
        // Traverse all the player from the level so, and after that check which player is selected 
        // From the vacant tiles array , spawn the player 
        yield return null;
    }
    IEnumerator SpawnEnemy()
    {
        // Spawn different - different types of enemies according to the Count in LevelSO;
        // Store all these enemies in the list 
        // 
        yield return null;
    }
}

// Level SO pass houga 
// First Generate the map 
// Genrerate the collectible 
// Generate the power ups 
// Generate the player 
// Generate the enemies and set the target as player


/*
    Logic for Collectible Spawner 

    // First spawn all the collectible and store it in a list
    
*/