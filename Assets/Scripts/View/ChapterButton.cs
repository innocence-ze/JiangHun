using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 所有章节切换的按钮
/// </summary>
public class ChapterButton : MonoBehaviour {

    public GameObject sand;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void Load()
    {
        sand.GetComponent<Animator>().Play(Animator.StringToHash("Sand"), 0, 0);
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(sand.GetComponent<Animation>().clip.length/2);
        SceneLoadManager.LoadScene(gameObject.name.ToCharArray()[0] - 48);
    }
}
