using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressSaver : MonoBehaviour
{
    public static LevelProgressSaver Instance;
    private string LEVEL_SAVE_KEY = "completedLevel";
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        SetPref();
    }
    void SetPref()
    {
        if (!PlayerPrefs.HasKey(LEVEL_SAVE_KEY))
        {
            PlayerPrefs.SetInt(LEVEL_SAVE_KEY, 1);
        }
    }

    public int LoadData()
    {
        int totalCompletedLevel = 0;
        if (PlayerPrefs.HasKey(LEVEL_SAVE_KEY))
        {
            totalCompletedLevel = PlayerPrefs.GetInt(LEVEL_SAVE_KEY, 0);
        }
        return totalCompletedLevel;
    }
    public void SaveData(int newCompletedLevel)
    {
        if (PlayerPrefs.HasKey(LEVEL_SAVE_KEY))
        {
            PlayerPrefs.SetInt(LEVEL_SAVE_KEY, newCompletedLevel);
        }
    }
}
