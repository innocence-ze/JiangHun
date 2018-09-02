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
    private RectTransform BG;

    [SerializeField]
    private GameObject shade;

    //用于镜头缩放
    //private float scale;
    //public float aimScale;
    [SerializeField]
    private GameObject sand;
    [SerializeField]
    private float animTime;

    void Awake()
    {
        //DontDestroyOnLoad(canvasOfSand);

        ChapterButton[] buttons = modelChoose.GetComponentsInChildren<ChapterButton>();
        foreach (ChapterButton c in buttons)
            c.sand = sand;
        buttons = levelChoose.GetComponentsInChildren<ChapterButton>();
        foreach (ChapterButton c in buttons)
            c.sand = sand;

        //scale = BG.GetComponentInParent<CanvasScaler>().scaleFactor;

        switch (SceneLoadManager.aimChoose)
        {
            case 1:BG.localPosition = -begin.localPosition; return;
            case 2:BG.localPosition = -modelChoose.localPosition; return;
            case 3:BG.localPosition = -levelChoose.localPosition; return;
        }
        SceneLoadManager.aimChoose = 1;
    }

    private void Update()
    {
        //BG.GetComponentInParent<CanvasScaler>().scaleFactor = scale;
    }

    public void MoveCamera(RectTransform rect)
    {
        shade.SetActive(true);
        sand.GetComponent<Animator>().Play(Animator.StringToHash("Sand"),0,0);
        StartCoroutine(delayMove(sand.GetComponent<Animation>().clip.length / 2, rect));
        StartCoroutine(ShadeActive(sand.GetComponent<Animation>().clip.length, false));
    }

    public void MoveToModelChoose()
    {
        MoveCamera(modelChoose);
    }

    public void MoveToLevelChoose()
    {
        MoveCamera(levelChoose);
    }

    public void MoveToBegin()
    {
        MoveCamera(begin);
    }

    //移动视角并且放大
    //public void FocusOn(RectTransform trans)
    //{
    //    shade.SetActive(true);
    //    StartCoroutine(focus());
    //    BG.DOLocalMove(BG.position - trans.position, 1f);
    //    StartCoroutine(ShadeActive(2f, false));
    //}

    //public void DisFocus()
    //{
    //    shade.SetActive(true);
    //    DOTween.To(() => scale, x => scale = x, 1, 1);
    //    StartCoroutine(disfocus());
    //    StartCoroutine(ShadeActive(2f, false));
    //}

    //IEnumerator focus()
    //{
    //    yield return new WaitForSeconds(1f);
    //    DOTween.To(() => scale, x => scale = x, aimScale, 1);
    //}

    //IEnumerator disfocus()
    //{
    //    yield return new WaitForSeconds(1f);
    //    MoveToLevelChoose();
    //}

    IEnumerator ShadeActive(float time,bool active)
    {
        yield return new WaitForSeconds(time);
        shade.SetActive(active);
    }

    IEnumerator delayMove(float time, RectTransform trans)
    {
        yield return new WaitForSeconds(time);
        BG.localPosition = -trans.localPosition;
    }
}
