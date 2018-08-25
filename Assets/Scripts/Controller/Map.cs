﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 关卡内地图的相关信息初始化与边的增删
/// </summary>
public class Map : MonoBehaviour {

    private static Map s_Instance = null;
    public static Map Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(Map)) as Map;
            }
            if (s_Instance == null)
                Debug.Log("Can't find Map");
            return s_Instance;
        }
    }

    //图中起作用的点
    public NodeQueue nodes;
    private List<StaticLine> staticLines = new List<StaticLine>();

    public void AddStaticLine(StaticLine staticLine)
    {
        staticLines.Add(staticLine);
    }

    public List<StaticLine> StaticLines { get { return staticLines; } }

    public void InitMap_Node()
    {        
        nodes = new NodeQueue();
        var nodeList = GameObject.FindGameObjectsWithTag("Node");
        foreach (GameObject n in nodeList)
        {
            var node = n.GetComponent<Node>();
            node.Init(n.transform.position);
            if (!nodes.Contains(node))
            {
                nodes.Add(node);
                node.TempleLine.Clear();
                node.TempleLineIndex = 0;
            }
        }
    }

    public void InitMap_Line()
    {
        foreach (var n in nodes.Nodes)
        {
            foreach (var l in n.LineList)
            {
                if(l.gameObject.tag == "Line")
                {
                    l.ChangeState(LineState.show);
                }
                l.IsUse = false;
            }
        }
    }

    /// <summary>
    /// 给点加上边，然后找环
    /// </summary>
    /// <param name="addLine"></param>
    public void AddLine(List<Line> addLine)
    {
        if (addLine == null) return;
        foreach(Line l in addLine)
        {
            foreach (Node n in l.Nodes)
            {
                n.AddLine(l);
            }
        }
    }

    /// <summary>
    /// 把边从点上删除
    /// </summary>
    /// <param name="removeLine"></param>
    public void RemoveLine(List<Line> removeLine)
    {
        if (removeLine == null) return;
        foreach(Line l in removeLine)
        {
            foreach (Node n in l.Nodes)
                n.RemoveLine(l);
        }
    }
}
