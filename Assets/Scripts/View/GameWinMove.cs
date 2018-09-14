using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 游戏胜利时移动相机
/// 1.游戏背景图片命名为BackGroundOld，以及BackGround
/// 2.BackGroundOld上挂载脚本SetImageAlpha
/// 3.将此脚本挂载到相机上
/// 4.根据游戏流程需要调用MoveWin()函数
/// </summary>
public class GameWinMove : MonoBehaviour {

    public bool isWin = false;

    private Vector3 target;
    private Vector3 myTarget;
    [SerializeField]
    [Header("移动间隔")]
    private float moveDis = 10;
    //用于控制背景透明度
    private float i = 0;

    [SerializeField]
    private GameObject BG;
    [SerializeField]
    private GameObject wall;
    [SerializeField]
    private GameObject levelManager;
    [SerializeField]
    private GameObject oldBG;
    [SerializeField]
    private GameObject newBG;

    // Use this for initialization
    void Start () {
        myTarget = transform.position;
    }

    private void LateUpdate()
    {
        transform.position = myTarget;
        GameObject.Find("BackGroundOld").GetComponent<SetImageAlpha>().leftX = i;
    }
    /// <summary>
    /// 游戏胜利的时候移动相机
    /// </summary>
    public void MoveWin()
    {
        if(levelManager.GetComponent<LevelManager>().prefab!=null)
            DestroyImmediate(levelManager.GetComponent<LevelManager>().prefab);
        BG = GameObject.Find("BG");
        oldBG.transform.position = BG.transform.position;
        newBG.transform.position = BG.transform.position;
        DestroyImmediate(BG);
        DestroyImmediate(wall);
        StartCoroutine(moveWinWait());
    }
    IEnumerator moveWinWait()
    {
        yield return new WaitForSeconds(0.8f);

        Vector3 nowPos = new Vector3(20,0,-10f);
        Vector3 startPos = new Vector3(-30, 0, -10f);
        Sequence s = DOTween.Sequence();
        Sequence s2 = DOTween.Sequence();
        //快速回到初始点，坐标或移动时间有需求自己改
        s.Append(DOTween.To(() => myTarget, x => myTarget = x, startPos, 1f)).SetEase(Ease.OutQuad);
        //缓慢移动到终点，坐标或移动时间有需求自己改
        s.Insert(1f, DOTween.To(() => myTarget, x => myTarget = x, nowPos, 14.0f)).SetEase(Ease.Linear);

        //s2.Insert(0.5f,DOTween.To(() => i, x => i =x, 1, 15.0f)).SetEase(Ease.Linear);
        s2.Insert(1f, DOTween.To(() => i, x => i = x, 1, 14.0f));
        //s.Insert(0.5f, DOTween.To(() => myTarget, x => myTarget = x, nowPos, 10.0f));
    }
}
