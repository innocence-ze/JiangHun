using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToturialManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] panel;
    [SerializeField]
    private bool[] flag=new bool[3] {false,false,false};
    private Click click;

	// Use this for initialization
	void Start () {
        click = gameObject.GetComponent<Click>();

        panel[0].GetComponent<ChoosePanel>().Stop();
        flag[0] = true;
        foreach (GameObject g in panel[0].GetComponent<ChoosePanel>().objects)
            g.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
        if(click.ClickSteps==1&&flag[1]==false)
        {
            panel[1].GetComponent<ChoosePanel>().Stop();
            foreach (GameObject g in panel[1].GetComponent<ChoosePanel>().objects)
                g.SetActive(false);
            flag[1] = true;
        }

	}

    public void NextStepShow()
    {
        if (gameObject.GetComponent<R_GameManager>().BDefeat == true)
            return;
        if (flag[2] == false)
        {
            panel[2].GetComponent<ChoosePanel>().Stop();
            foreach (GameObject g in panel[2].GetComponent<ChoosePanel>().objects)
                g.SetActive(false);
            flag[2] = true;
        }
    }
}
