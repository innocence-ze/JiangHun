using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GatheringNum : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeChapter()
    {
        Debug.Log(gameObject.name.ToCharArray()[0] - 48);
        gameObject.GetComponentInParent<GatheringChapter>().ChangeChapter(gameObject.name.ToCharArray()[0]-48);
        gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    }
}
