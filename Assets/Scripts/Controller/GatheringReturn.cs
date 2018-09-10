using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringReturn : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ReturnBack()
    {
        SceneLoadManager.aimChoose = 2;
        StartCoroutine(Shade());
    }

    IEnumerator Shade()
    {
        yield return new WaitForSeconds(1.5f);
        SceneLoadManager.LoadScene(0);
    }
}
