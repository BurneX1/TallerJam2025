using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

/// Script encargado de almacenar y controlar cada uno de los sonidos del juego, y poder modificar los valores de estos.
/// 
/// El SoundManager permite llevar un mejor control de todos los sonidos del juego, 
/// para utilizarlo deberá de agregar los clips de sonidos en la pestaña de Inspector de 
/// Unity y posteriormente llamar a la función que ejecuta el sonido mediante código.
public class AudioManager : MonoBehaviour
{
    [Range(0f, 1f)]
    public float globalVolume;
    [Range(0f, 1f)]
    public float mscVolume;
    [Range(0f, 1f)]
    public float sndVolume;
    public Sound[] soundsArray;
    public static AudioManager instance;
    public AudioMixerGroup masterMixer;
    public AudioMixerGroup bgmMixer;
    public AudioMixerGroup sndfxMixer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        //Global Volume Values
        if (PlayerPrefs.HasKey("GenVol") == false)
        {
            PlayerPrefs.SetFloat("GenVol", 0.5f);
        }
        if (PlayerPrefs.HasKey("MscVol") == false)
        {
            PlayerPrefs.SetFloat("MscVol", 0.5f);
        }
        if (PlayerPrefs.HasKey("SndVol") == false)
        {
            PlayerPrefs.SetFloat("SndVol", 0.5f);
        }

        globalVolume = PlayerPrefs.GetFloat("GenVol");
        //bgmMixer.audioMixer.SetFloat()


        //Specific Sound Sett
        foreach (Sound s in soundsArray)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;// * globalVolume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.effect3D;
            if (s.bgMusic == true)
            {
                s.source.outputAudioMixerGroup = bgmMixer;
            }
            else
            {
                s.source.outputAudioMixerGroup = sndfxMixer;
            }

        }

    }
    // Start is called before the first frame update
    void Start()
    {

        Play("Theme");
    }

    // Update is called once per frame
    void Update()
    {
        globalVolume = PlayerPrefs.GetFloat("GenVol");
        mscVolume = PlayerPrefs.GetFloat("MscVol");
        sndVolume = PlayerPrefs.GetFloat("SndVol");
        RefreshVolumes();
    }

    //________________________ Mixer & Volume Methods________________________//

    public void RefreshVolumes()
    {
        //Master Update
        /*if (globalVolume != 0)
        {
            masterMixer.audioMixer.SetFloat("MasterVol", Mathf.Log10(globalVolume) * 20);
        }
        else
        {
            masterMixer.audioMixer.SetFloat("MasterVol", Mathf.Log10(0.001f) * 20);
        }

        //BGM Update
        if (mscVolume != 0)
        {
            masterMixer.audioMixer.SetFloat("BgmVol", Mathf.Log10(mscVolume) * 20);
        }
        else
        {
            masterMixer.audioMixer.SetFloat("BgmVol", Mathf.Log10(0.001f) * 20);
        }

        //Snd Update
        if (sndVolume != 0)
        {
            masterMixer.audioMixer.SetFloat("SndVol", Mathf.Log10(sndVolume) * 20);
        }
        else
        {
            masterMixer.audioMixer.SetFloat("SndVol", Mathf.Log10(0.001f) * 20);
        }*/

    }

    public void SetVolume(AudioMixer mixer, float volumeVal)
    {
        mixer.SetFloat("BgmVol", Mathf.Log10(volumeVal) * 20);
    }

    //_______________________________________________________________________//

    //-----------------------------Audio Methods------------------------------//
    public void Play(string name)
    {
        Sound s = Array.Find(soundsArray, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("El sonido " + name + " no se ha encontrado");
            return;

        }
        s.source.Play();
    }
    public void Pause(string name)
    {
        Sound s = Array.Find(soundsArray, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("El sonido " + name + " no se encontró");
            return;
        }
        s.source.Pause();
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(soundsArray, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("El sonido " + name + " no se encontró");
            return;
        }
        s.source.Stop();
    }
    public void Resume(string name)
    {
        Sound s = Array.Find(soundsArray, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("El sonido " + name + " no se encontró");
            return;
        }
        s.source.UnPause();
    }

    public bool DetectPlaying(string name)
    {
        Sound s = Array.Find(soundsArray, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("El sonido " + name + " no se encontró");
            return false;
        }
        return s.source.isPlaying;
    }
    //------------------------------------------------------------------------//
}