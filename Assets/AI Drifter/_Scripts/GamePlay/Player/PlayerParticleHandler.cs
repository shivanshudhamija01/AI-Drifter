using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleHandler : MonoBehaviour
{
    [SerializeField] private ParticleSystem shieldEffect;
    [SerializeField] private ParticleSystem chemicalEffect;
    [SerializeField] private ParticleSystem healthUpEffect;
    [SerializeField] private ParticleSystem ghostEffect;
    [SerializeField] private ParticleSystem bombBlastEffect;
    [SerializeField] private ParticleSystem powerAttackEffect;
    [SerializeField] private ParticleSystem speedUpEffect;
    [SerializeField] private ParticleSystem burningEffect;
    private ParticleSystem currentEffect;


    void OnEnable()
    {
        PlayerServices.Instance.OnCollectiblePicked.AddListener(HandleSpecificCollectibleCollected);
        PlayerServices.Instance.OnPlayerDead.AddListener(BurnCar);
        LevelServices.Instance.LevelRestart.AddListener(ResetBurnEffect);
        LevelServices.Instance.LoadNextLevel.AddListener(ResetBurnEffect);
    }
    void OnDisable()
    {
        PlayerServices.Instance.OnCollectiblePicked.RemoveListener(HandleSpecificCollectibleCollected);
        PlayerServices.Instance.OnPlayerDead.RemoveListener(BurnCar);
        LevelServices.Instance.LevelRestart.RemoveListener(ResetBurnEffect);
        LevelServices.Instance.LoadNextLevel.RemoveListener(ResetBurnEffect);
    }

    private void BurnCar()
    {
        var main = burningEffect.main;
        main.loop = true;
        burningEffect.Play();
    }
    private void ResetBurnEffect()
    {
        var main = burningEffect.main;
        main.loop = false;
    }
    void HandleSpecificCollectibleCollected(Enums.Collectibles collectible)
    {
        switch (collectible)
        {
            case Enums.Collectibles.shield:
                PlayEffect(shieldEffect);
                break;

            case Enums.Collectibles.chemical:
                PlayEffect(chemicalEffect);
                break;

            case Enums.Collectibles.healthUp:
                PlayEffect(healthUpEffect);
                break;

            case Enums.Collectibles.ghost:
                PlayEffect(ghostEffect);
                break;

            case Enums.Collectibles.blast:
                PlayEffect(bombBlastEffect);
                break;

            case Enums.Collectibles.powerAttack:
                PlayEffect(powerAttackEffect);
                break;

            case Enums.Collectibles.speedUp:
                PlayEffect(speedUpEffect);
                break;

            // case Enums.Collectibles.coin:
            // case Enums.Collectibles.timer:
            //     break;

            default:
                Debug.LogWarning($"No particle effect assigned for {collectible}");
                break;
        }
    }
    private void PlayEffect(ParticleSystem newEffect)
    {
        if (newEffect == null)
            return;

        // Stop previously playing effect
        if (currentEffect != null && currentEffect.isPlaying)
        {
            currentEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        // Play new effect
        currentEffect = newEffect;
        currentEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        currentEffect.Play();
    }

}
