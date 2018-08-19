using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 线的管理类，方法有找大边及环(都是静态的)
/// </summary>
public class LineManager {

    static public ArrayList FindBigLine(Line chooseLine)
    {
        InitLine();
        ArrayList bigLine = new ArrayList();
        chooseLine.IsUse = true;
        bigLine.Add(chooseLine);

        //DFS
        var temp = (Node)chooseLine.Nodes[0];
        OnesideBigLine(temp, ref bigLine);
        temp = (Node)chooseLine.Nodes[1];
        OnesideBigLine(temp, ref bigLine);

        InitLine();
        return bigLine;
    }

    static void OnesideBigLine(Node temp, ref ArrayList bigLine)
    {
        Stack<Node> bigNode = new Stack<Node>();
        bigNode.Push(temp);
        while (true)
        {
            temp = bigNode.Peek();
            if (temp.LineCount() != 2)
            {
                break;
            }
            float dRot = Mathf.Abs(temp.LineAt(0).Rotation - temp.LineAt(1).Rotation);
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

    public static ArrayList FindCircleLine(Line addLine)
    {
        InitLine();
        ArrayList circleLine = new ArrayList();//成环的线的数组
        Stack<Node> circleNodes = new Stack<Node>();//成环的点的栈
        addLine.IsUse = true;
        circleLine.Add(addLine);
        circleNodes.Push((Node)addLine.Nodes[0]);

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
                        if ((Node)anotherNode != temp)
                        {
                            circleLine.Add(temp.LineAt(d));
                            temp = (Node)anotherNode;
                            circleNodes.Push(temp);
                            break;
                        }
                    }
                    if (temp == (object)addLine.Nodes[1])
                    {
                        break;
                    }
                    d = 0;
                }
            }
            if (temp == (object)addLine.Nodes[1])
                break;
            foreach(Line l in circleLine)
            {
                if((object)l.Nodes[0] == circleNodes.Peek() || (object)l.Nodes[1] == circleNodes.Peek())
                circleLine.Remove(l);
            }
            circleNodes.Pop();
        }

        if(circleNodes.Count == 0)
        {
            circleLine = new ArrayList();
        }

        InitLine();

        return circleLine;
    }

    static void InitLine()
    {
        foreach (var i in NodeQueue.Nodes)
        {
            Node n = (Node)i;
            foreach (var j in n.LineList)
            {
                Line l = (Line)j;
                l.IsUse = false;
            }
        }
    }

}
