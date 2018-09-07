using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] source;
    private int currentScene;

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

        source[1].GetComponent<AudioController>().PlayMusic(Resources.Load<AudioClip>("Tittle(wind)"));
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
        source[0].GetComponent<AudioController>().PlayMusic(Resources.Load<AudioClip>("Tittle(start button)"));
    }

    public void ChangeMusic()
    {
        if (currentScene != 0 && currentScene != 5 && currentScene != 6)
            source[1].GetComponent<AudioSource>().Stop();

        switch (currentScene)
        {
            case 0: source[0].GetComponent<AudioController>().PlayMusic(Resources.Load<AudioClip>("Tittle(wind)")); GameBegin(); break;
            case 1: source[0].GetComponent<AudioController>().PlayMusic(Resources.Load<AudioClip>("Level1(Loop)")); break;
            case 2: source[0].GetComponent<AudioController>().PlayMusic(Resources.Load<AudioClip>("Level2(Loop)")); break;
            case 3: source[0].GetComponent<AudioController>().PlayMusic(Resources.Load<AudioClip>("Level1(Loop)")); break;
            case 4: source[0].GetComponent<AudioController>().PlayMusic(Resources.Load<AudioClip>("Level2(Loop)")); break;
        }
    }
 
}
