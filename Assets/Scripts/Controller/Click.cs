using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 关卡中的点击效果
/// </summary>
public class Click : MonoBehaviour
{

    List<Line> bigLine = new List<Line>();
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
                        foreach (var bl in bigLine)
                            bl.ChangeState(LineState.isChoose);
                        break;
                    case LineState.isChoose:
                        if (clickStep > 0)
                        {
                            Map.Instance.RemoveLine(bigLine);
                            foreach (var bl in bigLine)
                                Destroy(bl.gameObject);
                            clickStep--;
                        }
                        break;
                }
            }
        }
    }
}
