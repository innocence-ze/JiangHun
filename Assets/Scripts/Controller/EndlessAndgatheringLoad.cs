using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessAndgatheringLoad : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Endless()
    {
        SceneLoadManager.LoadScene(4);
    }
    public void Gathering()
    {
        SceneLoadManager.LoadScene(6);
    }
}
