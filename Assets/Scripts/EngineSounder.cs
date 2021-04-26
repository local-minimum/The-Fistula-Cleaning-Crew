using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSounder : MonoBehaviour
{
    [SerializeField, Range(0.5f, 3)] float onPitch = 3f;
    [SerializeField, Range(0.5f, 3)] float offPitch = 0.6f;
    [SerializeField, Range(0, 1)] float onVolume = 0.8f;
    [SerializeField, Range(0, 2)] float startupTime = 1f;
    [SerializeField, Range(0, 1)] float turnoffTime = 0.3f;

    AudioSource speaker;

    bool fadingIn = false;
    bool fadingOut = false;

    public void EngineOn()
    {
        if (!fadingIn)
        {
            fadingOut = false;
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator<WaitForSeconds> FadeIn()
    {
        fadingIn = true;
        float start = Time.timeSinceLevelLoad;
        float progress = 0f;
        while (progress < 1f)
        {
            speaker.pitch = Mathf.Lerp(offPitch, onPitch, progress);
            speaker.volume = Mathf.Lerp(0f, onVolume, progress);
            if (speaker.mute) speaker.mute = false;
            yield return new WaitForSeconds(0.02f);
            progress = (Time.timeSinceLevelLoad - start) / startupTime;
            if (!fadingIn)
            {
                yield break;
            }
        }

        speaker.pitch = onPitch;
        speaker.volume = onVolume;
    }

    public void EngineOff()
    {
        if (!fadingOut)
        {
            fadingIn = false;
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator<WaitForSeconds> FadeOut()
    {
        fadingOut = true;
        float start = Time.timeSinceLevelLoad;
        float progress = 0f;
        while (progress < 1f)
        {
            speaker.pitch = Mathf.Lerp(onPitch, offPitch, progress);
            speaker.volume = Mathf.Lerp(onVolume, 0f, progress);
            yield return new WaitForSeconds(0.02f);
            progress = (Time.timeSinceLevelLoad - start) / turnoffTime;
            if (!fadingOut)
            {
                yield break;
            }
        }

        speaker.pitch = offPitch;
        speaker.volume = 0f;
    }

    private void Start()
    {
        speaker = GetComponentInChildren<AudioSource>();
        speaker.mute = true;
        speaker.volume = 0f;
    }
}
