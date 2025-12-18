using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public string name;
    public int rows;
    public int columns;
    public List<int> data; // 1D array instead of 2D
}

[Serializable]
public class LevelCollection
{
    public List<LevelData> levels;
}

public class LevelDataLoader : MonoBehaviour
{
    [SerializeField] private TextAsset jsonFile;
    private LevelCollection levelCollection;
    public static LevelDataLoader Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadLevelData(); 
    }

    void Start()
    {
        // Example usage
        int[][] level1 = GetLevel(0);
        if (level1 != null)
        {
            Debug.Log("Level 1 first row: " + string.Join(", ", level1[0]));
        }
    }

    // Load level data from JSON file
    private void LoadLevelData()
    {
        if (jsonFile == null)
        {
            Debug.LogError("JSON file not assigned!");
            return;
        }

        levelCollection = JsonUtility.FromJson<LevelCollection>(jsonFile.text);

        if (levelCollection == null || levelCollection.levels == null)
        {
            Debug.LogError("Failed to parse level JSON!");
            return;
        }

        Debug.Log($"Loaded {levelCollection.levels.Count} levels");
        
        for(int i = 0; i < levelCollection.levels.Count; i++)
        {
            LevelData level = levelCollection.levels[i];
            Debug.Log($"Level {i + 1}: {level.name}");
            Debug.Log($"  Grid size: {level.rows} rows x {level.columns} columns");
        }
    }

    // Get level data by index and convert 1D to 2D
    public int[][] GetLevel(int levelIndex)
    {
        if (levelCollection == null)
        {
            Debug.LogError("Level data not loaded!");
            return null;
        }

        if (levelIndex < 0 || levelIndex >= levelCollection.levels.Count)
        {
            Debug.LogError($"Invalid level index: {levelIndex}");
            return null;
        }

        LevelData level = levelCollection.levels[levelIndex];
        
        if (level.data == null || level.data.Count == 0)
        {
            Debug.LogError($"Level '{level.name}' has no data!");
            return null;
        }

        // Convert 1D to 2D array
        int[][] levelArray = Convert1DTo2D(level.data, level.rows, level.columns);

        Debug.Log($"Loaded '{level.name}' - Grid: {level.rows}x{level.columns}");
        return levelArray;
    }

    // Get level data by name
    public int[][] GetLevelByName(string levelName)
    {
        if (levelCollection == null)
        {
            Debug.LogError("Level data not loaded!");
            return null;
        }

        LevelData level = levelCollection.levels.Find(l => l.name == levelName);
        
        if (level == null)
        {
            Debug.LogError($"Level '{levelName}' not found!");
            return null;
        }

        return Convert1DTo2D(level.data, level.rows, level.columns);
    }

    // Convert 1D list to 2D array
    private int[][] Convert1DTo2D(List<int> data, int rows, int cols)
    {
        int[][] result = new int[rows][];
        
        for (int i = 0; i < rows; i++)
        {
            result[i] = new int[cols];
            for (int j = 0; j < cols; j++)
            {
                int index = i * cols + j;
                result[i][j] = data[index];
            }
        }
        
        return result;
    }

    // Get the number of levels
    public int GetLevelCount()
    {
        return levelCollection?.levels.Count ?? 0;
    }

    // Get level name by index
    public string GetLevelName(int levelIndex)
    {
        if (levelCollection == null || levelIndex < 0 || levelIndex >= levelCollection.levels.Count)
            return null;
        
        return levelCollection.levels[levelIndex].name;
    }

    // Print entire level grid for debugging
    public void PrintLevelGrid(int levelIndex)
    {
        int[][] level = GetLevel(levelIndex);
        if (level == null) return;

        Debug.Log($"=== {GetLevelName(levelIndex)} ===");
        for (int row = 0; row < level.Length; row++)
        {
            Debug.Log($"Row {row}: [{string.Join(", ", level[row])}]");
        }
    }
}