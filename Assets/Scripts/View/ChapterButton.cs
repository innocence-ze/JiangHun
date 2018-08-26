using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 选关界面的按钮
/// </summary>
public class ChapterButton : MonoBehaviour {

    [SerializeField]
    private UIMove ui;
    [SerializeField]
    private GameObject[] childrens;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Focus()
    {              
        ui.FocusOn(gameObject.GetComponent<RectTransform>());
        StartCoroutine(ShowOrHide(true));
    }   

    public void DisFocus()
    {        
        ui.DisFocus();
        StartCoroutine(ShowOrHide(false));
    }

    public void decrease()
    {
        int num = childrens[0].GetComponentInChildren<Text>().text.ToCharArray()[0]-48;
        if (num <= 1)
            return;
        childrens[0].GetComponentInChildren<Text>().text = (num - 1).ToString();
    }

    public void increase()
    {
        int num = childrens[0].GetComponentInChildren<Text>().text.ToCharArray()[0] - 48;
        if (num >= 4)
            return;
        childrens[0].GetComponentInChildren<Text>().text = (num + 1).ToString();
    }

    IEnumerator ShowOrHide(bool active)
    {
        yield return new WaitForSeconds(2f);
        foreach(GameObject g in childrens)
        {
            g.SetActive(active);
        }
        childrens[0].GetComponentInChildren<Text>().text = 1.ToString();
    }


    public void Load()
    {
        SceneLoadManager.aimLevel = childrens[0].GetComponentInChildren<Text>().text.ToCharArray()[0] - 48;
        Debug.Log(SceneLoadManager.aimLevel);
        SceneLoadManager.LoadScene(gameObject.name.ToCharArray()[0] - 48);
    }

    public void LoadEndless()
    {
        SceneLoadManager.LoadScene(gameObject.name.ToCharArray()[0] - 48);
    }
}
