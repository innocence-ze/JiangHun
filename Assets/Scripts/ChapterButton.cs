using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Load()
    {
        //SceneLoadManager.Instance.LoadScene(gameObject.name.ToCharArray()[0]-49);
        Debug.Log(gameObject.name.ToCharArray()[0] - 48);
    }
}
