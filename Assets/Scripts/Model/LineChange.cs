using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlendModes;
using cakeslice;

public class LineChange : MonoBehaviour
{
    //camera的
    private OutlineEffect ole;
    //两个子物体
    private GameObject lineGo;
    private GameObject lightGo;
    //这个线
    private Line line;
    //红边渐隐
    public bool B_Build1 = false;
    //线出
    public bool B_Build2 = false;
    //红边渐现
    public bool B_Build3 = false;
    //一个开关
    private bool bInit = true;
    private Outline OL;

    private void OnEnable()
    {
        lineGo = transform.Find("Line").gameObject;
        lightGo = transform.Find("Light").gameObject;
        var camera = GameObject.FindGameObjectWithTag("MainCamera");
        if (camera.GetComponent<OutlineEffect>() != null)
            ole = camera.GetComponent<OutlineEffect>();
        else
            ole = camera.AddComponent<OutlineEffect>();
        line = GetComponent<Line>();
        OL = GetLineGoComponent<Outline>();
    }

    //方向
    private float dir;
    // Use this for initialization
    private void Start()
    {
        if (GameManager.Instance.Step <= 1)
            return;
        GetLineGoComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        lightGo.SetActive(false);
        dir = Random.Range(0, 1);
        OL.enabled = false;

    }

    [SerializeField]
    [Header("白边出现速率，默认1")]
    private float whiteAlpahUpSpeed = 1;
    void WhiteAlphaUp()
    {
        var c = ole.lineColor0;
        c.a += Time.deltaTime * whiteAlpahUpSpeed;
        c.a = Mathf.Clamp01(c.a);
        if (c.a >= 0.98f)
        {
            B_Build3 = false;
            c.a = 1;
        }
        ole.lineColor0 = c;
        ole.UpdateMaterialsPublicProperties();
    }

    [SerializeField]
    [Header("白边消失速率，默认1")]
    private float whiteAlpahDownSpeed = 1;
    void WhiteAlphaDown()
    {
        if(OL.enabled == false)
        {
            B_Build1 = false;
            B_Build2 = true;
        }
        var c = ole.lineColor0;
        c.a -= Time.deltaTime * whiteAlpahDownSpeed;
        c.a = Mathf.Clamp01(c.a);
        if (c.a <= 0.05f)
        {
            B_Build1 = false;
            B_Build2 = true;
            OL.enabled = false;
            c.a = 0;
        }
        ole.lineColor0 = c;
        ole.UpdateMaterialsPublicProperties();

    }

    [SerializeField]
    [Header("线出现速率，默认1")]
    private float lineAlphaUpSpeed = 1;
    private void LineAlphaUp()
    {
        if(lineGo.GetComponent<SetImageAlpha>() == null)
        {
            //lineGo.GetComponent<SpriteRenderer>().enabled = false;
            var sia = lineGo.AddComponent<SetImageAlpha>();            
            if (dir > 0.5f)
                sia.leftX = 1;
            else
                sia.rightX = 1;
        }
        var ia = GetLineGoComponent<SetImageAlpha>();

       
        if (dir > 0.5f)
        {
            float x = ia.leftX;
            x -= Time.deltaTime * lineAlphaUpSpeed;
            x = Mathf.Clamp01(x);
            if (x <= 0.05f)
            {
                B_Build3 = true;
                x = 0;
            }
            ia.leftX = x;
        }
        else
        {
            float x = ia.rightX;
            x -= Time.deltaTime * lineAlphaUpSpeed;
            x = Mathf.Clamp01(x);
            if (x <= 0.05f)
            {
                B_Build3 = true;
                x = 0;
            }
            ia.rightX = x;
        }
        if (B_Build3)
        {
            OverShowChange();
            B_Build2 = false;
        }
    }

    private void OverShowChange()
    {
        if (lightGo.activeSelf == false)
        {
            lightGo.SetActive(true);
            GetLineGoComponent<SpriteRenderer>().color = new Color(1,1,1,0);
        }
        if (lineGo.GetComponent<SetImageAlpha>() != null)
            DestroyImmediate(lineGo.GetComponent<SetImageAlpha>());
        //lineGo.GetComponent<SpriteRenderer>().enabled = true;
        GetLineGoComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        var blend = GetLineGoComponent<BlendModeEffect>();
        blend.BlendMode = BlendMode.Multiply;
    }
    
    [SerializeField]
    [Header("选中后外发光速率，默认1")]
    private float a2 = 1;
    private bool b2 = false;
    private void OutLineChange(LineState lineState)
    {
        if (B_Build1)
            return;
        if (lineState != LineState.ready)
        {
            if (lightGo.GetComponent<SpriteRenderer>().color.a < 0.95f)
            {
                var c = lightGo.GetComponent<SpriteRenderer>().color;
                c.a += Time.deltaTime;
                c.a = Mathf.Clamp01(c.a);
                if (c.a >= 0.95f)
                {
                    c.a = 1;
                }
                lightGo.GetComponent<SpriteRenderer>().color = c;
            }
        }
        switch (lineState)
        {
            case LineState.ready:
                break;

            case LineState.show:
                if (line.BStatic)
                {
                    OL.enabled = true;
                    OL.color = 1;
                }
                else
                    OL.enabled = false;
                break;
                
            case LineState.isChoose:

                if (OL.enabled == false)
                    OL.enabled = true;
                OL.color = 2;

                var c2 = ole.lineColor2;
                if (b2)
                {
                    c2.a += Time.deltaTime * a2;
                    if (c2.a >= 1)
                        b2 = false;
                }
                else
                {
                    c2.a -= Time.deltaTime * a2;
                    if (c2.a <= 0)
                        b2 = true;
                }
                c2.a = Mathf.Clamp01(c2.a);
                ole.lineColor2 = c2;
                ole.UpdateMaterialsPublicProperties();
                break;
        }
    }
    
    private void Init()
    {
        switch (line.GetState())
        {
            case LineState.ready:
                lightGo.SetActive(false);
                GetLineGoComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                OL.enabled = true;
                break;
            case LineState.show:
                if (line.BStatic == false)
                {
                    OL.enabled = false;
                }
                else
                {
                    OL.enabled = true;
                    OL.color = 1;
                }
                var bme = GetLineGoComponent<BlendModeEffect>();
                bme.BlendMode = BlendMode.Multiply;            
                lightGo.SetActive(true);
                break;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.Instance.BDefeat)
            return;
        if(GameManager.Instance.Step <= 1 && line.GetState() != LineState.isChoose)
        {
            Init();
            return;
        }
        if (bInit)
        {
            foreach(var node in Map.Instance.nodes.Nodes)
            {
                foreach(var line in node.LineList)
                {
                    var lc = line.GetComponent<LineChange>();
                    if (lc.B_Build1 == true || lc.B_Build2 == true)
                    {
                        goto aa;
                    }
                }
            }
            bInit = false;
            B_Build3 = true;
            OL.enabled = true;
        }
        aa:
        if (B_Build3)
        {
            WhiteAlphaUp();
        }
        if (B_Build1)
        {
            WhiteAlphaDown();
        }
        if (B_Build2)
        {
            LineAlphaUp();
        }
        OutLineChange(line.GetState());
    }

    private T GetLineGoComponent<T>() where T : Component
    {
        T component;
        if (lineGo.GetComponent<T>() == null)
        {
            component = lineGo.AddComponent<T>();
        }
        else
            component = lineGo.GetComponent<T>();
        return component;
    }
}
