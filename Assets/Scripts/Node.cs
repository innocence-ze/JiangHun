using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 点的类，字段有点的位置与点上的线的集合
/// </summary>
public class Node : IComparable
{
    [SerializeField]
    private ArrayList LineList = new ArrayList();
    [SerializeField]
    private Vector3 position;

    public Vector3 Position
    {
        get
        {
            return position;
        }
    }

    public Node(Vector3 position)
    {
        this.position = position;
    }

    public void AddLine(Line line)
    {
        LineList.Add(line);
        LineList.Sort();
    }

    public void RemoveLine(Line line)
    {
        LineList.Remove(line);
        LineList.Sort();
    }

    public int LineCount()
    {
        return LineList.Count;
    }

    public bool ContainLine(Line line)
    {
        return LineList.Contains(line);
    }

    public int CompareTo(object obj)
    {
        Node node = (Node)obj;
        if (position.x < node.position.x)
            return -1;        
        else if (position.x > node.position.x)
            return 1;
        else
        {
            if (position.y > node.position.y)
                return 1;
            else
                return -1;
        }
    }
}
