﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 关卡中的点击效果
/// </summary>
public class Click : MonoBehaviour
{

    public int ClickScore { get; private set; }

    private int _1score = 100;

    List<Line> bigLine = new List<Line>();
    public int BLCount { get; private set; }
    [SerializeField]
    [Header("可操作数，要设定，可修改")]
    private int clickStep;

    public int ClickSteps
    {
        get
        {
            return clickStep;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    /// <summary>
    /// 改变点击步数时调用，index可正可负
    /// </summary>
    /// <param name="index"></param>
    public void ChangeClickStep(int index)
    {
        clickStep += index;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.GetComponent<Line>() != null)
            {
                Line l = hit.collider.gameObject.GetComponent<Line>();

                switch (l.GetState())
                {
                    case LineState.show:
                        Map.Instance.InitMap_Line();
                        bigLine = LineManager.FindBigLine(l);
                        BLCount = bigLine.Count;
                        foreach (var bl in bigLine)
                            bl.ChangeState(LineState.isChoose);
                        break;
                    case LineState.isChoose:
                        if (clickStep > 0)
                        {
                            BLCount = 0;
                            switch (bigLine.Count)
                            {
                                case 1:
                                    ClickScore += _1score;
                                    break;
                                case 2:
                                    ClickScore += _1score * 2;
                                    break;
                                case 3:
                                    ClickScore += _1score * 4;
                                    break;
                                case 4:
                                    ClickScore += _1score * 5;
                                    break;
                                case 5:
                                    ClickScore += _1score * 8;
                                    break;
                                case 6:
                                    ClickScore += _1score * 12;
                                    break;
                                case 7:
                                    ClickScore += _1score * 15;
                                    break;
                                case 8:
                                    ClickScore += _1score * 20;
                                    break;
                                case 9:
                                    ClickScore += _1score * 25;
                                    break;
                                case 10:
                                    ClickScore += _1score * 30;
                                    break;
                            }
                            Map.Instance.RemoveLine(bigLine);
                            foreach (var bl in bigLine)
                                Destroy(bl.gameObject);
                            clickStep--;
                            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Music/Erase"), Camera.main.transform.position);
                        }
                        break;
                }
            }
        }
    }
}
