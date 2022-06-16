using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager
{
    private static AudioManager _instance;
    private bool _paused;
    private AudioSource[] _audioSource = new AudioSource[3];

    public enum AudioSources
    {
        SONG = 0,
        HIT = 1,
        HURT = 2
    }

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
        for (int i = 0; i < _audioSource.Length; ++i)
            _audioSource[i] = null;
        _paused = true;
    }

    public void PlayOnce(AudioSources source)
    {
        _audioSource[(int)source].PlayOneShot(_audioSource[(int)source].clip);
    }

    public void Load(GameObject gameObject, string path, AudioSources source)
    {
        if (_audioSource[(int)source] == null)
        {
            AudioClip audioClip = Resources.Load<AudioClip>(path);
            _audioSource[(int)source] = gameObject.AddComponent<AudioSource>();
            _audioSource[(int)source].clip = audioClip;

            if ((int)source == 0)
                _paused = true;
            else
                _audioSource[(int)source].volume = 0.25f;
        }
    }

    public void TogglePlay()
    {
        if (_paused)
        {
            _audioSource[0].Play();
            _paused = false;
        } else
        {
            _audioSource[0].Pause();
            _paused = true;
        }
    }

    public void Stop()
    {
        _audioSource[0].Stop();
    }

    public void Destroy()
    {
        _paused = true;
        for (int i = 0;i < _audioSource.Length; ++i)
            Object.Destroy(_audioSource[i]);
    }
}
