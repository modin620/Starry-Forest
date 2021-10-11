using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource[] allAudioChannel;

    public void PauseAllhannels()
    {
        for (int i = 0; i < allAudioChannel.Length; i++)
            allAudioChannel[i].Pause();
    }

    public void PlayAllhannels()
    {
        for (int i = 0; i < allAudioChannel.Length; i++)
            allAudioChannel[i].Play();
    }
}
