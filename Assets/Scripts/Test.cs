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
        Map.Instance.InitMap_Node();
        step = 0;
        addLineList = GetComponent<AddLineList>();
        Step(0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void NextStep()
    {
        //foreach (Line l in addLine)
            //l.ChangeState(LineState.show);
        foreach (Line l in addLine)
        {
            l.ChangeState(LineState.show);
            var circle = LineManager.FindCircleLine(l);
            if (circle.Count != 0)
            {
                Fail();
            }
        }
        addLine = new List<Line>();
        if (step < addLineList.eachLine_node.Length - 1)
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
            var nodes = new List<Node>
            {
                linelist.Array[i * 2].GetComponent<Node>(),
                linelist.Array[i * 2 + 1].GetComponent<Node>()
            };

            //TODO 设置父物体
            GameObject line = Resources.Load<GameObject>("Line");
            line = Instantiate(line);
            line.GetComponent<Line>().Init(nodes);
            addLine.Add(line.GetComponent<Line>());
        }
        Map.Instance.AddLine(addLine);
    }

    public void Victory()
    {
        Debug.Log("Victory");
    }
    public void Fail()
    {
        Debug.Log("Defeat");
    }
}
