using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int totalCollectibles;
    private int collected;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetTotalCollectibles(int total)
    {
        totalCollectibles = total;
        collected = 0;
        Debug.Log($"Total Collectibles in Level: {total}");
    }

    public void AddCollectible()
    {
        collected++;
        Debug.Log($"Collected {collected}/{totalCollectibles}");

        if (collected >= totalCollectibles)
        {
            LevelClear();
        }
    }

    // ---------- RESET ----------
    public void ResetLevelData()
    {
        collected = 0;
    }

    void LevelClear()
    {
        ResetLevelData();
        LevelServices.Instance.OnLevelCompleted?.Invoke();
    }

    public void RestartLevel()
    {
        ResetLevelData();
        LevelServices.Instance.OnLevelRestarted?.Invoke();
    }
}
