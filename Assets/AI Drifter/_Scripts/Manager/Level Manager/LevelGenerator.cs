using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> tile;
    [SerializeField] private Transform Enviroment;

private int[,] matrix = {
    {09, 11, 15, 10, 07, 20, 18, 14, 01, 17, 06, 16, 12, 13, 19, 08, 21, 00, 06, 06},
    {17, 13, 06, 19, 00, 06, 11, 08, 16, 12, 15, 07, 14, 21, 10, 18, 09, 06, 12, 01},
    {15, 07, 10, 16, 18, 13, 17, 06, 14, 01, 11, 20, 08, 19, 06, 12, 15, 09, 00, 21},
    {12, 06, 19, 07, 14, 11, 21, 10, 00, 06, 13, 08, 17, 15, 16, 01, 18, 20, 06, 16},
    {06, 18, 01, 15, 12, 09, 06, 19, 17, 13, 07, 14, 16, 00, 20, 21, 10, 08, 11, 17},
    {00, 14, 16, 17, 13, 08, 10, 06, 18, 07, 21, 01, 19, 11, 09, 06, 12, 15, 20, 06},
    {13, 19, 07, 00, 06, 17, 09, 11, 15, 16, 10, 06, 06, 08, 14, 12, 18, 21, 01, 10},
    {11, 12, 18, 06, 21, 14, 13, 07, 19, 00, 08, 17, 15, 16, 06, 09, 10, 06, 13, 15},
    {07, 01, 06, 18, 10, 15, 00, 12, 09, 11, 14, 13, 20, 06, 17, 19, 08, 16, 21, 07},
    {14, 08, 17, 11, 19, 21, 16, 15, 06, 10, 12, 09, 07, 01, 13, 00, 17, 14, 18, 06},
    {19, 10, 14, 09, 16, 12, 07, 20, 02, 03, 08, 11, 06, 17, 21, 15, 13, 18, 00, 06},
    {08, 15, 12, 13, 17, 00, 18, 01, 04, 05, 06, 10, 11, 14, 19, 07, 20, 06, 16, 21},
    {21, 00, 11, 14, 09, 19, 06, 16, 10, 08, 17, 15, 13, 18, 12, 20, 07, 01, 06, 07},
    {16, 17, 08, 12, 15, 07, 19, 21, 11, 14, 09, 18, 00, 10, 06, 06, 19, 13, 17, 01},
    {10, 06, 13, 01, 16, 18, 14, 09, 07, 06, 15, 21, 17, 12, 00, 08, 11, 19, 10, 20},
    {18, 16, 17, 10, 11, 06, 01, 00, 13, 19, 12, 07, 21, 06, 08, 15, 06, 14, 17, 09},
    {06, 19, 20, 13, 08, 12, 15, 18, 14, 21, 16, 09, 10, 17, 07, 01, 00, 11, 19, 16},
    {00, 09, 16, 21, 14, 10, 17, 13, 18, 06, 19, 06, 01, 07, 15, 11, 12, 20, 08, 17},
    {13, 20, 09, 17, 18, 01, 12, 14, 06, 15, 00, 10, 19, 06, 11, 16, 21, 07, 10, 08},
    {16, 06, 10, 11, 07, 09, 20, 19, 21, 08, 13, 16, 14, 00, 17, 18, 15, 12, 01, 06}
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
