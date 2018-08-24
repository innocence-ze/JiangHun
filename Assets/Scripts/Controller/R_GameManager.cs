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
        Map.Instance.AddLine(addLines);
        Map.Instance.InitMap_Node();
        foreach (Line l in addLines)
        {
            l.ChangeState(LineState.show);
        }
        foreach (Line l in addLines)
        {
            var circle = LineManager.FindCircleLine(l);
            if (circle.Count != 0)
            {
                Fail();
            }
        }
        addLines.Clear();
        if (_step < f_step)
        {
            AddFixedLine(_step);
        }
        else if(_step < f_step + r_step)
        {
            AddRandomLine(randomIndex);
        }
        if (_step == f_step+r_step && !bDefeat) 
            Victory();
    }

    void AddFixedLine(int _step)
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

            //TODO 设置父物体,修改随机生成的线
            GameObject line;
            if (Random.Range(0f, 2f) > 2f)
                line = Resources.Load<GameObject>("StaticLine");
            else
                line = Resources.Load<GameObject>("Line");
            line = Instantiate(line, gameObject.transform);
            line.GetComponent<Line>().Init(nodes);
            addLines.Add(line.GetComponent<Line>());
        }
    }

    void AddRandomLine(int randomIndex)
    {
        int addIndex = 0;
        var _index = 0;
        foreach(var n in Map.Instance.nodes.Nodes)
        {
            _index += n.FreeNode.Count;
        }
        if(_index < randomIndex * 2)
        {
            Debug.Log("加的点太多了");
            return;
        }
        while (addIndex < randomIndex)
        {
            var oneNode = Map.Instance.nodes.Contains(Random.Range(0, Map.Instance.nodes.Length));
            if (oneNode.LineCount() + oneNode.TempleLineIndex == 0) continue;
            if (oneNode.LineCount() + oneNode.TempleLineIndex < oneNode.NearNode.Count)
            {
                var anotherNode = oneNode.FreeNode[Random.Range(0, oneNode.FreeNode.Count)];
                var nodes = new List<Node> { oneNode, anotherNode };
                if (!IsCircleLine(nodes))
                {
                    GameObject line = Resources.Load<GameObject>("Line");
                    line = Instantiate(line, gameObject.transform);
                    line.GetComponent<Line>().Init(nodes);
                    foreach(var n in nodes)
                    {
                        n.TempleLineIndex++;
                        n.TempleLine.Add(line.GetComponent<Line>());
                    }
                    addLines.Add(line.GetComponent<Line>());
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
        Debug.Log("Defeat");
    }
}
