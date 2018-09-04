using System.Collections;
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
    private RectTransform endless;
    [SerializeField]
    private RectTransform gathering;
    [SerializeField]
    private RectTransform BG;

    [SerializeField]
    private GameObject shade;

    //用于镜头缩放
    private float scale;
    public float aimScale;
    [SerializeField]
    private GameObject sand;
    [SerializeField]
    private float animTime;
    [SerializeField]
    private GameObject canvas;

    void Awake()
    {
        ChapterButton[] buttons = modelChoose.GetComponentsInChildren<ChapterButton>();
        foreach (ChapterButton c in buttons)
        {
            c.canvas = canvas;
            c.sand = sand;
        }
        buttons = levelChoose.GetComponentsInChildren<ChapterButton>();
        foreach (ChapterButton c in buttons)
        {
            c.canvas = canvas;
            c.sand = sand;
        }

        scale = BG.GetComponentInParent<CanvasScaler>().scaleFactor;

        switch (SceneLoadManager.aimChoose)
        {
            case 1:BG.localPosition = -begin.localPosition; return;
            case 2:BG.localPosition = -modelChoose.localPosition; BG.GetComponentInParent<CanvasScaler>().scaleFactor = aimScale; return;
            case 3:BG.localPosition = -levelChoose.localPosition; BG.GetComponentInParent<CanvasScaler>().scaleFactor = aimScale; return;
        }
        SceneLoadManager.aimChoose = 1;
    }

    private void Update()
    {
        BG.GetComponentInParent<CanvasScaler>().scaleFactor = scale;
    }

    public void MoveToModelChoose(float time)
    {
        FocusOn(modelChoose,time);
    }

    public void MoveToLevelChoose()
    {
        LocalMove(levelChoose);
    }

    public void MoveToEndless()
    {
        LocalMove(endless);
    }

    public void MoveToGathering()
    {
        LocalMove(gathering);
    }

    public void LocalMove(RectTransform trans)
    {
        shade.SetActive(true);
        BG.DOLocalMove(-trans.localPosition, 1f);
        StartCoroutine(ShadeActive(1f, false));
    }

    //移动视角并且放大
    public void FocusOn(RectTransform trans,float time)
    {
        shade.SetActive(true);
        //StartCoroutine(focus());
        DOTween.To(() => scale, x => scale = x, aimScale, time);
        BG.DOLocalMove( - trans.localPosition, time);
        StartCoroutine(ShadeActive(time, false));
    }

    IEnumerator ShadeActive(float time,bool active)
    {
        yield return new WaitForSeconds(time);
        shade.SetActive(active);
    }


}
