using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMove : MonoBehaviour
{

    [SerializeField]
    private Transform modelChoose;
    [SerializeField]
    private Transform levelChoose;
    [SerializeField]
    private Transform gathering;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToModelChoose()
    {
        //gameObject.transform.DOMove(modelChoose.position, 1f);
    }

    public void ToLevelChoose()
    {
        //gameObject.transform.DOMove(levelChoose.position, 1f);
    }

    public void ToGathering()
    {
        //gameObject.transform.DOMove(levelChoose.position, 1f);
    }

    public void De()
    {
        Debug.Log("1");
    }
}
