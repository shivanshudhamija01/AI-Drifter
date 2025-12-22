using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TempLevelGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> tile;
    [SerializeField] private Transform Enviroment;
    [SerializeField] private string spawnPoint = "SpawnPoint";
    [SerializeField] private int levelNumber = 2;
    private int width = 30;
    private int height = 30;
    private int[][] matrix;

    [ContextMenu("Generate Ground")]
   void GetMapFromLevelLoader()
    {
        matrix = LevelDataLoader.Instance.GetLevel(levelNumber);
        Debug.Log(matrix.Length);
        Debug.Log(matrix[0].Length);
        GenerateGround();
    }


     void GenerateGround()
    {
        for (int i = 0; i < matrix.Length; i++)
        {
            for (int j = 0; j < matrix[0].Length; j++)
            {
                Vector3 spawnPos = new Vector3(i * width, 0, j * height);
                GameObject obj = Instantiate(tile[matrix[i][j]], spawnPos, Quaternion.identity);
                if(i==0)
                {
                    obj.transform.rotation = Quaternion.Euler(0,-90,0);
                }
                else if(j==0)
                {
                    obj.transform.rotation = Quaternion.Euler(0,-180,0);
                }
                else if(i==(matrix.Length-1))
                {
                    obj.transform.rotation = Quaternion.Euler(0,90,0);
                }
            }
        }

        NavMeshSurface navMesh = Enviroment.GetComponent<NavMeshSurface>();
        if (navMesh != null)
        {
            navMesh.RemoveData();
            navMesh.BuildNavMesh();
        }
    }

    
}
