using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour {

    public AudioController audioController;
	// Use this for initialization
	void Start () {
        audioController = GameObject.Find("Main Camera").GetComponent<AudioController>();

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            audioController.PlayMusic(Resources.Load<AudioClip>("Tittle(start button)"));
        }
        if (Input.GetMouseButtonDown(1))
        {
            audioController.PlayMusic(Resources.Load<AudioClip>("Level1(loop)"));

        }
	}
}
