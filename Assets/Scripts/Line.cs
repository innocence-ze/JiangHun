using System.Collections;
using System;
using UnityEngine;

/// <summary>
/// 线的类，字段有线的位置，旋转，长度，是否被选中
/// </summary>
public class Line : IComparable
{
    [SerializeField]
    private bool isChoose;
    [SerializeField]
    private float length;
    [SerializeField]
    private float rotation;
    [SerializeField]
    private Vector3 position;
    private ArrayList nodes = new ArrayList();

    /// <summary>
    /// 是否被选中
    /// </summary>
    public bool IsChoose
    {
        get { return isChoose; }
        set { isChoose = value; }
    }

    /// <summary>
    /// 获取线段的端点
    /// </summary>
    public ArrayList Nodes { get { return nodes; } }

    /// <summary>
    /// 寻路用
    /// </summary>
    public bool IsUse { get; set; }

    /// <summary>
    /// 线的长度(两端点的距离)
    /// </summary>
    public float Length
    {
        get { return length; }
        set { length = value; }
    }

    /// <summary>
    /// 线段的旋转角度(-90,90)
    /// </summary>
    public float Rotation
    {
        get { return rotation; }
        set { rotation = value; }
    }

    /// <summary>
    /// 线段的位置(transform.position)
    /// </summary>
    public Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }

    float CalculateLength()
    {
        Node node1 = (Node)nodes[0];
        Node node2 = (Node)nodes[1];
        return Vector3.Distance(node1.Position,node2.Position);
    }

    float CalculateRotation()
    {
        Node node1 = (Node)nodes[0];
        Node node2 = (Node)nodes[1];
        float dX = node1.Position.x - node2.Position.x;
        float dY = node1.Position.y - node2.Position.y;
        float tan = dY / dX;
        return Mathf.Atan(tan) * Mathf.Rad2Deg;
    }

    Vector3 CalculatePosition()
    {
        Node node1 = (Node)nodes[0];
        Node node2 = (Node)nodes[1];
        return (node1.Position + node2.Position) / 2;
    }

    /// <summary>
    /// Line的构造函数，nodes为线的两个端点构成的数组，储存两个端点的(Node)
    /// </summary>
    /// <param name="nodes"></param>
    public Line(ArrayList nodes)
    {       
        isChoose = false;
        IsUse = false;
        if (nodes.Count >= 2)
        {
            nodes.Sort();
            this.nodes[0] = nodes[0];
            this.nodes[1] = nodes[1];
            length = CalculateLength();
            rotation = CalculateRotation();
            position = CalculatePosition();
        }
       
    }

    public int CompareTo(object obj)
    {
        Line line = (Line)obj;
        if (line.rotation > rotation)
            return -1;
        else
            return 1;
    }
}
