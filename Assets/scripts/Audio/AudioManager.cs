using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    public PlayAudioEvent FXEvent;
    public PlayAudioEvent BGMEvent;
    public FloatEventSO volumeEvent;
    public VoidEventSO pauseEventSO;
    public FloatEventSO syncVolumEventSO;
    public AudioSource BGMSource;
    public AudioSource FXSource;
    public AudioMixer mixer;
    private void OnEnable()
    {
        FXEvent.OnEventRaised += OnFXEvent;
        BGMEvent.OnEventRaised += OnBGMEvent;
        volumeEvent.OnEventRaised += OnVolumeEvent;
        pauseEventSO.OnEventRaised += OnPauseEvent;
    }

    private void OnBGMEvent(AudioClip audioClip)
    {
        BGMSource.clip = audioClip;
        BGMSource.Play();
    }

    private void OnFXEvent(AudioClip audioClip)
    {
       FXSource.clip = audioClip;
        FXSource.Play();
    }

    private void OnDisable()
    {
        FXEvent.OnEventRaised -= OnFXEvent;
        BGMEvent.OnEventRaised -= OnBGMEvent;
        volumeEvent.OnEventRaised -= OnVolumeEvent;
        pauseEventSO.OnEventRaised -= OnPauseEvent;
    }

    private void OnPauseEvent()
    {
        float amount;
        mixer.GetFloat("MasterVolume", out amount);
        syncVolumEventSO.RaiseEvent(amount);
    }

    private void OnVolumeEvent(float amount)
    {
        mixer.SetFloat("MasterVolume", amount*100-80);
    }
}
