using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject tile;
    [SerializeField] private Transform Enviroment;

    private int[,] matrix = {{1,1,1},{1,1,1}};
    private List<GameObject> tileInCurrentScene = new List<GameObject>();
    int width = 10;
    int height = 10;
    void Start()
    {
        // GenerateGround();
    }

    [ContextMenu("Generate Ground")]
    void GenerateGround()
    {
        for(int i = 0 ; i < 2 ;i++)
        {
            for(int j = 0 ; j < 3 ; j++)
            {
                float spawnX = i * width;
                float spawnZ = j * height;
                Vector3 spawnPos = new Vector3(spawnX,0,spawnZ);

                GameObject obj = Instantiate(tile,spawnPos,Quaternion.identity);
                tileInCurrentScene.Add(obj);
                obj.transform.SetParent(Enviroment);
            }
        }
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
