using Destructible2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class E_GameManager : GameManager {

    [SerializeField]
    [Header("每次随机生成几条边，要设定，可修改")]
    private int randomIndex;

    [SerializeField]
    [Header("第一次随机生成几条边，要设定，可修改")]
    private int firstIndex;

    [SerializeField]
    [Header("每步增加的可点击次数，要设定，可修改")]
    private int addClick;

    [SerializeField]
    private TextMeshProUGUI text;

    void Awake()
    {
        Init();

        AddRandomLine(firstIndex);

        NextStep();
    }

    void ChangeRandomIndex()
    {

    }

    void ChangeNode()
    {
        foreach(Node n in Map.Instance.nodes.Nodes)
        {
            if (n.BNearNodeHaveLine())
            {

                for(var i = 0; i < n.transform.childCount; i++)
                {
                    n.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
            else
            {
                for (var i = 0; i < n.transform.childCount; i++)
                {
                    n.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }

    public new void NextStep()
    {
        ChangeNode();
        _step++;
        m_recordSystem.SetEndless(_step);
        gameObject.GetComponent<Click>().ChangeClickStep(addClick);
        base.NextStep();
        if(!bDefeat)
            AddRandomLine(randomIndex);
    }

    private void Update()
    {
        ChangeBgState();
    }

    private void AddRandomLine(int randomIndex)
    {
        int addIndex = 0;

        //var _lineIndex = 0;
        ////能不能加下这么多边TODO
        //foreach(var n in Map.Instance.nodes.Nodes)
        //{
        //    _lineIndex += n.FreeNode.Count;
        //}
        //if(_lineIndex < randomIndex * 2)
        //{
        //    Debug.Log("加的点太多了");
        //    return;
        //}

        var _index = 0;
        while (addIndex < randomIndex)
        {
            _index++;
            if (_index > 10000) { break; }
            var oneNode = Map.Instance.nodes.Contains(Random.Range(0, Map.Instance.nodes.Length));
            //这个点是否为空
            if (oneNode.LineCount() + oneNode.TempleLineIndex == 0 && _step != 0) continue;
            //如果是第一步
            else if (_step == 0 && oneNode.TempleLineIndex < oneNode.NearNode.Count)
            {
                var anotherNode = oneNode.FreeNode[Random.Range(0, oneNode.FreeNode.Count)];
                var nodes = new List<Node> { oneNode, anotherNode };
                if (!IsCircleLine(nodes))
                {
                    LoadLine(nodes);
                    addIndex++;
                }
                continue;
            }

            if (oneNode.LineCount() + oneNode.TempleLineIndex < oneNode.NearNode.Count)
            {
                var anotherNode = oneNode.FreeNode[Random.Range(0, oneNode.FreeNode.Count)];
                var nodes = new List<Node> { oneNode, anotherNode };
                if (!IsCircleLine(nodes))
                {
                    LoadLine(nodes);
                    addIndex++;
                }
            }
        }
    }

    public override void Fail()
    {
        ShowData(LoadData());
        bDefeat = true;
        SaveData();
        overPanel.GetComponent<ChoosePanel>().E_DisableButton();
        StartCoroutine(delayFail());
    }

    public override void Victory()
    {

    }

    //TODO 与UI对接
    public override void ShowData(RecordSaveData data)
    {
        //当前的步数
        var currentStep = _step;
        //记录步数
        var recordStep = data.EndlessStep;
        //是否为新纪录
        //var bHighScore = currentStep >= recordStep ? true : false;
        //Debug.Log("当前步数：" + currentStep + "记录是：" + recordStep + "是否为新纪录:" + bHighScore);
        text.text = "当前步数：" + currentStep + "\n" + "历史纪录：" + recordStep;
    }

    protected override void SaveData()
    {
        RecordSaveData saveData = m_recordSystem.CreatSaveEndlessData();
        saveData.SaveEndless();
    }

    protected override RecordSaveData LoadData()
    {
        RecordSaveData oldData = new RecordSaveData();
        oldData.LoadEndless();
        m_recordSystem.SetSaveData(oldData);
        return oldData;
    }

    IEnumerator delayFail()
    {
        yield return new WaitForSeconds(1f);
        overPanel.GetComponent<ChoosePanel>().EndStop();
    }

}
