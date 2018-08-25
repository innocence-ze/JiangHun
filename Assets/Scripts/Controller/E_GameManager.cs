using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 无尽模式主脚本，主循环
/// </summary>
public class E_GameManager : MonoBehaviour {

    [SerializeField]
    [Header("记录当前步数，不用设定")]
    private int _step;

    [SerializeField]
    [Header("每次随机生成几条边，要设定，可修改")]
    private int randomIndex;

    [SerializeField]
    [Header("每次增加的线，不用设定")]
    private List<Line> addLines;

    [SerializeField]
    [Header("每步增加的可点击次数，要设定，可修改")]
    private int addClick;

    public GameObject overPanel;

    void Awake()
    {
        Map.Instance.InitMap_Node();
        addLines = new List<Line>();

        _step = 0;
        AddRandomLine(randomIndex);

        NextStep();
    }

    public void NextStep()
    {
        _step++;
        gameObject.GetComponent<Click>().ChangeClickStep(addClick);
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
        AddRandomLine(randomIndex);
    }

    private void LoadLine(List<Node> nodes)
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

    private void AddRandomLine(int randomIndex)
    {
        int addIndex = 0;

        //var _lineIndex = 0;
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

        var _index = 0;
        while (addIndex < randomIndex)
        {
            _index++;
            if (_index > 10000) { break; }
            var oneNode = Map.Instance.nodes.Contains(Random.Range(0, Map.Instance.nodes.Length));
            //这个点是否为空
            if (oneNode.LineCount() + oneNode.TempleLineIndex == 0 && _step != 0) continue;
            //如果是第一步
            else if(_step == 0 && oneNode.TempleLineIndex < oneNode.NearNode.Count)
            {
                var anotherNode = oneNode.FreeNode[Random.Range(0, oneNode.FreeNode.Count)];
                var nodes = new List<Node> { oneNode, anotherNode };
                if (!IsCircleLine(nodes))
                {
                    LoadLine(nodes);
                    addIndex++;
                }
                continue;
            }

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
        overPanel.GetComponent<ChoosePanel>().Stop();
    }

}
