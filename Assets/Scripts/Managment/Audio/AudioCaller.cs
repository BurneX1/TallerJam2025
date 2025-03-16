using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCaller : MonoBehaviour
{
    public string enableAudioCall;
    private void OnEnable()
    {
        //if (enableAudioCall != "") AudioManager.Instance.Play(enableAudioCall,AudioManager.Instance.sounds);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (enableAudioCall != "") AudioManager.instance.Play(enableAudioCall);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallPlay(string audioName)
    {
        if (audioName != "") AudioManager.instance.Play(audioName);
    }
}
