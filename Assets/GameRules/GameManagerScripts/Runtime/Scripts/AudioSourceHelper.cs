using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    

public class AudioSourceHelper : MonoBehaviour
{
    public static AudioSourceHelper insatnce;
    
    [SerializeField]AudioSource[] sources = new AudioSource[0];
    [HideInInspector]
    public ReferenceAudio[] refernceAudioArray;

   
    #region Awake Methods
   
    void Awake()
    {
        CreateAudioScources();
        if (insatnce == null)
        {
            insatnce = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
  
    /// <summary>
    /// Generates an audio source for each type of audio
    /// </summary>
    public void CreateAudioScources()
    {
        if(sources.Length == 0)
        {
            sources = new AudioSource[5];
        }
        
        string[] names = new string[] { "Dialouge AudioSource", "Music AudioSource", "SoundEffect AudioSource", "Foley AudioSource", "Background AudioSource" };
        for(int i = 0; i < sources.Length; i++)
        {
            if (sources[i] == null)
            {
                 Type audioSource = typeof(AudioSource);
                 GameObject go = new GameObject(names[i], audioSource);
                 go.transform.parent = transform;
            }
            
            sources[i] = GetComponentsInChildren<AudioSource>()[i];
        }
        
    }
    #endregion

    #region Play Methods
    /// <summary>
    /// Play that takes in a string value
    /// </summary>
    /// <param name="audio"></param>
    /// <param name="tr"></param>
    public void PlaySound(string audio, Transform tr)
    {
       
        var audioClip = GetAudioClip(audio); 
        AudioSource audioSource = GetAudioScource(audioClip.audioClipType);
        audioSource.transform.position = tr.position;
        if(!audioClip.Equals(default(ReferenceAudio.AudioClips)))
        {
            audioSource.PlayOneShot(audioClip.clip);
        }
    }
    public void PlaySoundLooping(string audio, Transform tr)
    {
        StopMusic();
        var audioClip = GetAudioClip(audio);
        AudioSource audioSource = GetAudioScource(audioClip.audioClipType);
        audioSource.loop = true;
        audioSource.transform.position = tr.position;
        if (!audioClip.Equals(default(ReferenceAudio.AudioClips)))
        {
            audioSource.clip = (audioClip.clip);
            audioSource.Play();
        }
    }
    public void PlayRandom(string[] audio, Transform tr)
    {
        PlaySound(audio[UnityEngine.Random.Range(0, 100)%(audio.Length-1)], tr);
    }
    public void StopMusic()
    {
        sources[1].Stop();
    }
    #endregion

    #region Get Audio Clips Methods
    /// <summary>
    /// Gets the clip value within the refernece audio array
    /// </summary>
    /// <param name="audioLabel"></param>
    /// <returns></returns>
    public ReferenceAudio.AudioClips GetAudioClip(string audioLabel)
    {
        for(int i = 0; i < refernceAudioArray.Length; i++)
        {
            foreach(var clipValues in refernceAudioArray[i].clips)
            {
                if(clipValues.name == audioLabel)
                {
                    return clipValues;
                }
            }
        }
        return new ReferenceAudio.AudioClips();
    }
    #endregion

    #region Get Audio Source Methods
    public AudioSource GetAudioScource(AudioClipType type)
    {
        return sources[((int)type)];
    }
    #endregion
    public enum AudioClipType
    {
        DX = 0,
        MX = 1, 
        SFX = 2,
        FOL = 3,
        BG = 4
    }
}
