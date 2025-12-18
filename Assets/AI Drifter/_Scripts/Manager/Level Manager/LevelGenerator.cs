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
    // private int [,] matrix =
    // {
    // {30, 24, 25, 25, 26, 27, 28, 29, 30, 28},
    // {24, 23, 23, 23, 23, 23, 23, 23, 23, 24},
    // {25, 22, 0, 0, 16, 0, 15, 0, 22, 24},
    // {26, 22, 0, 11, 0, 0, 15, 0, 22, 28},
    // {27, 22, 11, 0, 9, 0, 0, 9, 22, 29},
    // {28, 22, 0, 0, 16, 6, 16, 0, 22, 25},
    // {29, 22, 0, 0, 11, 12, 0, 0, 22, 27},
    // {30, 22, 16, 0, 15, 0, 12, 0, 22, 26},
    // {24, 23, 23, 23, 23, 23, 23, 23, 22, 24},
    // {24, 24, 25, 25, 26, 27, 28, 29, 30, 26}
        
    // };
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

    void Awake()
    {
        vacantTiles = new List<Transform>();
    }
    void Start()
    {
        // GenerateGround();
        GetMapFromLevelLoader();
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
    }

    private void GetMapFromLevelLoader()
    {
        int[][] map = LevelDataLoader.Instance.GetLevelByName("Level 1");
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
    void SpawnCollectible()
    {
        // Tranverse the count of collectible from the LevelSO and place it according to the spawning point 
        // After spawning it , remove that spawn point from the list 

        // When it gets collected then put back this spawn point to the list back
        collectibleInScene = SpawnPrefab(5,gola,gameObjectToPosition);
        
    }
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
}

// Level SO pass houga 
// First Generate the map 
// Genrerate the collectible 
// Generate the power ups 
// Generate the player 
// Generate the enemies and set the target as player