using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 关卡内游戏主脚本，主逻辑
/// </summary>
public class R_GameManager : MonoBehaviour {
    [SerializeField]
    [Header("记录当前步数，不用设定")]
    private int _step;
    [SerializeField]
    [Header("储存随机步数，要设定")]
    private int r_step;
    [SerializeField]
    [Header("储存固定步数，不要设定")]
    private int f_step;

    [SerializeField]
    [Header("每次随机生成几条边，要设定")]
    private int randomIndex;

    [SerializeField]
    [Header("每次增加的线，不用设定")]
    private List<Line> addLines;
    [SerializeField]
    [Header("不可删除的线，不用设定")]
    private List<StaticLine> staticLines;
    private AddLineList addLineList;
    private bool bDefeat;

    public GameObject overPanel;

    // Use this for initialization
    void Awake()
    {
        Map.Instance.InitMap_Node();
        addLines = new List<Line>();
        staticLines = new List<StaticLine>();

        _step = 0;
        addLineList = GetComponent<AddLineList>();
        f_step = addLineList.eachLine_node.Length;
        AddFixedLine(_step);

        bDefeat = false;
    }

    public void NextStep()
    {
        _step++;

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
                Fail();
            }
        }
        addLines.Clear();
        //加ready的线
        if (_step < f_step)
        {
            AddFixedLine(_step);
        }
        else if(_step < f_step + r_step)
        {
            AddRandomLine(randomIndex);
        }
        //所有步数完成通关
        if (_step == f_step+r_step && !bDefeat) 
            Victory();
    }

    private void LoadLine(List<Node> nodes)
    {
        bool bStatic = false;
        foreach(var n in nodes)
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
            staticLines.Add(line.GetComponent<StaticLine>());
        addLines.Add(line.GetComponent<Line>());
    }

    private void AddFixedLine(int _step)
    {
        LineList linelist = addLineList.eachLine_node[_step];
        int numberOfLines = linelist.Array.Length / 2;

        for (int i = 0; i < numberOfLines; i++)
        {
            var nodes = new List<Node>
            {
                linelist.Array[i * 2].GetComponent<Node>(),
                linelist.Array[i * 2 + 1].GetComponent<Node>()
            };

            LoadLine(nodes);
        }
    }

    private void AddRandomLine(int randomIndex)
    {
        int addIndex = 0;
        var _lineIndex = 0;

        ////能不能加下这么多边TODO
        //foreach(var n in Map.Instance.nodes.Nodes)
        //{
        //    _lineIndex += n.FreeNode.Count;
        //}
        //if(_lineIndex < randomIndex * 2)
        //{
        //    Debug.Log("加的点太多了");
        //    return;
        //}

        //是否还有边
        _lineIndex = 0;
        foreach (var n in Map.Instance.nodes.Nodes)
        {
            _lineIndex += n.LineCount();
        }
        var _index = 0;
        while (addIndex < randomIndex)
        {
            _index++;
            if (_index > 10000) { break; }               
            var oneNode = Map.Instance.nodes.Contains(Random.Range(0, Map.Instance.nodes.Length));
            //是否存在边
            if (oneNode.LineCount() + oneNode.TempleLineIndex == 0 && _lineIndex + addIndex != 0) continue;
            if (oneNode.LineCount() + oneNode.TempleLineIndex < oneNode.NearNode.Count)
            {
                var anotherNode = oneNode.FreeNode[Random.Range(0, oneNode.FreeNode.Count)];
                var nodes = new List<Node> { oneNode, anotherNode };
                if (!IsCircleLine(nodes))
                {
                    LoadLine(nodes);
                    addIndex++;
                }               
            }
        } 
    }

    private bool IsCircleLine(List<Node> nodes)
    {
        bool isCircle = false;
        if (addLines.Count + staticLines.Count < 2)
            return false;

        var _lines = new List<Line>();
        foreach (var l in addLines)
        {
            _lines.Add(l);
            l.IsUse = false;
        }
        foreach (var l in staticLines)
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
                    foreach(var anotherNode in node_allLines[d].Nodes)
                    {
                        if(anotherNode != temp)
                        {
                            temp = anotherNode;
                            circleNodes.Push(temp);
                            break;
                        }
                    }
                    if(temp == nodes[1])
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

    public void RePlay()
    {
        LevelManager.Instance.ReStart();
    }

    public void Victory()
    {
        //Debug.Log("Victory");
        LevelManager.Instance.LoadNewLevel();
    }

    public void Fail()
    {
        bDefeat = true;
        overPanel.GetComponent<ChoosePanel>().Stop();
    }
}
