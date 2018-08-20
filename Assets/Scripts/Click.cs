using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null&&hit.collider.gameObject.GetComponent<Line>()!= null)
            {
                Line l = hit.collider.gameObject.GetComponent<Line>();
                switch (l.GetState())
                {
                    case Linestate.show: l.ChangeState(Linestate.isChoose); break;
                    case Linestate.isChoose: l.gameObject.SetActive(false); break;
                }
            }
        }
    }
}
