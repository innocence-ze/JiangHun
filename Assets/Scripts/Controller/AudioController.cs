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



    public  AudioSource musicSource = null;

    //方法
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="audio">将要播放的音效</param>
    public void Play(AudioClip audio)
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        StartCoroutine(play(audio, musicSource));
    }
    public void Play(AudioClip audio, AudioSource source)
    {
        StartCoroutine(play(audio, source));
    }
    private IEnumerator play(AudioClip audio, AudioSource source)
    {
        //AudioSource _source =
        //source.gameObject.AddComponent<AudioSource>();
        source.clip = audio;
        source.volume = musicVolumn;
        source.Play();

        yield return new WaitForSeconds(audio.length);
        Destroy(source);

        yield return 0;
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
        if(musicSource != null)
        {
            //已有bgm则淡出
            float volumn = musicVolumn;
            while (volumn > 0)
            {
                musicSource.volume = volumn;
                volumn -= musicVolumn * Time.deltaTime / musicVolumn;
                yield return 0;
            }
            Destroy(musicSource);
        }

        if(music == null)
        {
            musicSource = null;
        }
        else
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.clip = music;
            musicSource.volume = musicVolumn;
            musicSource.loop = true;
            musicSource.Play();
        }
        yield return 0;
    }

}
