using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherCanvas : MonoBehaviour {

    RectTransform RT;

	// Use this for initialization
	void Start () {
        RT = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        SetBoundary();

    }

    void SetBoundary()
    {
        var BG = GameObject.FindGameObjectWithTag("BackGround").GetComponent<SpriteRenderer>().sprite;
        var width = BG.texture.width / BG.pixelsPerUnit;
        var height = BG.texture.height / BG.pixelsPerUnit;
        RT.sizeDelta = new Vector2(width, height);
    }
}
