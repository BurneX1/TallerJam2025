using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public bool loop;
    public bool bgMusic;

    [Range(0f, 1f)]
    public float volume;

    [Range(0f, 1f)]
    public float pitch;

    [Range(0f, 1f)]
    public float effect3D;

    [HideInInspector]
    public AudioSource source;
}
