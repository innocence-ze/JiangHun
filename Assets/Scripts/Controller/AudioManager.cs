using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [SerializeField]
    private AudioController mainPlayer;
    private AudioSource source;
    private int currentScene;

    private AudioClip begin;
    private AudioClip intro;
    private AudioClip loop;

    private static bool exsist = false;

	// Use this for initialization
	void Start () {
		
        if(AudioManager.exsist != false)
        {
            DestroyImmediate(gameObject);
            return;
        }
        else
        {
            AudioManager.exsist = true;
        }
        currentScene = 0;
        DontDestroyOnLoad(gameObject);

        mainPlayer = gameObject.GetComponentInChildren<AudioController>();
        source = gameObject.GetComponent<AudioSource>();

        begin = Resources.Load<AudioClip>("Music\\StartButton");
        intro = Resources.Load<AudioClip>("Music\\title(intro)");
        loop = Resources.Load<AudioClip>("Music\\title(loop)");

        source.Play();
    }
	
	// Update is called once per frame
	void Update () {
		
        if(SceneLoadManager.currentChapter!=currentScene)
        {
            currentScene = SceneLoadManager.currentChapter;
            ChangeMusic();
        }
	}

    public void GameBegin()
    {
        AudioSource.PlayClipAtPoint(begin, gameObject.transform.position);
        StartCoroutine(ButtonDelay());
    }

    public void ChangeMusic()
    {
        if (currentScene != 0 && currentScene != 5 && currentScene != 6)
            source.Stop();

        switch (currentScene)
        {
            case 0: mainPlayer.PlayMusic(intro); StartCoroutine(BeginScene()); source.Play(); break;
            case 1: mainPlayer.PlayMusic(Resources.Load<AudioClip>("Music\\Level1(Loop)")); break;
            case 2: mainPlayer.PlayMusic(Resources.Load<AudioClip>("Music\\Level2(Loop)")); break;
            case 3: mainPlayer.PlayMusic(Resources.Load<AudioClip>("Music\\Level1(Loop)")); break;
            case 4: mainPlayer.PlayMusic(Resources.Load<AudioClip>("Music\\Level2(Loop)")); break;
        }
    }
 
    IEnumerator BeginScene()
    {
        yield return new WaitForSeconds(intro.length);
        mainPlayer.Play(loop);
    }

    IEnumerator ButtonDelay()
    {
        yield return new WaitForSeconds(begin.length);
        mainPlayer.PlayMusic(intro);
        StartCoroutine(BeginScene());
    }
}
