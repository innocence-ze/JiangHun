using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    [SerializeField]
    private int step;
    [SerializeField]
    private List<Line> addLine;
    private AddLineList addLineList;

	// Use this for initialization
	void Awake () {
        Map.Instance.InitMap();
        step = 0;
        addLineList = GetComponent<AddLineList>();
        Step(0);

        foreach (Line l in addLine)
            l.ChangeState(Linestate.show);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void NextStep()
    {
        foreach (Line l in addLine)
            l.ChangeState(Linestate.show);
        addLine = new List<Line>();
        if (step < addLineList.eachLine_node.Length)
        {
            step++;
            Step(step);
        }  

       Victory();
    }

    public void Step(int index)
    {
        LineList linelist = addLineList.eachLine_node[index];
        int numberOfLines = linelist.Array.Length / 2;

        for (int i = 0; i < numberOfLines; i++)
        {
            List<Node> nodes = new List<Node>();
            nodes.Add(linelist.Array[i * 2].GetComponent<Node>());
            nodes.Add(linelist.Array[i * 2 + 1].GetComponent<Node>());
            GameObject line = GameObject.Instantiate(Resources.Load("Line") as GameObject);
            line.GetComponent<Line>().Init(nodes);

            addLine.Add(line.GetComponent<Line>());
        }
        Map.Instance.AddLine(addLine);

        foreach(Line l in addLine)
        {
            if (LineManager.FindCircleLine(l) != null)
                Fail();
        }
    }

    public void Victory()
    {
        Debug.Log("This is victory judgement");
    }
    public void Fail()
    {
        Debug.Log("Circle exists!");
    }
}
