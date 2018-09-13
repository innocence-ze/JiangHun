using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {

    GameObject canvas;
    Image[] image;
    MoveGatherCamera mgc;
    TextMeshProUGUI textmesh;
    // Use this for initialization
    void Start () {
        mgc = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MoveGatherCamera>();
        canvas = GameObject.FindGameObjectWithTag("UI");
        image = canvas.GetComponentsInChildren<Image>();
        var textmeshes = canvas.GetComponentsInChildren<TextMeshProUGUI>();
        foreach(var t in textmeshes)
        {
            if (t.raycastTarget)
            {
                textmesh = t;
                break;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        foreach(var i in image)
        {
            if(i.sprite != null)
                i.DOColor(new Color(1, 1, 1, 1), 1);
            i.raycastTarget = true;
        }
        mgc.hide = false;
        canvas.transform.Find("Right").GetComponent<GatheringPanel>().MoveLeft();
    }

    public void ChangeText(Text text)
    {
        textmesh.text = text.text;
    }
}
