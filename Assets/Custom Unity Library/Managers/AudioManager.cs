using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a manager for playing audio
/// </summary>
public class AudioManager : ManagerBehaviour<AudioManager>
{
    void Reset()
    {
        foreach (var audioSource in gameObject.GetComponents<AudioSource>())
        {
            DestroyImmediate(audioSource);
        }
    }

    void OnLevelWasLoaded()
    {
        foreach (var audioSource in gameObject.GetComponents<AudioSource>())
        {
            Destroy(audioSource);
        }
    }

    /// <summary>
    /// Plays the specified audio, and loops it if specified.
    /// If no Game Object is provided, it will play at the position of the AudioManager.
    /// </summary>
    public void PlayAudio(AudioClip clip, bool loop = false, GameObject sourceGameObject = null)
    {
        if (!sourceGameObject)
        {
            sourceGameObject = gameObject;
        }
        AudioSource newAudioSource = null;
        foreach (var audioSource in sourceGameObject.GetComponents<AudioSource>())
        {
            if (!audioSource.isPlaying)
            {
                newAudioSource = audioSource;
                break;
            }
        }
        if (!newAudioSource)
        {
            newAudioSource = sourceGameObject.AddComponent<AudioSource>();
            newAudioSource.spatialize = sourceGameObject != gameObject;
        }
        newAudioSource.clip = clip;
        newAudioSource.loop = loop;
        newAudioSource.Play();
    }

    /// <summary>
    /// Returns a list of all AudioClips currently being played.
    /// If no Game Object is provided, it will provide a list of all clips on the AudioManager
    /// </summary>
    public AudioClip[] GetPlayingClips(GameObject sourceGameObject = null)
    {
        if (!sourceGameObject)
        {
            sourceGameObject = gameObject;
        }
        var clips = new List<AudioClip>();
        foreach (var audioSource in sourceGameObject.GetComponents<AudioSource>())
        {
            if (audioSource.isPlaying)
            {
                clips.Add(audioSource.clip);
            }
        }
        return clips.ToArray();
    }

    /// <summary>
    /// Stops all AudioClips which are currently being played.
    /// If no Game Object is provided, it will stop all clips playing on the AudioManager
    /// </summary>
    public void StopAudio(GameObject sourceGameObject = null)
    {
        if (!sourceGameObject)
        {
            sourceGameObject = gameObject;
        }
        foreach (var audioSource in sourceGameObject.GetComponents<AudioSource>())
        {
            audioSource.Stop();
        }
    }

    /// <summary>
    /// Stops all instances of a specific AudioClip which are being played.
    /// If no Game Object is provided, it will stop all clips of that type on the AudioManager
    /// </summary>
    public void StopAudio(AudioClip clip, GameObject sourceGameObject = null)
    {
        if (!sourceGameObject)
        {
            sourceGameObject = gameObject;
        }
        foreach (var audioSource in sourceGameObject.GetComponents<AudioSource>())
        {
            if (audioSource.clip == clip)
            {
                audioSource.Stop();
            }
        }
    }
}