using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    int step;
    [SerializeField]
    private ArrayList[] addLine;

	// Use this for initialization
	void Awake () {
        Map.Instance.InitMap();
        step = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void NextStep()
    {
       
    }
}
