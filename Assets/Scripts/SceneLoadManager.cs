﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour {

    private static SceneLoadManager instance;
    public static SceneLoadManager Instance
    {
        get
        {
            if(instance==null)
            {
                instance = FindObjectOfType(typeof(SceneLoadManager)) as SceneLoadManager;
            }
            if (instance == null)
                Debug.Log("can't find SceneLoadManager");
            return instance;
        }
    }

    public static int currentChapter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadScene(int i)
    {
        i = 1;
        currentChapter = i;
        SceneManager.LoadScene(i);
    }

    public void LoadNextScene()
    {
        //currentchapter++;
        //loadscene(currentchapter);
        Debug.Log("load next scene!");
    }

    public void LoadEndLessScene()
    {
        //currentchapter
        Debug.Log("load endless scene!");
    }
}
