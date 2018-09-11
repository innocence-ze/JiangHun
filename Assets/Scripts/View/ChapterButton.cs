using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 所有章节切换的按钮
/// </summary>
public class ChapterButton : MonoBehaviour {

    public GameObject canvas;
    public GameObject sand;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void Load()
    {
        sand.GetComponent<Animator>().Play(Animator.StringToHash("Shade"));
        StartCoroutine(LoadScene());    
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(sand.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);     
        SceneLoadManager.LoadScene(gameObject.name.ToCharArray()[0] - 48);  
    }

    IEnumerator LoadAsync()
    {
        AsyncOperation async;
        if (gameObject.name.ToCharArray()[0] - 48 == 1)
        {
            async = SceneManager.LoadSceneAsync(5);
            SceneLoadManager.currentChapter = 5;
        }
        else
        {
            async = SceneManager.LoadSceneAsync(gameObject.name.ToCharArray()[0] - 48);
            SceneLoadManager.currentChapter = gameObject.name.ToCharArray()[0] - 48;
        }
        yield return async;
        // return new WaitForSeconds(canvas.GetComponent<DestroyCanvas>().sand.GetComponent<Animation>().clip.length / 2);
    }
}
