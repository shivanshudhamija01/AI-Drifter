using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // [SerializeField] private ParticleSystem firework;
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
        // HurrayLevelCompleted();
        AudioServices.Instance.PlayAudio.Invoke(Enums.Audios.levelClear);
        LevelServices.Instance.OnLevelCompleted?.Invoke();
    }
    // void HurrayLevelCompleted()
    // {
    //     float cameraSize = Camera.main.orthographicSize;
    //     Vector3 initialPos = firework.transform.position;
    //     firework.transform.position = new Vector3(Camera.main.transform.position.x, -1 * Camera.main.transform.position.y, initialPos.z);
    //     firework.transform.localScale = new Vector3(cameraSize * 0.2f, cameraSize * 0.2f, cameraSize * 0.2f);
    //     firework.Play();
    // }
    public void RestartLevel()
    {
        ResetLevelData();
        LevelServices.Instance.OnLevelRestarted?.Invoke();
    }
}
