using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadSceneOne()
    {
        StartCoroutine(StoryLoad());
    }

    IEnumerator StoryLoad()
    {
        yield return new WaitForSeconds(1.5f);
        SceneLoadManager.currentChapter = 1;
        SceneManager.LoadScene(1);
    }

}
