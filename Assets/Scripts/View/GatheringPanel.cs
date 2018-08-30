using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// 用于藏经洞的面板控制
/// </summary>
public class GatheringPanel : MonoBehaviour {

    [SerializeField]
    private GameObject leftButton;
    [SerializeField]
    private GameObject rightButton;
    [SerializeField]
    private RectTransform leftPosition;
    [SerializeField]
    private RectTransform rightPosition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MoveLeft()
    {
        gameObject.GetComponent<RectTransform>().DOLocalMove(leftPosition.position - new Vector3(960, 540, 0), 1f);
        rightButton.SetActive(true);
        leftButton.SetActive(false);
    }

    public void MoveRight()
    {
        gameObject.GetComponent<RectTransform>().DOLocalMove(rightPosition.position - new Vector3(960, 540, 0), 1f);
        leftButton.SetActive(true);
        rightButton.SetActive(false);
    }
}
