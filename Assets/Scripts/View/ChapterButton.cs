using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 所有章节切换的按钮
/// </summary>
public class ChapterButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void Load()
    {
        SceneLoadManager.LoadScene(gameObject.name.ToCharArray()[0] - 48);
    }
}
