using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Destructible2D;

public abstract class GameManager : MonoBehaviour {

    //-----------------------------------------------
    //字段及变量
    [System.NonSerialized]
    private Texture2D stampTax;
    private Vector3 bgRot = new Vector3(40, 40, 55);
    [SerializeField]
    [Header("无敌")]
    private bool xiaoze = false;

    private static GameManager s_Instance = null;
    public static GameManager Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            }
            if (s_Instance == null)
                Debug.Log("Can't find GameManager");
            return s_Instance;
        }
    }


    protected RecordSystem m_recordSystem = new RecordSystem();

    public Texture2D Tex { get { return stampTax; } }

    [SerializeField]
    protected bool bDefeat = false;
    public bool BDefeat { get { return bDefeat; } }

    protected int score;
    public int Score { get; protected set; }

    public GameObject overPanel;

    [SerializeField]
    [Header("记录当前步数，不用设定")]
    protected int _step;
    public int Step
    {
        get
        {
            return _step;
        }
    }

    [SerializeField]
    [Header("每次增加的线，不用设定")]
    protected List<Line> addLines = new List<Line>();
    
    public int AddLineCount
    {
        get
        {
            return addLines.Count;
        }
    }
    //-------------------------------------------------
    //方法

    //游戏循环
    protected void Init()
    {
        stampTax = Resources.Load<Texture2D>("StampTex");
        bDefeat = false;
        xiaoze = false;
        Map.Instance.InitMap_Node();
        _step = 0;
        score = 0;
        foreach(var node in Map.Instance.nodes.Nodes)
        {
            node.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        }
    }

    public void NextStep()
    {

        //把ready的线添加到点上
        Map.Instance.AddLine(addLines);
        //所有点初始化
        Map.Instance.InitMap_Node();
        //所有线设为show
        foreach (Line l in addLines)
        {
            l.ChangeState(LineState.show);
        }
        //如果有环，gameover
        foreach (Line l in addLines)
        {
            var circle = LineManager.FindCircleLine(l);
            if (circle.Count != 0)
            {
                if (!xiaoze)
                {
                    DropOutBG(circle);

                    Fail();
                }
            }
        }
        foreach (var line in addLines)
        {
            line.GetComponent<LineChange>().B_Build1 = true;
        }
        addLines.Clear();
    }

    private void DropOutBG(List<Line> circleLines)
    {
        foreach(var line in circleLines)
        { 
            D2dDestructible.SliceAll(line.Nodes[0].transform.position, line.Nodes[1].transform.position, 0.2f, stampTax, 10);
            Destroy(line.gameObject);
        }
    }

    private List<GameObject> gos = new List<GameObject>();
    private Vector3 dirRot = new Vector3();
    /// <summary>
    /// 背景掉落后的旋转
    /// </summary>
    protected void ChangeBgState(int level = 0)
    {
        if (bDefeat && gos.Count == 0)
        {
            switch (level)
            {
                case 0:
                    bgRot.z /= 2;
                    break;
                case 4:
                    bgRot.z /= -4;
                    break;
                case 3:
                    bgRot.z /= -1.5f;
                    break;
                case 2:
                    bgRot.z /= 1.5f;
                    break;
                case 1:
                    bgRot.z /= 4;
                    break;
            }
            foreach (var g in GameObject.FindGameObjectsWithTag("BackGround"))
            {
                if (g.transform.childCount == 0)
                {
                    gos.Add(g);
                }
            }
            foreach (var g in gos)
            {
                g.GetComponent<D2dSorter>().SortingLayerName = "Drop";
            }
        }
        if (gos.Count != 0)
        {
            foreach(var go in gos)
            {
                dirRot += bgRot * Time.deltaTime;

                go.transform.rotation = Quaternion.Euler(dirRot);
            }
        }
        
    }

    /// <summary>
    /// 背景复原
    /// </summary>
    /// <returns></returns>
    public GameObject Find_HealBG()
    {
        gos.Clear();
        var bgs = GameObject.FindGameObjectsWithTag("BackGround");
        GameObject bg = null;
        foreach (var go in bgs)
        {
            if (go.transform.childCount == 0)
            {
                Destroy(go, 2f);
            }
            else
            {
                bg = go;
                bg.GetComponent<D2dDestructible>().ResetAlpha();
            }
        }
        return bg;
    }

    /// <summary>
    /// 实例化线
    /// </summary>
    /// <param name="nodes"></param>
    protected void LoadLine(List<Node> nodes)
    {
        bool bStatic = false;
        foreach (var n in nodes)
        {
            if (n.B_Fragile)
            {
                n.B_Fragile = false;
                bStatic = true;
            }
        }
        var length = Vector3.Distance(nodes[0].Position, nodes[1].Position);
        GameObject line;
        if (length < 2)
        {
            line = Resources.Load<GameObject>("Line1");
        }
        else if (length < 3.5f)
        {
            line = Resources.Load<GameObject>("Line2");
        }
        else if (length < 5)
        {
            line = Resources.Load<GameObject>("Line3");
        }
        else
        {
            line = Resources.Load<GameObject>("Line4");
        }
        line = Instantiate(line, gameObject.transform);
        line.GetComponent<Line>().Init(nodes, bStatic);
        foreach (var n in nodes)
        {
            n.TempleLineIndex++;
            n.TempleLine.Add(line.GetComponent<Line>());
        }
        if (bStatic)
            Map.Instance.AddStaticLine(line.GetComponent<Line>());
        addLines.Add(line.GetComponent<Line>());
    }

    /// <summary>
    /// 添加的线是否自成环
    /// </summary>
    /// <param name="nodes"></param>
    /// <returns></returns>
    protected bool IsCircleLine(List<Node> nodes)
    {
        bool isCircle = false;
        if (addLines.Count + Map.Instance.StaticLines.Count < 2)
            return false;

        var _lines = new List<Line>();
        foreach (var l in addLines)
        {
            _lines.Add(l);
            l.IsUse = false;
        }
        foreach (var l in Map.Instance.StaticLines)
        {
            _lines.Add(l);
            l.IsUse = false;
        }

        var circleNodes = new Stack<Node>();//成环的点的栈
        circleNodes.Push(nodes[0]);
        var node_allLines = new List<Line>();

        while (circleNodes.Count != 0)
        {
            var temp = circleNodes.Peek();
            int d = 0;
            while (d < temp.LineCount() + temp.TempleLineIndex)
            {
                node_allLines.Clear();
                foreach (var l in temp.LineList)
                    node_allLines.Add(l);
                foreach (var l in temp.TempleLine)
                    node_allLines.Add(l);
                if (!_lines.Contains(node_allLines[d]) || node_allLines[d].IsUse)
                {
                    d++;
                }
                else
                {
                    node_allLines[d].IsUse = true;
                    foreach (var anotherNode in node_allLines[d].Nodes)
                    {
                        if (anotherNode != temp)
                        {
                            temp = anotherNode;
                            circleNodes.Push(temp);
                            break;
                        }
                    }
                    if (temp == nodes[1])
                    {
                        isCircle = true;
                        _lines.Clear();
                        return isCircle;
                    }
                    d = 0;
                }
            }
            circleNodes.Pop();
        }
        _lines.Clear();
        return isCircle;
    }

    //游戏结束
    public abstract void Fail();

    public abstract void Victory();

    //public void RePlay()
    //{        
    //    LevelManager.Instance.ReStart();
    //}

    //文件系统
    public abstract void ShowData(RecordSaveData data);

    protected abstract void SaveData();

    protected abstract RecordSaveData LoadData();


}
