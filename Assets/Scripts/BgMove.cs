using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BgMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MoveLeft()
    {
        gameObject.transform.DOMove(new Vector3(transform.position.x-10f,transform.position.y,transform.position.z),1f);
    }
}
