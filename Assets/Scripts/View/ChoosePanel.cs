using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// 面板按键以及移动效果
/// </summary>
public class ChoosePanel : MonoBehaviour {

    [SerializeField]
    private RectTransform target;
    [SerializeField]
    private RectTransform source;

    public void MoveToTarget()
    {
        gameObject.transform.DOLocalMove(Vector3.zero, 1f);
    }

    public void MoveToSource()
    {
        gameObject.transform.DOLocalMove(source.position - target.position, 1f);
    }

    public void Restart()
    {
        //LevelManager.Instance.prefab.SetActive(true);
        //LevelManager.Instance.ReStart();
        MoveToSource();
    }

    public void Continue()
    {
        //LevelManager.Instance.prefab.SetActive(true);
        MoveToSource();
    }

    public void ReturnToMenu()
    {
        SceneLoadManager.LoadScene(0);
    }

    public void Stop()
    {
        //LevelManager.Instance.prefab.SetActive(false);
        MoveToTarget();
    }
}
