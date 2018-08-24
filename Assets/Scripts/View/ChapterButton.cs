using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 选关界面的按钮
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
        SceneLoadManager.LoadScene(gameObject.name.ToCharArray()[0]-48);        
    }
}
