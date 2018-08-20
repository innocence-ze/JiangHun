using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public static NodeQueue nodes;
    //点图中的所有点
    private GameObject[] nodeList;

    public void InitMap()
    {
        nodes = new NodeQueue();
        nodeList = GameObject.FindGameObjectsWithTag("Node");
        foreach (GameObject n in nodeList)
        {
            Node node = n.GetComponent<Node>();
            node.Init(n.GetComponent<Transform>().position);
            if (!nodes.Contains(node))
            {
                nodes.Add(node);
            }
        }
    }

    public void AddLine(List<Line> addLine)
    {
        if (addLine == null) return;
        foreach(Line l in addLine)
        {
            foreach (Node n in l.Nodes)
                n.AddLine(l);
            LineManager.FindCircleLine(l);
        }
    }

    public void RemoveLine(ArrayList removeLine)
    {
        if (removeLine == null) return;
        foreach(Line l in removeLine)
        {
            foreach (Node n in l.Nodes)
                n.RemoveLine(l);
        }
    }
}
