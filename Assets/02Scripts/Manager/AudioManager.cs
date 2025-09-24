using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AudioManager : Singleton<AudioManager>
{
    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        PoolObject obj = PoolManager.Instance.SpawnFromPool("SoundObject", spawnTransform.position, Quaternion.identity);
        AudioSource audioSource = obj.GetComponent<AudioSource>();

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        StartCoroutine(ReturnWhenFinished(audioSource, obj));
    }

    private IEnumerator ReturnWhenFinished(AudioSource source, PoolObject obj)
    {
        yield return new WaitForSeconds(source.clip.length);
        PoolManager.Instance.ReturnToPool(obj);
    }
}