using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundObject : MonoBehaviour {
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
    }

    public void RemoveAudioClip()
    {
        Stop();
        audioSource.clip = null;
    }

    public void Play()
    {
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void Pause()
    {
        audioSource.Pause();
    }

    public void Unpause()
    {
        audioSource.UnPause();
    }

    public bool isDone()
    {
        return !audioSource.isPlaying;
    }

    public void SetLoop(bool flag)
    {
        audioSource.loop = flag;
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
