using UnityEngine;
using System.Collections;

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

    public static NodeQueue nodes;

    private GameObject[] nodeList;

    public void InitMap()
    {
        nodeList = GameObject.FindGameObjectsWithTag("Node");
        foreach (GameObject n in nodeList)
        {
            Node node = new Node(n.transform.position);
            if (!nodes.Contains(node))
            {
                nodes.Add(node);
            }
        }
    }

    public void UpdateMap(ArrayList addLine, ArrayList removeLine)
    {
        if(addLine != null)
        {
            foreach (Line l in addLine)
            {
                foreach (Node n in l.Nodes)
                {
                    n.AddLine(l);
                }
            }
            foreach (Line l in addLine)
                LineManager.FindCircleLine(l);
        }
        if (removeLine != null)
        {
            foreach (Line l in addLine)
            {
                foreach (Node n in l.Nodes)
                {
                    n.RemoveLine(l);
                }
            }
        }
    }
}
