using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public static void Play2DAudioFromRange(AudioSource source, AudioClip[] audios, Vector2 pitchRange, Vector2 volumeRange) 
    {
        source.spatialBlend = 0;
        PlayAudioFromRange(source, audios, pitchRange, volumeRange);
    }

    public static void Play2DAudioFromRange(AudioSource source, AudioClip[] audios)
    {
        source.spatialBlend = 0;
        PlayAudioFromRange(source, audios);
    }

    public static void Play3DAudioFromRange(AudioSource source, AudioClip[] audios, Vector2 pitchRange, Vector2 volumeRange) 
    {
        source.spatialBlend = 1f;
        PlayAudioFromRange(source, audios, pitchRange, volumeRange);
    }

    public static void Play3DAudioFromRange(AudioSource source, AudioClip[] audios)
    {
        source.spatialBlend = 1f;
        PlayAudioFromRange(source, audios);
    }

    static void PlayAudioFromRange(AudioSource source, AudioClip[] audios, Vector2 pitchRange, Vector2 volumeRange) 
    {
        AudioClip randomClip = audios[Random.Range(0, audios.Length - 1)];
        source.clip = randomClip;
        source.pitch = Random.Range(pitchRange.x, pitchRange.y);
        source.volume = Random.Range(volumeRange.x, Mathf.Clamp(volumeRange.y, 0, 1f));
        source.Play();
        GameObject.Find("GameManager").GetComponent<GameManager>().DebugText.text = "PLAYED";
    }

    static void PlayAudioFromRange(AudioSource source, AudioClip[] audios)
    {
        AudioClip randomClip = audios[Random.Range(0, audios.Length - 1)];
        source.clip = randomClip;
        source.Play();
    }
}
