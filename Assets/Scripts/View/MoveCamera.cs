﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 关卡内缩放移动（相机）
/// </summary>
public class MoveCamera : MonoBehaviour {

    [SerializeField]
    private float maxSize = 4f;
    [SerializeField]
    private float minSize = 1f;

    //记录上一次手机触摸位置判断用户是在放大还是缩小
    private Vector2 oldPosition1;
    private Vector2 oldPosition2;


    private Vector2 lastSingleTouchPosition;

    private Vector3 m_CameraOffset;
    private Camera m_Camera;

    [SerializeField]
    float sizeFactor = 0.1f;
    [SerializeField]
    float moveFactor = 1f;

    //定义摄像机可以活动的范围
    [SerializeField]
    private float xMin = -8;
    [SerializeField]
    private float xMax = 8;
    [SerializeField]
    private float yMin = -5;
    [SerializeField]
    private float yMax = 5;

    //这个变量用来记录单指双指的变换
    private bool m_IsSingleFinger;

    //初始化游戏信息设置
    void Start()
    {
        m_Camera = GetComponent<Camera>();
        m_Camera.orthographicSize = 10f;
        m_CameraOffset = m_Camera.transform.position;
    }

    void Update()
    {
#if !UNITY_EDITOR
        //判断触摸数量为单点触摸
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began || !m_IsSingleFinger)
            {
                //在开始触摸或者从两字手指放开回来的时候记录一下触摸的位置
                lastSingleTouchPosition = Input.GetTouch(0).position;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                MovingCamera(Input.GetTouch(0).position);
            }
            m_IsSingleFinger = true;

        }
        else if (Input.touchCount == 2)
        {
            //当从单指触摸进入多指触摸的时候,记录一下触摸的位置
            //保证计算缩放都是从两指手指触碰开始的
            if (m_IsSingleFinger)
            {
                oldPosition1 = Input.GetTouch(0).position;
                oldPosition2 = Input.GetTouch(1).position;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                ScaleCamera();
            }

            m_IsSingleFinger = false;
        }       
#else
        m_Camera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * sizeFactor;
        m_Camera.orthographicSize = Mathf.Clamp(m_Camera.orthographicSize, minSize, maxSize);
        m_CameraOffset = new Vector3(Mathf.Clamp(m_CameraOffset.x, xMin + 16 / 9f * m_Camera.orthographicSize, xMax - 16 / 9f * m_Camera.orthographicSize), Mathf.Clamp(m_CameraOffset.y , yMin + m_Camera.orthographicSize, yMax - m_Camera.orthographicSize), m_CameraOffset.z);
        if (Input.GetMouseButtonDown(0))
        {
            lastSingleTouchPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            MovingCamera(Input.mousePosition);
        }
#endif
    }

    /// <summary>
    /// 触摸缩放摄像头
    /// </summary>
    private void ScaleCamera()
    {
        //计算出当前两点触摸点的位置
        var tempPosition1 = Input.GetTouch(0).position;
        var tempPosition2 = Input.GetTouch(1).position;


        float currentTouchDistance = Vector3.Distance(tempPosition1, tempPosition2);
        float lastTouchDistance = Vector3.Distance(oldPosition1, oldPosition2);


        //计算上次和这次双指触摸之间的距离差距
        //然后去更改摄像机的距离
        m_Camera.orthographicSize -= (currentTouchDistance - lastTouchDistance) * sizeFactor * Time.deltaTime;


        //把距离限制住在min和max之间
        m_Camera.orthographicSize = Mathf.Clamp(m_Camera.orthographicSize, minSize, maxSize);
        //m_CameraOffset = new Vector3(Mathf.Clamp(m_CameraOffset.x, xMin + 16 / 9f * m_Camera.orthographicSize, xMax - 16 / 9f * m_Camera.orthographicSize), Mathf.Clamp(m_CameraOffset.y + m_Camera.orthographicSize, yMin, yMax - m_Camera.orthographicSize), -10);


        //备份上一次触摸点的位置，用于对比
        oldPosition1 = tempPosition1;
        oldPosition2 = tempPosition2;
    }

    //Update方法一旦调用结束以后进入这里算出重置摄像机的位置
    private void LateUpdate()
    {
        m_Camera.transform.position = m_CameraOffset;
    }

    private void MovingCamera(Vector3 scenePos)
    {
        Vector3 lastTouchPostion = m_Camera.ScreenToWorldPoint(new Vector3(lastSingleTouchPosition.x, lastSingleTouchPosition.y, -10));
        Vector3 currentTouchPosition = m_Camera.ScreenToWorldPoint(new Vector3(scenePos.x, scenePos.y, -10));

        Vector3 v = currentTouchPosition - lastTouchPostion;
        m_CameraOffset += new Vector3(v.x, v.y, 0) * -moveFactor;

        //把摄像机的位置控制在范围内
        m_CameraOffset = new Vector3(Mathf.Clamp(m_CameraOffset.x, xMin + 16 / 9f * m_Camera.orthographicSize, xMax - 16 / 9f * m_Camera.orthographicSize), Mathf.Clamp(m_CameraOffset.y, yMin + m_Camera.orthographicSize, yMax - m_Camera.orthographicSize), -10);
        //Debug.Log(lastTouchPostion + "|" + currentTouchPosition + "|" + v);
        lastSingleTouchPosition = scenePos;
    }

}
