using UnityEngine;

/// <summary>
///     播放音乐、音效
/// </summary>
public class AudioManager : MonoBehaviour
{
    private AudioSource _audioS;
    public static AudioManager _instance { get; private set; }

    private void Start()
    {
        _instance = this;
        _audioS = GetComponent<AudioSource>();
    }

    /// <summary>
    ///     播放指定音效
    /// </summary>
    /// <param name="clip"></param>
    public void AudioPlay(AudioClip clip)
    {
        _audioS.PlayOneShot(clip);
    }
}