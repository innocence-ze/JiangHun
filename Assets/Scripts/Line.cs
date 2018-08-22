using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// 线的类，字段有线的位置，旋转，长度，是否被选中
/// </summary>
/// 
public class Line : MonoBehaviour
{
    [SerializeField]
    protected LineState linestate;
    [SerializeField]
    private float length;
    [SerializeField]
    private float rotation;
    [SerializeField]
    private Vector3 position;
    [SerializeField]
    private List<Node> nodes = new List<Node>();


    public LineState GetState()
    {
        return linestate;
    }

    /// <summary>
    /// 获取线段的端点
    /// </summary>
    public List<Node> Nodes { get { return nodes; } }

    /// <summary>
    /// 寻路用
    /// </summary>
    [SerializeField]
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
        Node node1 = nodes[0];
        Node node2 = nodes[1];
        return Vector3.Distance(node1.Position,node2.Position);
    }

    float CalculateRotation()
    {
        Node node1 = nodes[0];
        Node node2 = nodes[1];
        float dX = node1.Position.x - node2.Position.x;
        float dY = node1.Position.y - node2.Position.y;
        float tan = dY / dX;
        return Mathf.Atan(tan) * Mathf.Rad2Deg;
    }

    Vector3 CalculatePosition()
    {
        Node node1 = nodes[0];
        Node node2 = nodes[1];
        
        return (node1.Position + node2.Position) / 2;
    }

    /// <summary>
    /// Line的构造函数，nodes为线的两个端点构成的数组，储存两个端点的(Node)
    /// </summary>
    /// <param name="nodes"></param>
    public void Init(List<Node> nodes)
    {
        ChangeState(LineState.ready);
        IsUse = false;
        if (nodes.Count >= 2)
        {
            this.nodes.Add(nodes[0]);
            this.nodes.Add(nodes[1]);
            this.nodes.Sort();
            length = CalculateLength();
            rotation = CalculateRotation();
            position = CalculatePosition();
        }
        Transform trans=gameObject.GetComponent<Transform>();
        trans.position = new Vector3(position.x, position.y, 0);
        trans.localScale = new Vector3(length / 8.0f, length / 8.0f, 1);
        trans.rotation= Quaternion.AngleAxis(rotation, Vector3.forward);
    }

    //public int CompareTo(object obj)
    //{
    //    Line line = (Line)obj;
    //    if (line.rotation > rotation)
    //        return -1;
    //    else
    //        return 1;
    //}

    public virtual void ChangeState(LineState state)
    {
        linestate = state;
        switch(state)
        {
            case LineState.isChoose: gameObject.GetComponent<SpriteRenderer>().color = Color.red; break;
            case LineState.ready: gameObject.GetComponent<SpriteRenderer>().color = Color.white; break;
            case LineState.show: gameObject.GetComponent<SpriteRenderer>().color = Color.black;gameObject.tag = "Line"; break;
        }
    }
}

public enum LineState
{
    isChoose,
    ready,
    show
}