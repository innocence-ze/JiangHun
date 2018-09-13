using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 章节管理器
/// </summary>
public class SceneLoadManager : MonoBehaviour {

    //private static SceneLoadManager instance;
    //public static SceneLoadManager Instance
    //{
    //    get
    //    {
    //        if(instance==null)
    //        {
    //            instance = FindObjectOfType(typeof(SceneLoadManager)) as SceneLoadManager;
    //        }
    //        if (instance == null)
    //            Debug.Log("can't find SceneLoadManager");
    //        return instance;
    //    }
    //}

    public static int currentChapter = 0;

    /*用于选择关卡*/
    public static int aimLevel = 1;
    public static int aimChoose = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    static public void LoadScene(int i)
    {
        if (i == 1 && aimLevel == 1)
        {
            currentChapter = 7;
            SceneManager.LoadScene(7);
            return;
        }
        currentChapter = i;
        SceneManager.LoadScene(i);
    }

    public static void LoadNextScene()
    {
        currentChapter++;
        if (currentChapter > 5)
            currentChapter = 0;
        SceneManager.LoadScene(currentChapter);       
    }
}
