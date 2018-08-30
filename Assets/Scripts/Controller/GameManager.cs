using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameManager : MonoBehaviour {

    //-----------------------------------------------
    //字段及变量
    [SerializeField]
    [Header("无敌")]
    private bool xiaoze = false;

    protected RecordSystem m_recordSystem = new RecordSystem();

    protected bool bDefeat;
    public bool BDefeat { get { return BDefeat; } }

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
    protected List<Line> addLines;
    
    //-------------------------------------------------
    //方法

    //游戏循环
    protected void Init()
    {
        bDefeat = false;
        xiaoze = false;
        Map.Instance.InitMap_Node();
        addLines = new List<Line>();
        _step = 0;
        score = 0;
    }

    public void NextStep()
    {
        print(_step);
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
                    ShowData(LoadData());
                    Fail();
                }
            }
        }
        addLines.Clear();
    }

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
        GameObject line;
        if (bStatic)
            line = Resources.Load<GameObject>("StaticLine");
        else
            line = Resources.Load<GameObject>("Line");
        line = Instantiate(line, gameObject.transform);
        line.GetComponent<Line>().Init(nodes);
        foreach (var n in nodes)
        {
            n.TempleLineIndex++;
            n.TempleLine.Add(line.GetComponent<Line>());
        }
        if (bStatic)
            Map.Instance.AddStaticLine(line.GetComponent<StaticLine>());
        addLines.Add(line.GetComponent<Line>());
    }

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

    public void RePlay()
    {
        LevelManager.Instance.ReStart();
    }

    //文件系统
    public abstract void ShowData(RecordSaveData data);

    protected abstract void SaveData();

    protected abstract RecordSaveData LoadData();
}
