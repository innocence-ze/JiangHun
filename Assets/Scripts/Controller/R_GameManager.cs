using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Destructible2D;

/// <summary>
/// 关卡内游戏主脚本，主逻辑
/// </summary>
public class R_GameManager : GameManager {

    private int eachLeftScore = 800;

    [SerializeField]
    private int chapter;
    [SerializeField]
    private int level;

    [SerializeField]
    [Header("储存随机步数，要设定")]
    private int r_step;
    [SerializeField]
    [Header("储存固定步数，不要设定")]
    private int f_step;

    [SerializeField]
    [Header("每次随机生成几条边，要设定")]
    private int randomIndex;

    private AddLineList addLineList;

    [SerializeField]
    private GameObject passPanel;


    public int StepsLeft
    {
        get
        {
            return r_step + f_step - _step;
        }
    }

    // Use this for initialization
    void Awake()
    {
        Init();

        chapter = SceneLoadManager.currentChapter;
        level = LevelManager.Instance.Level;      
        
        addLineList = GetComponent<AddLineList>();
        f_step = addLineList.eachLine_node.Length;
        AddFixedLine(_step);

        
        NextStep();
    }

    private void Update()
    {
        ChangeBgState(level);
    }

    public new void NextStep()
    {
        _step++;
        base.NextStep();
        //加ready的线
        if (!BDefeat)
        {
            if (_step < f_step)
            {
                AddFixedLine(_step);
            }
            else if (_step < f_step + r_step)
            {
                AddRandomLine(randomIndex);
            }
            //所有步数完成通关
            if (_step == f_step + r_step && !bDefeat)
                Victory();
        }
    }

    private void AddFixedLine(int _step)
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

            LoadLine(nodes);
        }
    }

    private void AddRandomLine(int randomIndex)
    {
        int addIndex = 0;
        var _lineIndex = 0;

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

        //是否还有边
        _lineIndex = 0;
        foreach (var n in Map.Instance.nodes.Nodes)
        {
            _lineIndex += n.LineCount();
        }
        var _index = 0;
        while (addIndex < randomIndex)
        {
            _index++;
            if (_index > 10000) { break; }               
            var oneNode = Map.Instance.nodes.Contains(Random.Range(0, Map.Instance.nodes.Length));
            //是否存在边
            if (oneNode.LineCount() + oneNode.TempleLineIndex == 0 && _lineIndex + addIndex != 0) continue;
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

    public override void Victory()
    {
        Score += gameObject.GetComponent<Click>().ClickScore + gameObject.GetComponent<Click>().ClickSteps * eachLeftScore;
        m_recordSystem.SetCurrentCL(chapter, level, Score);
        SaveData();
        //ShowData(LoadData());
        //Debug.Log("Victory");
        //LevelManager.Instance.LoadNewLevel();
        //passPanel.GetComponent<ChoosePanel>().Stop();
        StartCoroutine(DelayStop(passPanel));
    }

    public override void Fail()
    {
        ShowData(LoadData());
        bDefeat = true;
        overPanel.GetComponent<ChoosePanel>().R_DisableButton();
        StartCoroutine(DelayStop(overPanel));
    }


    //TODO
    public override void ShowData(RecordSaveData data)
    {
        var currentScore = Score;
        var highScore = data.CurrentHighScore;
        //text.text =" 当前得分：" + currentScore +"\n"+ "  最高得分：" + highScore;
        //Debug.Log("第"+chapter+"章，第"+level+"关, 当前得分：" + currentScore +"  最高得分："+ highScore);
    }

    protected override void SaveData()
    {
        
        RecordSaveData saveData = m_recordSystem.CreatSaveCurrentCLData();
        saveData.SaveCurrentCL();
    }

    protected override RecordSaveData LoadData()
    {
        RecordSaveData oldData = new RecordSaveData();
        oldData.LoadCurrentCL();
        m_recordSystem.SetSaveData(oldData);
        return oldData;
    }

    IEnumerator DelayStop(GameObject panel)
    {
        yield return new WaitForSeconds(2f);
        panel.GetComponent<ChoosePanel>().Stop();
    }
}
