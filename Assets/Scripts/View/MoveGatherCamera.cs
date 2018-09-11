using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGatherCamera : MoveCamera {

    // Use this for initialization
    new void Start () {
        base.Start();
        ChangeBoundary();
	}

    // Update is called once per frame
    new void Update () {
        ChangeBoundary();
        base.Update();
	}

    void ChangeBoundary()
    {
        var BG = GameObject.FindGameObjectWithTag("BackGround").GetComponent<SpriteRenderer>().sprite;
        var width = BG.texture.width / BG.pixelsPerUnit;
        var height = BG.texture.height / BG.pixelsPerUnit;
        xMax = width / 2f;
        xMin = -xMax;
        yMax = height / 2f;
        yMin = -yMax;
        maxSize = (xMax / 16 > yMax / 9) ? yMax : 16 / 9f * xMax;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMin + 16 / 9f * m_Camera.orthographicSize, xMax - 16 / 9f * m_Camera.orthographicSize), Mathf.Clamp(transform.position.y, yMin + m_Camera.orthographicSize, yMax - m_Camera.orthographicSize), -10);
    }
}
