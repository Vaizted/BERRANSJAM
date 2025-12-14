using UnityEngine;

public class AudioSystem : Singleton<AudioSystem>
{
    [SerializeField] private AudioSource m_MusicSource;
    [SerializeField] private AudioSource m_SoundSource;

    public void PlaySound(AudioClip clip, Vector3 pos, float vol = 1)
    {
        m_SoundSource.transform.position = pos;
        PlaySound(clip, vol);
    }
    public void PlaySound(AudioClip clip, float vol = 1)
    {
        m_SoundSource.PlayOneShot(clip, vol);
    }
}
