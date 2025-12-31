using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private ParticleSystem firework;
    [SerializeField] private TextMeshProUGUI collectibleCoinCount;
    private int totalCollectibles;
    private int collected;
    private int rewardValue = 0;
    private int currentLevelNumber = 0;

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
        collectibleCoinCount.text = $"{collected} / {total}";
        Debug.Log($"Total Collectibles in Level: {total}");
    }

    public void AddCollectible()
    {
        collected++;
        collectibleCoinCount.text = $"{collected} / {totalCollectibles}";
        Debug.Log($"Collected {collected}/{totalCollectibles}");

        if (collected >= totalCollectibles)
        {
            LevelClear();
        }
    }
    public void ResetLevelData()
    {
        collected = 0;
        currentLevelNumber = 0;
        rewardValue = 0;
    }

    void LevelClear()
    {
        HurrayLevelCompleted();
        currentLevelNumber++;
        AssignRewardAndUpdateLevelCompleteCount(currentLevelNumber);
        AudioServices.Instance.PlayAudio.Invoke(Enums.Audios.levelClear);
        LevelServices.Instance.OnLevelCompleted?.Invoke();
        ResetLevelData();
    }
    void HurrayLevelCompleted()
    {
        firework.Play();
    }
    public void RestartLevel()
    {
        ResetLevelData();
        LevelServices.Instance.OnLevelRestarted?.Invoke();
    }

    public void SetLevelNumber(int value)
    {
        currentLevelNumber = value;
        Debug.Log("Current Level according to game manager is : " + currentLevelNumber);
    }
    public void SetRewardValue(int value)
    {
        rewardValue = value;
        Debug.Log("Reward value for this level according to game mananger is : " + rewardValue);
    }
    public void AssignRewardAndUpdateLevelCompleteCount(int lvlNumber)
    {
        int currentLevel = LevelProgressSaver.Instance.LoadData() - 1;
        Debug.Log("Current Level Number is : " + currentLevel);
        Debug.Log("Now New Level Number Is : " + lvlNumber);
        if (lvlNumber > currentLevel)
        {
            Debug.Log("Assign reward and update reward");
            LevelProgressSaver.Instance.SaveData(lvlNumber + 1);
            LevelProgressSaver.Instance.SaveCoin(rewardValue);
        }
    }
}
