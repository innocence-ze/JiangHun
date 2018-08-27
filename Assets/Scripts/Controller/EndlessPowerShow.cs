using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndlessPowerShow : MonoBehaviour {

    [SerializeField]
    private Click manager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Text>().text = manager.ClickSteps.ToString();
	}
}
