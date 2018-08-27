using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 章节内关卡的循环，最后一个关卡时加载下个章节
/// </summary>
public class LevelManager : MonoBehaviour {

    private static LevelManager instance = null;
    public static LevelManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(LevelManager)) as LevelManager;
            }
            if (instance == null)
                Debug.Log("Can't find Map");
            return instance;
        }
    }

    public int numOfLevel;
    public GameObject bg;
    [SerializeField]
    private int level;
    public GameObject prefab;

	// Use this for initialization
	void Start () {

        if(SceneLoadManager.aimLevel!=1)
        {
            level = SceneLoadManager.aimLevel-1;
            bg.transform.position -= new Vector3(10*level, 0, 0);
            LoadNewLevel();
            SceneLoadManager.aimLevel = 1;
            return;
        }

        level = 0;
        LoadNewLevel();
	}
	
	// Update is called once per frame
	void Update () {
		
	}  

    public void LoadNewLevel()
    {
        if (level >= numOfLevel)
        {
            SceneLoadManager.LoadNextScene();
            return;
        }

        if (level!=0&&prefab!=null)
        {
            DestroyImmediate(prefab);
            bg.GetComponent<BgMove>().MoveLeft();
        }
       
        level++;
        StartCoroutine(Load()); 
    }

    public void ReStart()
    {
        DestroyImmediate(prefab);

        prefab = Resources.Load<GameObject>("C"+SceneLoadManager.currentChapter.ToString()+"L"+level.ToString());
        prefab = Instantiate(prefab, gameObject.transform);
        prefab.transform.position = Vector3.zero;
    }

    //TODO增加显示效果
    IEnumerator Load()
    {
        yield return new WaitForSeconds(1f);  
        prefab = Resources.Load<GameObject>("C"+SceneLoadManager.currentChapter.ToString()+"L"+level.ToString());
        if (prefab != null)
        {
            prefab = Instantiate(prefab);
            prefab.transform.position = Vector3.zero;
        }
    }

    public void LoadLevelAt(int i)
    {
        level = i - 1;
        bg.transform.position = new Vector3(bg.transform.position.x - 10f * level, bg.transform.position.y, bg.transform.position.z);
        LoadNewLevel();
    }
}
