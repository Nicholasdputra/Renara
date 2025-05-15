using System;
using UnityEngine;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip source;
    public bool loop;

    [Range(0f, 1f)]
    public float volume;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private Sound[] audios;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string name)
    {
        Sound music = Array.Find(audios, sound => sound.name == name);
        if (music != null)
        {
            musicSource.clip = music.source;
            musicSource.volume = music.volume;
            musicSource.loop = music.loop;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound sfx = Array.Find(audios, sound => sound.name == name);
        if (sfx != null)
        {
            sfxSource.clip = sfx.source;
            sfxSource.volume = sfx.volume;
            sfxSource.loop = sfx.loop;
            sfxSource.Play();
        }
    }
}