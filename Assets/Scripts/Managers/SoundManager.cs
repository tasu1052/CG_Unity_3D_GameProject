using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }


    public AudioSource audioSource;
    public AudioSource SFXSource;
    public List<AudioClip> clipList;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        AudioPlay("InGame");
        DontDestroyOnLoad(gameObject);
    }


    public void AudioPlay(string audioName)
    {
        audioSource.clip = clipList.Find(x => x.name == audioName);

        if (audioSource.clip == null)
        {
            Debug.LogWarning($"BGM '{audioName}' not found in clipList!");
            return;
        }

        audioSource.loop = true; // 🔁 반복 재생
        audioSource.Play();
    }

        public void AudioStop()
    {
        audioSource.Stop();
    }

    public void SFXPlay(string SFXName)
    {
            AudioClip clip = clipList.Find(x => x.name == SFXName);
    if (clip == null)
    {
        Debug.LogWarning($"SFX '{SFXName}' not found in clipList!");
        return;
    }

    float volume = 1.0f; // 기본 볼륨

    if (SFXName == "heal")
    {
        volume = 3.0f; // 🔊 heal만 크게
    }

    SFXSource.PlayOneShot(clip, volume);
        //SFXSource.PlayOneShot(clipList.Find(x => x.name == SFXName));
    }


    public void ChangeVolume(float value)
    {
        audioSource.volume = value;
        SFXSource.volume = value;
    }
}
