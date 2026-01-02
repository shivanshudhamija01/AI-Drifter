using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource enemy;
    [SerializeField] private AudioSource sfx;


    [SerializeField] private AudioClip coinPickedUp;
    [SerializeField] private AudioClip guiClicked;
    [SerializeField] private AudioClip blast;
    [SerializeField] private AudioClip collision;
    [SerializeField] private AudioClip levelCleared;
    [SerializeField] private AudioClip levelFailed;
    [SerializeField] private AudioClip healthUp;
    [SerializeField] private AudioClip chemicalEffect;
    [SerializeField] private AudioClip powerAttack;
    [SerializeField] private AudioClip shieldEffect;
    [SerializeField] private AudioClip policeSiren;
    void OnEnable()
    {
        AudioServices.Instance.PlayAudio.AddListener(PlayAudioOnParticularEvent);
        PlayerServices.Instance.OnPlayerDead.AddListener(BurningAudio);
        LevelServices.Instance.LevelRestart.AddListener(ResetAudioSystem);
        LevelServices.Instance.LoadNextLevel.AddListener(ResetAudioSystem);
        UIServices.Instance.onHomeButtonClicked.AddListener(ResetAudioSystem);
        UIServices.Instance.resetAudioOnPauseButton.AddListener(ResetAudioSystem);
    }
    void OnDisable()
    {
        AudioServices.Instance.PlayAudio.RemoveListener(PlayAudioOnParticularEvent);
        PlayerServices.Instance.OnPlayerDead.RemoveListener(BurningAudio);
        LevelServices.Instance.LevelRestart.RemoveListener(ResetAudioSystem);
        LevelServices.Instance.LoadNextLevel.RemoveListener(ResetAudioSystem);
        UIServices.Instance.onHomeButtonClicked.RemoveListener(ResetAudioSystem);
        UIServices.Instance.resetAudioOnPauseButton.RemoveListener(ResetAudioSystem);
    }

    private void ResetAudioSystem()
    {
        sfx.loop = false;
    }

    private void BurningAudio()
    {
        sfx.loop = true;
        sfx.clip = chemicalEffect;
        sfx.Play();
    }

    void PlayAudioOnParticularEvent(Enums.Audios audioClip)
    {
        sfx.Stop();
        switch (audioClip)
        {
            case Enums.Audios.coinPickedUp:
                sfx.clip = coinPickedUp;
                break;

            case Enums.Audios.blast:
                sfx.clip = blast;
                break;

            case Enums.Audios.collision:
                sfx.clip = collision;
                break;

            case Enums.Audios.guiClicked:
                sfx.clip = guiClicked;
                break;

            case Enums.Audios.levelClear:
                sfx.clip = levelCleared;
                break;

            case Enums.Audios.levelFailed:
                sfx.clip = levelFailed;
                break;

            case Enums.Audios.healthUp:
                sfx.clip = healthUp;
                break;
            case Enums.Audios.chemical:
                sfx.clip = chemicalEffect;
                break;

            case Enums.Audios.shield:
                sfx.clip = shieldEffect;
                break;
            case Enums.Audios.powerAttack:
                sfx.clip = powerAttack;
                break;
        }
        sfx.Play();
    }
}
