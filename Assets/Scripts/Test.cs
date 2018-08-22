using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    [SerializeField]
    private int step;
    [SerializeField]
    private List<Line> addLine;
    private AddLineList addLineList;
    private bool bDefeat = false;

	// Use this for initialization
	void Awake () {
        Map.Instance.InitMap_Node();
        step = 0;
        addLineList = GetComponent<AddLineList>();
        AddReadyLine(step);
	}

    public void NextStep()
    {
        step++;
        foreach (Line l in addLine)
        {
            l.ChangeState(LineState.show);
        }
        foreach(Line l in addLine)
        {
            var circle = LineManager.FindCircleLine(l);
            if (circle.Count != 0)
            {
                Fail();
            }
        }
        addLine = new List<Line>();
        if (step < addLineList.eachLine_node.Length)
        {           
            AddReadyLine(step);
        }
        if(step == addLineList.eachLine_node.Length && !bDefeat)
            Victory();
    }

    void AddReadyLine(int _step)
    {
        LineList linelist = addLineList.eachLine_node[_step];
        int numberOfLines = linelist.Array.Length / 2;

        for (int i = 0; i < numberOfLines; i++)
        {
            var nodes = new List<Node>
            {
                linelist.Array[i * 2].GetComponent<Node>(),
                linelist.Array[i * 2 + 1].GetComponent<Node>()
            };

            //TODO 设置父物体,修改随机生成的线
            GameObject line;
            if(Random.Range(0f,2f)>2f)
                line = Resources.Load<GameObject>("StaticLine");
            else
                line = Resources.Load<GameObject>("Line");
            line = Instantiate(line,gameObject.transform);
            line.GetComponent<Line>().Init(nodes);
            addLine.Add(line.GetComponent<Line>());
        }
        Map.Instance.AddLine(addLine);
    }

    public void Victory()
    {
        //Debug.Log("Victory");
        LevelManager.Instance.LoadNewLevel();
    }

    public void Fail()
    {
        bDefeat = true;
        Debug.Log("Defeat");
    }
}
