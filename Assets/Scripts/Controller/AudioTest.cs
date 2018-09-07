using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour {

    private AudioController controller;
	// Use this for initialization
	void Start () {
        controller = gameObject.GetComponent<AudioController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            controller.Play(Resources.Load<AudioClip>("Crack4"));
            //controller.PlayMusic(Resources.Load<AudioClip>("Level1(Loop)"));
        }
       
	}
}
