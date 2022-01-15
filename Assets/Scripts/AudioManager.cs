using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager
{
    private static AudioManager _instance;
    private bool _paused;
    private AudioSource _audioSource;

    public static AudioManager Instance { 
        get { 
            if (_instance != null)
            {
                return _instance;
            } else
            {
                _instance = new AudioManager();
                return _instance; 
            }
        } 
    }

    private AudioManager() 
    {
        _audioSource = null;
        _paused = true;
    }

    public void Load(GameObject gameObject, string path)
    {
        AudioClip audioClip = Resources.Load<AudioClip>(path);
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.clip = audioClip;
        _paused = true;
    }

    public void TogglePlay()
    {
        if (_paused)
        {
            _audioSource.Play();
            _paused = false;
        } else
        {
            _audioSource.Pause();
            _paused = true;
        }
    }

    public void Stop()
    {
        _audioSource.Stop();
    }

    public void Destroy()
    {
        _paused = true;
        Object.Destroy(_audioSource);
    }
}
