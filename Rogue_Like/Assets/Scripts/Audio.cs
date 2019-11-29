using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{

    public AudioSource fxAudio;
    public AudioSource audioSourceMusic;

    public void PlaySingle(AudioClip clip)
    {
        fxAudio.clip = clip;
        fxAudio.Play();
    }

    public void PlayRandomClip(AudioClip[] clips)
    {
        int index = Random.Range(0, clips.Length);
        AudioClip clip = clips[index];
        PlaySingle(clip);

    }

    public void StopMusic()
    {
        audioSourceMusic.Stop();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
