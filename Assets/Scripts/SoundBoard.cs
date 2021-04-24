using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    TargetGunk, CleaningGunk, ExplodingGunk, BouncingGround
}

public class SoundBoard : MonoBehaviour
{
    [SerializeField] AudioClip targetingGunk;
    [SerializeField] AudioClip cleaningGunk;
    [SerializeField] AudioClip explodingGunk;
    [SerializeField] AudioClip bouncingGround;
    public static void Play(SoundType sound)
    {
        var speakers = instance.speakers;
        switch (sound)
        {
            case SoundType.BouncingGround:
                speakers.PlayOneShot(instance.bouncingGround);
                break;
            case SoundType.ExplodingGunk:
                speakers.PlayOneShot(instance.explodingGunk);
                break;
            case SoundType.CleaningGunk:
                speakers.PlayOneShot(instance.cleaningGunk);
                break;
            case SoundType.TargetGunk:
                speakers.PlayOneShot(instance.targetingGunk);
                break;
        }
    }    

    static SoundBoard instance
    {
        get;
        set;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(this);
        }        
    }

    AudioSource speakers;

    private void Start()
    {
        speakers = GetComponentInChildren<AudioSource>();
    }

    private void OnDestroy()
    {
        if (instance = this)
        {
            instance = null;
        }
    }
}
