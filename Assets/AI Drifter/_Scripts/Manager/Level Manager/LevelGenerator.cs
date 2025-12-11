using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> tile;
    [SerializeField] private Transform Enviroment;

private int[,] matrix = {
    {30, 24, 25, 25, 26, 27, 28, 29, 30, 31, 24, 25, 26, 27, 28, 24, 24, 30, 31, 28},
    {24, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 24},
    {25, 22, 10, 16, 18, 13, 17, 06, 14, 22, 11, 20, 08, 19, 06, 12, 15, 09, 22, 24},
    {26, 22, 19, 07, 14, 11, 21, 10, 00, 22, 13, 08, 17, 15, 16, 01, 18, 20, 22, 28},
    {27, 22, 01, 15, 04, 05, 06, 19, 17, 22, 07, 14, 16, 00, 20, 21, 10, 08, 22, 29},
    {28, 22, 16, 17, 02, 03, 10, 06, 18, 22, 21, 01, 19, 11, 09, 06, 12, 15, 22, 25},
    {29, 22, 07, 00, 06, 17, 09, 11, 15, 22, 10, 06, 06, 08, 14, 12, 18, 21, 22, 27},
    {30, 22, 18, 06, 21, 14, 13, 07, 19, 22, 08, 17, 15, 16, 06, 09, 10, 06, 22, 26},
    {31, 22, 06, 18, 10, 15, 00, 12, 09, 22, 14, 13, 20, 06, 17, 19, 08, 16, 22, 24},
    {30, 23, 23, 23, 23, 23, 23, 23, 23, 06, 23, 23, 23, 23, 23, 23, 23, 23, 23, 28},
    {28, 22, 14, 09, 16, 12, 07, 20, 11, 22, 08, 11, 06, 17, 21, 15, 13, 18, 22, 30},
    {24, 22, 12, 13, 17, 00, 18, 01, 15, 22, 06, 10, 11, 14, 19, 07, 20, 06, 22, 31},
    {26, 22, 11, 14, 09, 19, 06, 16, 10, 22, 17, 15, 13, 18, 12, 20, 07, 01, 22, 30},
    {27, 22, 08, 12, 15, 07, 19, 21, 11, 22, 09, 18, 00, 10, 06, 06, 19, 13, 22, 29},
    {25, 22, 13, 01, 16, 18, 14, 09, 07, 22, 15, 21, 17, 12, 00, 04, 05, 19, 22, 28},
    {29, 22, 17, 10, 11, 06, 01, 00, 13, 22, 12, 07, 21, 06, 08, 02, 03, 14, 22, 27},
    {28, 22, 20, 13, 08, 12, 15, 18, 14, 22, 16, 09, 10, 17, 07, 01, 00, 11, 22, 26},
    {24, 22, 16, 21, 14, 10, 17, 13, 18, 22, 19, 06, 01, 07, 15, 11, 12, 20, 22, 25},
    {24, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 22, 24},
    {24, 24, 25, 25, 26, 27, 28, 29, 30, 31, 24, 25, 26, 27, 28, 24, 24, 30, 31, 26}
};

    private List<GameObject> tileInCurrentScene = new List<GameObject>();
    int width = 30;
    int height = 30;
    void Start()
    {
        // GenerateGround();
        Debug.Log(tile.Count);
    }

    [ContextMenu("Generate Ground")]
    void GenerateGround()
    {
        for(int i = 0 ; i < 20 ;i++)
        {
            for(int j = 0 ; j < 20 ; j++)
            {
                float spawnX = i * width;
                float spawnZ = j * height;
                Debug.Log("Width is : " + width + " " + "height is : "+height);
                Vector3 spawnPos = new Vector3(spawnX,0,spawnZ);

                GameObject obj = Instantiate(tile[matrix[i,j]],spawnPos,Quaternion.identity);
                if(i==0)
                {
                    obj.transform.rotation = Quaternion.Euler(0,-90,0);
                }
                else if(j==0)
                {
                    obj.transform.rotation = Quaternion.Euler(0,-180,0);
                }
                else if(i==19)
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
}
