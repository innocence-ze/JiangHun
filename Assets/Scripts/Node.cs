﻿using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 点的类，字段有点的位置与点上的线的集合
/// 方法有在点上对线进行增删查及找线的数量
/// 排序方式先x后y
/// </summary>
public class Node : IComparable
{
    [SerializeField]
    private ArrayList lineList = new ArrayList();
    [SerializeField]
    private Vector3 position;

    public ArrayList LineList { get { return lineList; } }

    /// <summary>
    /// 返回位于index的线
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Line LineAt(int index)
    {
        if (lineList.Count > index)
            return (Line)lineList[index];
        else
            return null;
    }

    /// <summary>
    /// 点的位置(transform.position)
    /// </summary>
    public Vector3 Position
    {
        get
        {
            return position;
        }
    }

    /// <summary>
    /// 构造函数，position = transform.position
    /// </summary>
    /// <param name="position"></param>
    public Node(Vector3 position)
    {
        this.position = position;
    }

    /// <summary>
    /// 在点上添加边
    /// </summary>
    /// <param name="line"></param>
    public void AddLine(Line line)
    {
        lineList.Add(line);
        lineList.Sort();
    }

    /// <summary>
    /// 删除点上的边
    /// </summary>
    /// <param name="line"></param>
    public void RemoveLine(Line line)
    {
        lineList.Remove(line);
        lineList.Sort();
    }

    /// <summary>
    /// 该点上的线的数量
    /// </summary>
    /// <returns></returns>
    public int LineCount()
    {
        return lineList.Count;
    }

    /// <summary>
    /// 这条线是否在点上
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public bool ContainLine(Line line)
    {
        return lineList.Contains(line);
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
