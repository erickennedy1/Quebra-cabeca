using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource; 

    private static AudioManager instance; 

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    public void SetAudioState(bool isOn)
    {
        if (audioSource != null)
        {
            audioSource.enabled = isOn; 
        }
    }
}
