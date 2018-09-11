using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

    [Header("音乐音量"), Range(0, 1.0f)]
    public float musicVolumn = 1.0f;

    /// <summary>
    /// 音乐淡出时间（秒）
    /// </summary>
    [Header("音乐淡出时间"), Range(0.1f, 2.0f)]
    public float musicFadeOutTime = 1.0f;

    private  AudioSource audioSource = null;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void Play(AudioClip audio)
    {
        audioSource.clip = audio;
        audioSource.volume = musicVolumn;
        audioSource.loop = true;
        audioSource.Play();
    }


    /// <summary>
    /// 播放bgm
    /// </summary>
    /// <param name="music"></param>
    public void PlayMusic(AudioClip music)
    {
        StartCoroutine(playMusic(music));
    }
    private IEnumerator playMusic(AudioClip music)
    {
        if(audioSource.isPlaying == true )
        {
            //已有bgm则淡出
            float volumn = musicVolumn;
            while (volumn > 0)
            {
                audioSource.volume = volumn;
                volumn -= musicVolumn * Time.deltaTime / musicVolumn;
                yield return 0;
            }
        }

        Play(music);

        yield return 0;
    }

}
