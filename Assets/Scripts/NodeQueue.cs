using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeQueue  {

    ArrayList nodes = new ArrayList();

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
