using UnityEngine;
using System.Collections.Generic;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("SFX 클립 등록")]
    public List<NamedAudioClip> sfxClips;

    private Dictionary<string, AudioClip> sfxClipDict;

    [Header("볼륨 조절")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float bgmVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        InitializeClipDictionary();
        ApplyVolume();
    }

    private void InitializeClipDictionary()
    {
        sfxClipDict = new Dictionary<string, AudioClip>();
        foreach (var clip in sfxClips)
        {
            if (clip.clip != null && !sfxClipDict.ContainsKey(clip.name))
                sfxClipDict[clip.name] = clip.clip;
        }
    }

    private void ApplyVolume()
    {
        bgmSource.volume = bgmVolume * masterVolume;
        sfxSource.volume = sfxVolume * masterVolume;
    }

    // === BGM ===
    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (clip == null) return;
        bgmSource.clip = clip;
        bgmSource.loop = loop;
        ApplyVolume();
        bgmSource.Play();
    }

    public void StopBGM() => bgmSource.Stop();

    // === SFX ===
    public void PlaySFX(string name, float volume = 1f)
    {
        if (sfxClipDict.TryGetValue(name, out var clip))
        {
            PlaySFX(clip, volume);
        }
        else
        {
            Debug.LogWarning($"[AudioManager] '{name}' 효과음 없음");
        }
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, volume * sfxVolume * masterVolume);
        }
    }

    // === 볼륨 조절 ===
    public void SetMasterVolume(float value)
    {
        masterVolume = Mathf.Clamp01(value);
        ApplyVolume();
    }

    public void SetBGMVolume(float value)
    {
        bgmVolume = Mathf.Clamp01(value);
        ApplyVolume();
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
        ApplyVolume();
    }
}

[System.Serializable]
public class NamedAudioClip
{
    public string name;
    public AudioClip clip;
}
