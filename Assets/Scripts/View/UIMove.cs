﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// 初始界面UI移动效果
/// </summary>
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

    [SerializeField]
    private GameObject shade;

    //用于镜头缩放
    private float scale;
    public float aimScale;

    void Awake()
    {
        scale = BG.GetComponentInParent<CanvasScaler>().scaleFactor;
        //levelChoose.gameObject.SetActive(false);
        //gathering.gameObject.SetActive(false);
        //switch(SceneLoadManager.aimChoose)
        //{
        //    case 1:BG.position = begin.position;return;
        //    case 2:BG.position = begin.position - modelChoose.position; return;
        //    case 3:BG.position = begin.position - levelChoose.position; levelChoose.gameObject.SetActive(true); return;
        //    case 4:BG.position = begin.position - gathering.position; gathering.gameObject.SetActive(true); return;
        //}
        //SceneLoadManager.aimChoose = 1;
    }

    private void Update()
    {
        BG.GetComponentInParent<CanvasScaler>().scaleFactor = scale;
    }


    public void MoveToModelChoose()
    {
        shade.SetActive(true);
        BG.DOLocalMove(BG.position - modelChoose.position, 1f);
        StartCoroutine(ShadeActive(1f, false));
    }

    public void MoveToLevelChoose()
    {
        shade.SetActive(true);
        levelChoose.gameObject.SetActive(true);
        gathering.gameObject.SetActive(false);
        BG.DOLocalMove(BG.position - levelChoose.position, 1f);
        StartCoroutine(ShadeActive(1f, false));
    }

    public void MoveToGathering()
    {
        shade.SetActive(true);
        gathering.gameObject.SetActive(true);
        levelChoose.gameObject.SetActive(false);
        BG.DOLocalMove(BG.position - gathering.position, 1f);
        StartCoroutine(ShadeActive(1f, false));
    }

    public void MoveToBegin()
    {
        shade.SetActive(true);
        BG.DOLocalMove(BG.position - begin.position, 1f);
        StartCoroutine(ShadeActive(1f, false));
    }

    //移动视角并且放大
    public void FocusOn(RectTransform trans)
    {
        shade.SetActive(true);
        StartCoroutine(focus());
        BG.DOLocalMove(BG.position - trans.position, 1f);
        StartCoroutine(ShadeActive(2f, false));
    }

    public void DisFocus()
    {
        shade.SetActive(true);
        DOTween.To(() => scale, x => scale = x, 1, 1);
        StartCoroutine(disfocus());
        StartCoroutine(ShadeActive(2f, false));
    }

    IEnumerator focus()
    {
        yield return new WaitForSeconds(1f);
        DOTween.To(() => scale, x => scale = x, aimScale, 1);
    }

    IEnumerator disfocus()
    {
        yield return new WaitForSeconds(1f);
        MoveToLevelChoose();
    }

    IEnumerator ShadeActive(float time,bool active)
    {
        yield return new WaitForSeconds(time);
        shade.SetActive(active);
    }
}
