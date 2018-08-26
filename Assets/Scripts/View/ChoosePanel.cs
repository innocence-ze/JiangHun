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
    
    //只在无尽模式需要赋值
    [SerializeField]
    private Click click;

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

    public void EndRestart()
    {
        SceneLoadManager.LoadScene(4);
    }

    public void EndStop()
    {
        click.enabled = false;
        stopButton.SetActive(false);
        nextButton.SetActive(false);
        MoveToTarget();
    }

    public void EndContinue()
    {
        click.enabled = true;
        stopButton.SetActive(true);
        nextButton.SetActive(true);
        MoveToSource();
    }
}
