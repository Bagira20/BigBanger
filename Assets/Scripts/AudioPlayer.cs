using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public static void PlayAudioFromRange(AudioSource source, AudioClip[] audios, Vector2 pitchRange, Vector2 volumeRange, float delay)
    {
        AudioClip randomClip = audios[Random.Range(0, audios.Length - 1)];
        source.pitch += Random.Range(pitchRange.x, pitchRange.y);
        source.volume += Random.Range(volumeRange.x, volumeRange.y);
        source.PlayDelayed(delay);
    }

    public static void PlayAudioFromRange(AudioSource source, AudioClip[] audios, Vector2 pitchRange, Vector2 volumeRange) 
    {
        AudioClip randomClip = audios[Random.Range(0, audios.Length - 1)];
        source.pitch += Random.Range(pitchRange.x, pitchRange.y);
        source.volume += Random.Range(volumeRange.x, volumeRange.y);
        source.Play();
    }

    public static void PlayAudioFromRange(AudioSource source, AudioClip[] audios, float delay)
    {
        AudioClip randomClip = audios[Random.Range(0, audios.Length - 1)];
        source.PlayDelayed(delay);
    }

    public static void PlayAudioFramRange(AudioSource source, AudioClip[] audios)
    {
        AudioClip randomClip = audios[Random.Range(0, audios.Length - 1)];
        source.Play();
    }
}
