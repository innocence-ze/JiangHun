using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 线的管理类，方法有找大边及环(都是静态的)
/// </summary>
public class LineManager {

    static public List<Line> FindBigLine(Line chooseLine)
    {
        Map.Instance.InitMap_Line();
        List<Line> bigLine = new List<Line>();
        chooseLine.IsUse = true;
        bigLine.Add(chooseLine);

        //DFS
        var temp = chooseLine.Nodes[0];
        OnesideBigLine(temp, ref bigLine);
        temp = chooseLine.Nodes[1];
        OnesideBigLine(temp, ref bigLine);

        Map.Instance.InitMap_Line();
        return bigLine;
    }

    static void OnesideBigLine(Node temp, ref List<Line> bigLine)
    {
        Stack<Node> bigNode = new Stack<Node>();
        bigNode.Push(temp);
        while (true)
        {
            temp = bigNode.Peek();
            if (temp.LineCount() != 2 || temp.LineAt(0) is StaticLine || temp.LineAt(1) is StaticLine)
            {
                break;
            }
            float dRot = Mathf.Abs(temp.LineAt(0).Rotation - temp.LineAt(1).Rotation);
            if (temp.LineAt(1).Nodes[0] == temp.LineAt(0).Nodes[0] || temp.LineAt(1).Nodes[1] == temp.LineAt(0).Nodes[1])
                break;
            if (dRot < 15 || dRot > 165)
            {
                foreach (Line l in temp.LineList)
                {
                    if (!l.IsUse)
                    {
                        l.IsUse = true;
                        bigLine.Add(l);
                        foreach (Node n in l.Nodes)
                        {
                            if (n != temp)
                                bigNode.Push(n);
                        }
                    }
                }
            }
            else
            {
                break;
            }
        }
    }

    /// <summary>
    /// 找环
    /// </summary>
    /// <param name="addLine"></param>
    /// <returns>环</returns>
    public static List<Line> FindCircleLine(Line addLine)
    {
        Map.Instance.InitMap_Line();
        List<Line> circleLine = new List<Line>();//成环的线的数组
        Stack<Node> circleNodes = new Stack<Node>();//成环的点的栈
        addLine.IsUse = true;
        circleLine.Add(addLine);
        circleNodes.Push(addLine.Nodes[0]);

        //DFS
        while(circleNodes.Count != 0)
        {
            var temp = circleNodes.Peek();
            int d = 0;

            while (d < temp.LineCount())
            {
                if (temp.LineAt(d).IsUse)
                {
                    d++;
                }
                else
                {
                    temp.LineAt(d).IsUse = true;
                    foreach (var anotherNode in temp.LineAt(d).Nodes)
                    {
                        if (anotherNode != temp && temp.LineAt(d).gameObject.tag == "Line")
                        {
                            circleLine.Add(temp.LineAt(d));
                            temp = anotherNode;
                            circleNodes.Push(temp);
                            break;
                        }
                    }
                    if (temp == addLine.Nodes[1])
                    {
                        break;
                    }
                    d = 0;
                }
            }
            if (temp == addLine.Nodes[1])
                break;
            for (int i = 0; i < circleLine.Count; i++)
            {
                if (circleLine[i].Nodes[0] == circleNodes.Peek() || circleLine[i].Nodes[1] == circleNodes.Peek())
                    circleLine.Remove(circleLine[i]);
            }
            circleNodes.Pop();
        }

        if(circleNodes.Count == 0)
        {
            circleLine.Clear();
        }

        Map.Instance.InitMap_Line();

        return circleLine;
    }

}
