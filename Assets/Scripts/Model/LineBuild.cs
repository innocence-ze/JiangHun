using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Destructible2D;
using BlendModes;
using cakeslice;

public class LineBuild : MonoBehaviour {
    
    //camera的
    private OutlineEffect ole;
    //带有插件的子物体
    private GameObject lineGo;
    private GameObject lightGo;
    //生成点和终点
    private Node oneNode,anotherNode;
    //起始点位置
    private Vector3 pos;
    //方向
    private int dir;
    //这个线
    private Line line;
    //gamemanager
    private GameManager GM;
    private GameObject BG;
    public bool Enable { get; set; }

    public bool BBuild { get; set; }

    private void OnEnable()
    {
        lineGo = transform.Find("Line").gameObject;
        lightGo = transform.Find("Light").gameObject;
        if(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<OutlineEffect>() != null)
            ole = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<OutlineEffect>();
        else
            ole = GameObject.FindGameObjectWithTag("MainCamera").AddComponent<OutlineEffect>();
        ole.lineColor0.a = 0;
        line = GetComponent<Line>();
    }

    // Use this for initialization
    private void Start()
    {
        lightGo.SetActive(false);
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        BG = GameObject.FindGameObjectWithTag("BackGround");
        var d2d = lineGo.GetComponent<D2dDestructible>();
        d2d.HealTex = d2d.MainTex as Texture2D;
        for (int i = 0; i < line.Nodes.Count; i++)
        {
            if (line.Nodes[i].LineList.Count > 0)
            {
                oneNode = line.Nodes[i];
                anotherNode = line.Nodes[1 - i];
                dir = i * 2 - 1;
                pos = oneNode.Position;
                break;
            }
            else if (i == line.Nodes.Count - 1)
            {
                oneNode = line.Nodes[i];
                anotherNode = line.Nodes[1 - i];
                dir = i * 2 - 1;
                pos = oneNode.Position;
            }
        }
    }
	
    /// <summary>
    /// D2D换成Blend
    /// </summary>
    private void OnComponentChange()
    {
        lightGo.SetActive(true);

        DestroyImmediate(lineGo.GetComponent<D2dSorter>());
        DestroyImmediate(lineGo.GetComponent<Outline>());
        DestroyImmediate(lineGo.GetComponent<MeshRenderer>());
        DestroyImmediate(lineGo.GetComponent<MeshFilter>());

        var sr = lineGo.AddComponent<SpriteRenderer>();
        sr.sortingLayerName = "Line";
        var tex = lineGo.GetComponent<D2dDestructible>().MainTex as Texture2D;
        var spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), transform.Find("Light").GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
        sr.sprite = spr;
        DestroyImmediate(lineGo.GetComponent<D2dDestructible>());
        var blend = lineGo.AddComponent<BlendModeEffect>();
        blend.BlendMode = BlendMode.Multiply;
        lineGo.AddComponent<Outline>();
    }

    int index1 = 0;
    [SerializeField]
    [Header("渐变速率，默认1")]
    private float a2 = 1;
    private bool b1 = false;
    private bool b2 = false;
    private void OutLineChange(LineState lineState)
    {
        switch (lineState)
        {
            case LineState.ready:
                BBuild = false;
                lightGo.SetActive(false);
                foreach(var node in Map.Instance.nodes.Nodes)
                {
                    foreach(var line in node.LineList)
                    {
                        if (line.GetComponent<LineBuild>().BBuild)
                            goto AA;
                    }
                }

                lineGo.GetComponent<Outline>().color = 0;
                var c1 = ole.lineColor0;               
                if (index1 == 0 && b1)
                {
                    c1.a += Time.deltaTime;

                    if (c1.a >= 1)
                        index1 = 1;
                }
                else if(!b1)
                {
                    c1.a -= Time.deltaTime;

                    if (c1.a <= 0)
                        b1 = true;
                }
                c1.a = Mathf.Clamp01(c1.a);
                ole.lineColor0 = c1;
                ole.UpdateMaterialsPublicProperties();
                lineGo.GetComponent<D2dDestructible>().Color = new Color(1, 1, 1, 0);
                break;

                AA: ole.lineColor0.a = 0;
                break;

            case LineState.show:
                if (line.BStatic)
                {
                    lineGo.GetComponent<Outline>().color = 1;
                    foreach (var node in Map.Instance.nodes.Nodes)
                    {
                        foreach (var line in node.LineList)
                        {
                            if (line.GetComponent<LineBuild>().BBuild)
                                goto BB;
                        }
                    }
                    ole.lineColor1.a = 1;
                }                               
                else
                    lineGo.GetComponent<Outline>().enabled = false;
                break;

                BB: ole.lineColor1.a = 0;
                break;

            case LineState.isChoose:

                if (lineGo.GetComponent<Outline>().enabled == false)
                    lineGo.GetComponent<Outline>().enabled = true;
                lineGo.GetComponent<Outline>().color = 2;

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

    private bool bInit = true;
    private float timer = 0;
    // Update is called once per frame
    private void Update () {
        if(BG == null)
        {
            BG = GameObject.FindGameObjectWithTag("BackGround");
        }

        OutLineChange(line.GetState());
        if(line.GetState() != LineState.ready)
        {
            if (lineGo.GetComponent<D2dDestructible>() != null)
            {
                BBuild = true;
                if (bInit && !GM.BDefeat)
                {
                    BG.GetComponent<D2dDestructible>().Indestructible = true;
                    D2dDestructible.SliceAll(oneNode.Position, anotherNode.Position, 1, GM.Tex, 1);
                    bInit = false;                    
                }
                lineGo.GetComponent<D2dDestructible>().Color = Color.white;
                timer += Time.deltaTime;
                if (timer >= 0.07f)
                {
                    pos = pos - dir * new Vector3(Mathf.Cos(line.Rotation * Mathf.Deg2Rad), Mathf.Sin(line.Rotation * Mathf.Deg2Rad), 0) * 0.4f;
                    var v2 = new Vector2(1, 1);
                    D2dDestructible.StampAll(pos, v2, 0,GM.Tex, -1);
                    timer -= 0.07f;
                }
            }
            else
                BBuild = false;
            var n1 = (pos - oneNode.Position);
            var n2 = (pos - anotherNode.Position);
            if (lineGo.GetComponent<D2dDestructible>() != null && ( ( n1.x * n2.x >= 0 && n1.y * n2.y > 0) || (n1.x * n2.x > 0 && n1.y * n2.y >= 0)))
            {
                OnComponentChange();
            }

        }
    }
}
