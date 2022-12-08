using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AudioSourceType
{
    SOUND_FX,
    MUSIC
}


public class SoundManager : MonoBehaviour
{

    private List<AudioSource> audioSource;
    [SerializeField]
    private List<AudioClip> audioClips;

    private void Awake()
    {
        audioSource = GetComponents<AudioSource>().ToList();
        audioClips = new List<AudioClip>();
        InitializeSoundFX();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeSoundFX()
    {
        audioClips.Add(Resources.Load<AudioClip>("Sounds/jump"));
        audioClips.Add(Resources.Load<AudioClip>("Sounds/hit"));
        audioClips.Add(Resources.Load<AudioClip>("Sounds/death"));
        audioClips.Add(Resources.Load<AudioClip>("Sounds/main_sound"));
    }

    public void PlaySoundFX(SoundFX sound, Channel channel)
    {
        audioSource[(int)channel].clip = audioClips[(int)sound];
        audioSource[(int)channel].Play();
    }

    public void PlayMusic()
    {
        audioSource[(int)Channel.BGM].clip = audioClips[(int)SoundFX.MUSIC];
        audioSource[(int)Channel.BGM].pitch = 0.5f;
        audioSource[(int)Channel.BGM].loop = true;
        audioSource[(int)Channel.BGM].Play();
    }
}
