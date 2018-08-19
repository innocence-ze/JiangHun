using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 点的队列，储存所有点的信息，点的管理类
/// 字段有点的数量，方法有对点进行增删查找
/// </summary>
public class NodeQueue  {

    static ArrayList nodes = new ArrayList();

    public static ArrayList  Nodes { get { return nodes; } }

    public int Length
    {
        get { return nodes.Count; }
    }

    public bool Contains(object node)
    {
        return nodes.Contains(node);
    }

    public void Add(Node node)
    {
        nodes.Add(node);
        nodes.Sort();
    }

    public void Remove(Node node)
    {
        nodes.Remove(node);
        nodes.Sort();
    }
}
