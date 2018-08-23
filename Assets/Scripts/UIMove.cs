using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIMove : MonoBehaviour {

    /*获取空物体，用于定位*/
    [SerializeField]
    private RectTransform begin;
    [SerializeField]
    private RectTransform modelChoose;
    [SerializeField]
    private RectTransform levelChoose;
    [SerializeField]
    private RectTransform gathering;
    [SerializeField]
    private RectTransform BG;


    public void MoveToModelChoose()
    {
        BG.DOLocalMove(BG.position - modelChoose.position, 1f);       
    }

    public void MoveToLevelChoose()
    {
        BG.DOLocalMove(BG.position - levelChoose.position, 1f);
    }

    public void MoveToGathering()
    {
        BG.DOLocalMove(BG.position - gathering.position, 1f);
    }

    public void MoveToBegin()
    {
        BG.DOLocalMove(BG.position - begin.position, 1f);
    }
}
