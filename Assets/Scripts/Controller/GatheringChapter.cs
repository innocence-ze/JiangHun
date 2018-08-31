using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GatheringChapter : MonoBehaviour {

    [SerializeField]
    private GameObject bg;
    [SerializeField]
    private TextMeshProUGUI text;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeChapter(int i)
    {
        //TODO
        switch(i)
        {
            case 1: text.text = "第一章";break;
            case 2: text.text = "第二章";break;
            case 3: text.text = "第三章";break;
        }
        TextMeshProUGUI[] children = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        foreach(TextMeshProUGUI t in children)
        {
            t.color = Color.black;
        }
    }
}
