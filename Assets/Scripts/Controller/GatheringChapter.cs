using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GatheringChapter : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI text;
    public GameObject BG;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ChangeBG(string BGname)
    {
        var bg = Resources.Load<Sprite>(BGname);
        BG.GetComponent<SpriteRenderer>().sprite = bg;
        if (BG.transform.childCount != 0)
        {
            int bgcc = BG.transform.childCount;
            for (int cc = 0; cc < bgcc; cc++)
            {
                DestroyImmediate(BG.transform.GetChild(0).gameObject);
            }
        }
        var canvas = Resources.Load<GameObject>(BGname + "canvas");
        canvas = Instantiate(canvas, BG.transform);
    }

    public void ChangeChapter(int i)
    {
        //TODO
        switch(i)
        {
            case 1: text.text = "莫高窟第257窟西壁\n\n\n" +
                    "九色鹿本生故事主要宣扬佛教“施恩行善必得好报，忘恩为恶必遭严惩”的观念，" +
                    "亦是莫高窟唯一以动物为主角的本生故事画。这个故事仅见于莫高窟北魏第257窟，" +
                    "是据三国时代吴国人支谦翻译的《佛说九色鹿经》绘制。\n\n" +
                    "故事讲述了九色鹿（释迦牟尼的前生）救起溺水之人，但溺人见利忘义，到宫廷告密，" +
                    "并带领国王前往猎取鹿角及皮毛，九色鹿毫无惧色，向国王控诉溺人忘恩负义的恶行。国" +
                    "王深受感动，下令全国禁捕九色鹿，而溺人则因违背誓言而全身生疮受苦。";
                ChangeBG("九色鹿");
                break;
            case 2: text.text = "莫高窟第254窟\n\n\n" +
                    "开凿于北魏时期（465 - 500年），保存较为完整，有较为完备的佛教图像系统，在艺术成" +
                    "就上也十分突出，同时也拥有较为丰厚的学术研究积淀。\n\n" +
                    "南壁的舍身饲虎图讲述了释迦牟尼佛前世为Mahasattva王子时，与两位兄长在" +
                    "山间游玩的途中，为了拯救一只因生产而疲惫饥饿以致濒死的母虎和幼虎们，慈悲而决" +
                    "绝地舍出自己肉身饲虎的故事。";
                ChangeBG("舍身饲虎");
                break;
            case 3: text.text = "莫高窟第103窟\n\n\n此窟开凿于盛唐，塑像为释迦牟尼佛、菩萨、弟子，塑像为清" +
                    "修及清塑。窟形为覆斗形顶，窟顶四坡绘千佛。此窟主要画有两品：妙庄严王本事品和化城喻品。" +
                    "其中化城喻品为法华经的主要内容之一。\n\n该品主要讲述了一个富商在一向导的指引下到宝城取宝，" +
                    "历经千辛万苦，跋山涉水，以坚忍不拔的精神冲破重重险阻到达目的地的故事。";
                ChangeBG("化城取宝");
                break;
            case 4: text.text = "本壁画讲述了民间对佛教的崇拜，展现了当时人们的日常生活";
                ChangeBG("日常");
                break;
        }
        TextMeshProUGUI[] children = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        foreach(TextMeshProUGUI t in children)
        {
            t.color = Color.black;
        }
    }
}
