using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : Singleton<SoundManager> {
    public GameObject soundPrefab;
    public int maxPool = 10;

    [Range(0f, 1f)]
    public float defaultBGMVolume = 1f;
    [Range(0f, 1f)]
    public float defaultSFXVolume = 1f;
    [Range(0f, 1f)]
    public float defaultAmbientVolume = 1f;

    public AudioClip[] bgm;
    public AudioClip[] ambience;
    public AudioClip[] sfx;

    Dictionary<string, AudioClip> soundDictionary;
    Queue<SoundObject> soundPool;

    Dictionary<string, SoundObject> activeBGM;
    Dictionary<string, SoundObject> activeAmbience;
    List<SoundObject> activeSFX;

    Transform pool_Parent;
    Transform BGM_Parent;
    Transform ambience_Parent;
    Transform SFX_Parent;

    private void Awake()
    {
        soundDictionary = new Dictionary<string, AudioClip>();
        activeBGM = new Dictionary<string, SoundObject>();
        activeAmbience = new Dictionary<string, SoundObject>();
        activeSFX = new List<SoundObject>();

        pool_Parent = new GameObject().transform;
        pool_Parent.name = "SoundPool";
        pool_Parent.SetParent(this.transform);

        BGM_Parent = new GameObject().transform;
        BGM_Parent.name = "BGM (0)";
        BGM_Parent.SetParent(this.transform);

        ambience_Parent = new GameObject().transform;
        ambience_Parent.name = "Ambience (0)";
        ambience_Parent.SetParent(this.transform);

        SFX_Parent = new GameObject().transform;
        SFX_Parent.name = "SFX (0)";
        SFX_Parent.SetParent(this.transform);

        InitPool();

        for (int i = 0; i < bgm.Length; ++i)
        {
           AddSound(bgm[i].name, bgm[i]);
        }

        for (int i = 0; i < ambience.Length; ++i)
        {
            AddSound(ambience[i].name, ambience[i]);
        }

        for (int i = 0; i < sfx.Length; ++i)
        {
            AddSound(sfx[i].name, sfx[i]);
        }
    }

    private void Update()
    {
        foreach (SoundObject sound in activeSFX.ToList<SoundObject>())
        {
            if(sound.isDone())
            {
                activeSFX.Remove(sound);
                ReturnToPool(sound);
                SFX_Parent.name = "SFX (" + activeSFX.Count.ToString() + ")";
            }
        }
    }

    public void AddSound(string audioName, AudioClip sound)
    {
        if(soundDictionary.ContainsKey(audioName))
        {
            Debug.Log("Sound Directory already contains " + audioName);
            return;
        }

        soundDictionary.Add(audioName, sound);
    }

    public void AddSounds(Dictionary<string, AudioClip> addedSounds)
    {
        for (int i = 0; i < addedSounds.Count; ++i)
        {
            KeyValuePair<string, AudioClip> pair = addedSounds.ElementAt(i);
            if (soundDictionary.ContainsKey(pair.Key))
            {
                Debug.Log("Sound Directory already contains " + pair.Key);
                continue;
            }
            soundDictionary.Add(pair.Key, pair.Value);
        }
    }

    public void RemoveSound(string audioName)
    {
        if(!soundDictionary.Remove(audioName))
            Debug.Log("Unable to remove sound, " + audioName + " does not exist.");
    }

    public void PlayBGM(string audioName)
    {
        AudioClip clip = null;
        if(activeBGM.ContainsKey(audioName))
        {
            Debug.LogWarning("BGM sound is already playing.");
            return;
        }

        if (soundDictionary.TryGetValue(audioName, out clip))
        {
            SoundObject bgm = GrabFromPool();
            if (bgm == null)
                return;

            bgm.transform.SetParent(BGM_Parent);
            bgm.gameObject.name = audioName;
            bgm.SetAudioClip(clip);
            bgm.SetVolume(defaultBGMVolume);
            bgm.SetLoop(true);
            bgm.Play();

            activeBGM.Add(audioName, bgm);
            BGM_Parent.name = "BGM (" + activeBGM.Count.ToString() + ")";
        }
        else
            Debug.Log("Unable to play BGM sound, " + audioName + " does not exist.");
    }

    public void PlayAmbience(string audioName)
    {
        AudioClip clip = null;
        if (activeAmbience.ContainsKey(audioName))
        {
            Debug.LogWarning("Ambience sound is already playing.");
            return;
        }

        if (soundDictionary.TryGetValue(audioName, out clip))
        {
            SoundObject ambient = GrabFromPool();
            if (bgm == null)
                return;

            ambient.transform.SetParent(ambience_Parent);
            ambient.gameObject.name = audioName;
            ambient.SetAudioClip(clip);
            ambient.SetVolume(defaultAmbientVolume);
            ambient.SetLoop(true);
            ambient.Play();

            activeAmbience.Add(audioName, ambient);
            ambience_Parent.name = "Ambience (" + activeAmbience.Count.ToString() + ")";
        }
        else
            Debug.Log("Unable to play Ambient sound, " + audioName + " does not exist.");
    }

    public void PlaySFX(string audioName)
    {
        if (pool_Parent.transform.childCount <= 0)
        {
            Debug.LogWarning("Sound pool is empty, this sound will not be played.");
            return;
        }

        AudioClip clip = null;
        if (soundDictionary.TryGetValue(audioName, out clip))
        {
            SoundObject sfx = GrabFromPool();
            if (sfx == null)
                return;


            sfx.transform.SetParent(SFX_Parent);
            sfx.gameObject.name = audioName;
            sfx.SetAudioClip(clip);
            sfx.SetVolume(defaultSFXVolume);
            sfx.SetLoop(false);
            sfx.Play();
            activeSFX.Add(sfx);
            SFX_Parent.name = "SFX (" + activeSFX.Count.ToString() + ")";
        }
        else
            Debug.LogWarning("Unable to play SFX sound, " + audioName + " does not exist.");
    }

    public void PauseAllSounds()
    {
        PauseAllBGM();
        PauseAllAmbient();
    }

    public void PauseAllBGM()
    {
        foreach(KeyValuePair<string, SoundObject> pair in activeBGM)
        {
            pair.Value.Pause();
        }
    }

    public void PauseAllAmbient()
    {
        foreach (KeyValuePair<string, SoundObject> pair in activeAmbience)
        {
            pair.Value.Pause();
        }
    }

    public void PauseBGM(string audioName)
    {
        SoundObject sound = null;
        if (activeBGM.TryGetValue(audioName, out sound))
        {
            sound.Pause();
        }
        else
            Debug.Log("Unable to pause BGM sound, " + audioName + " does not exist.");
    }

    public void PauseAmbient(string audioName)
    {
        SoundObject sound = null;
        if (activeAmbience.TryGetValue(audioName, out sound))
        {
            sound.Pause();
        }
        else
            Debug.Log("Unable to pause Ambient sound, " + audioName + " does not exist.");
    }

    public void StopAllSounds()
    {
        StopAllBGM();
        StopAllAmbience();
    }

    public void StopAllBGM()
    {
        foreach(KeyValuePair<string, SoundObject> pair in activeBGM)
        {
            ReturnToPool(pair.Value);
        }

        activeBGM.Clear();
        BGM_Parent.name = "BGM (" + activeBGM.Count.ToString() + ")";
    }

    public void StopAllAmbience()
    {
        foreach(KeyValuePair<string, SoundObject> pair in activeAmbience)
        {
            ReturnToPool(pair.Value);
        }
        activeAmbience.Clear();
        BGM_Parent.name = "Ambience (" + activeAmbience.Count.ToString() + ")";
    }

    public void StopBGM(string audioName)
    {
        SoundObject sound = null;
        if (activeBGM.TryGetValue(audioName, out sound))
        {
            ReturnToPool(sound);
            activeBGM.Remove(name);
            BGM_Parent.name = "BGM (" + activeBGM.Count.ToString() + ")";
        }
        else
            Debug.Log("Unable to stop BGM sound, " + audioName + " does not exist.");
    }

    public void StopAmbient(string audioName)
    {
        SoundObject sound = null;
        if (activeAmbience.TryGetValue(audioName, out sound))
        {
            ReturnToPool(sound);
            activeAmbience.Remove(name);
            BGM_Parent.name = "Ambience (" + activeAmbience.Count.ToString() + ")";
        }
        else
            Debug.Log("Unable to stop Ambient sound, " + audioName + " does not exist.");
    }

    public void EmptySoundLibrary()
    {
        StopAllSounds();
        soundDictionary.Clear();
    }

    public void UpdateVolumes()
    {
        foreach(KeyValuePair<string, SoundObject> pair in activeBGM)
        {
            pair.Value.SetVolume(defaultBGMVolume);
        }

        foreach (KeyValuePair<string, SoundObject> pair in activeAmbience)
        {
            pair.Value.SetVolume(defaultAmbientVolume);
        }

        foreach (SoundObject sfx in activeSFX)
        {
            sfx.SetVolume(defaultSFXVolume);
        }
    }

    void InitPool()
    {
        soundPool = new Queue<SoundObject>();
        for (int i = 0; i < maxPool; ++i)
        {
            GameObject sfx = Instantiate(soundPrefab);
            sfx.name = "SoundObject";
            sfx.transform.SetParent(pool_Parent);
            soundPool.Enqueue(sfx.GetComponent<SoundObject>());
        }
        pool_Parent.name = "SoundPool (" + soundPool.Count.ToString() + ")";
    }

    void ReturnToPool(SoundObject sound)
    {
        sound.Stop();
        sound.gameObject.name = "SoundObject";
        sound.transform.SetParent(pool_Parent);
        sound.SetVolume(1f);
        soundPool.Enqueue(sound);
        pool_Parent.name = "SoundPool (" + soundPool.Count.ToString() + ")";
    }

    SoundObject GrabFromPool()
    {
        SoundObject sound = soundPool.Peek();
        if(sound)
        {
            soundPool.Dequeue();
            pool_Parent.name = "SoundPool (" + soundPool.Count.ToString() + ")";
            return sound;
        }
        Debug.LogWarning("Sound Pool is empty.");
        return null;
    }
}
