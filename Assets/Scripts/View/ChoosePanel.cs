using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// 面板按键以及移动效果
/// </summary>
public class ChoosePanel : MonoBehaviour
{

    [SerializeField]
    private RectTransform target;
    [SerializeField]
    private RectTransform source;
    [SerializeField]
    private GameObject stopButton;
    [SerializeField]
    private GameObject nextButton;

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
        LevelManager.Instance.prefab.GetComponentInChildren<Click>().enabled = true;
        MoveToSource();
        stopButton.SetActive(true);
        nextButton.SetActive(true);
        LevelManager.Instance.ReStart();       
    }

    public void Continue()
    {
        LevelManager.Instance.prefab.GetComponentInChildren<Click>().enabled = true;
        MoveToSource();
        stopButton.SetActive(true);
        nextButton.SetActive(true);
    }

    public void ReturnToMenu()
    {
        SceneLoadManager.LoadScene(0);
    }

    public void Stop()
    {
        LevelManager.Instance.prefab.GetComponentInChildren<Click>().enabled = false;
        stopButton.SetActive(false);
        nextButton.SetActive(false);
        MoveToTarget();
    }
}
