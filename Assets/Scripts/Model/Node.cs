using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 点的类，字段有点的位置与点上的线的集合
/// 方法有在点上对线进行增删查及找线的数量
/// 排序方式先x后y
/// </summary>
public class Node : MonoBehaviour, IComparable
{
    [SerializeField]
    private List<Line> lineList = new List<Line>();
    [SerializeField]
    private Vector3 position;
    [SerializeField]
    [Header("可与哪些点形成连线，需要手动添加")]
    private List<Node> nearNode = new List<Node>();
    [SerializeField]
    private List<Line> templeLine = new List<Line>();

    [SerializeField]
    [Header("是否为脆弱的点")]
    private bool bFragile;

    public bool B_Fragile { get { return bFragile; } set { bFragile = value; } }

    public int TempleLineIndex{ get; set; }

    public List<Line> LineList { get { return lineList; } }
    public List<Node> NearNode { get { return nearNode; } }
    public List<Line> TempleLine { get { return templeLine; } set { templeLine = value; } }
    public List<Node> FreeNode
    {
        private set {; }
        get
        {
            var freeNode = new List<Node>();
            foreach(var n in nearNode)
            {
                freeNode.Add(n);
            }
            if (lineList.Count + TempleLineIndex == 0)
                return freeNode;
            if (lineList.Count == nearNode.Count)
                return new List<Node>();
            foreach (var l in lineList)
            {
                foreach (var n in l.Nodes)
                {
                    if (n != this)
                    {
                        freeNode.Remove(n);
                        break;
                    }
                }
            }
            foreach(var l in templeLine)
            {
                foreach (var n in l.Nodes)
                {
                    if (n != this)
                    {
                        freeNode.Remove(n);
                        break;
                    }
                }
            }
            return freeNode;
        }
    }
    /// <summary>
    /// 返回位于index的线
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Line LineAt(int index)
    {
        if (LineCount() > index)
        {
            return lineList[index];
        }
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
    /// 初始函数，position = transform.position
    /// </summary>
    /// <param name="position"></param>
    public void Init(Vector3 position)
    {
        this.position = position;
    }

    /// <summary>
    /// 初始函数，position = transform.position，nodes = 周围的点
    /// </summary>
    /// <param name="position"></param>
    /// <param name="nodes"></param>
    public void Init(Vector3 position, List<Node> nodes)
    {
        this.position = position;
        foreach(var n in nodes)
        {
            n.NearNode.Add(this);
            NearNode.Add(n);
        }
    }

    /// <summary>
    /// 在点上添加边
    /// </summary>
    /// <param name="line"></param>
    public void AddLine(Line line)
    {
        lineList.Add(line);
    }

    /// <summary>
    /// 删除点上的边
    /// </summary>
    /// <param name="line"></param>
    public void RemoveLine(Line line)
    {
        lineList.Remove(line);
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
        else if(position.x == node.position.x)
        {
            if (position.y > node.position.y)
            {
                return 1;
            }
            else if (position.y < node.position.y)
            {
                return -1;
            }
            else
                return 0;
        }         
        else
           return 0;
        
    }
}
