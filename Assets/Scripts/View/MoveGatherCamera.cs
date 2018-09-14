using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MoveGatherCamera : MoveCamera {

    GameObject canvas;
    Image[] image;
    public bool hide;
    // Use this for initialization
    new void Start () {
        base.Start();
        ChangeBoundary();
        canvas = GameObject.FindGameObjectWithTag("UI");
        image = canvas.GetComponentsInChildren<Image>();
        hide = false;
    }

    Vector3 down, up;
    // Update is called once per frame
    new void Update()
    {
        ChangeBoundary();
        base.Update();
        //(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        if (Input.GetMouseButtonDown(0))
        {
            down = Input.mousePosition;

        }
        if (Input.GetMouseButtonUp(0))
        {
            up = Input.mousePosition;
            if (Vector3.Distance(down, up) < 10f)
            {
                if (!BOnUI())
                {
                    if (!hide)
                        HideUI();
                    else
                        ShowUI();
                }
            }
            else if (!BOnUI())
            {
                if (!(canvas.activeSelf))
                    return;
                HideUI();
            }
        }
#elif UNITY_IPHONE || UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                down = Input.GetTouch(0).position;

            }
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)
            {               
                up = Input.GetTouch(0).position;
                if (Vector3.Distance(down, up) < 10f)
                {
                    if (!BOnUI())
                    {
                        if (!hide)
                            HideUI();
                        else
                            ShowUI();
                    }
                }
                else if (!BOnUI())
                {
                    if (!(canvas.activeSelf))
                        return;
                    HideUI();
                }
            }
        }
#endif
    }

    void ChangeBoundary()
    {
        var BG = GameObject.FindGameObjectWithTag("BackGround").GetComponent<SpriteRenderer>().sprite;
        var width = BG.texture.width / BG.pixelsPerUnit;
        var height = BG.texture.height / BG.pixelsPerUnit;
        xMax = width / 2f;
        xMin = -xMax;
        yMax = height / 2f;
        yMin = -yMax;
        maxSize = (xMax / 16 > yMax / 9) ? yMax : 9f / 16 * xMax;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMin + 16 / 9f * m_Camera.orthographicSize, xMax - 16 / 9f * m_Camera.orthographicSize), Mathf.Clamp(transform.position.y, yMin + m_Camera.orthographicSize, yMax - m_Camera.orthographicSize), -10);
        m_Camera.orthographicSize = Mathf.Clamp(m_Camera.orthographicSize, minSize, maxSize);
    }

    void HideUI()
    {
        hide = true;
        canvas.transform.Find("Left").GetComponent<GatheringPanel>().MoveLeft();
        canvas.transform.Find("Right").GetComponent<GatheringPanel>().MoveRight();

        foreach(var i in image)
        {
            if (i.sprite != null)
                i.DOColor(new Color(1, 1, 1, 0), 1);
            i.raycastTarget = false;
        }
    }

    void ShowUI()
    {
        hide = false;
        foreach (var i in image)
        {
            if (i.gameObject.name == "Shade")
                continue;
            if (i.sprite != null)
                i.DOColor(new Color(1, 1, 1, 1), 1);
            i.raycastTarget = true;
        }
    }
}
