using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private GameObject prefab;

	// Use this for initialization
	void Start () {
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
            Debug.Log("need to load new scene!");
            SceneLoadManager.Instance.LoadNextScene();
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

        prefab = Resources.Load<GameObject>("Level" + level.ToString());
        prefab = Instantiate(prefab, gameObject.transform);
        prefab.transform.position = Vector3.zero;
    }

    //to do增加显示效果
    IEnumerator Load()
    {
        yield return new WaitForSeconds(1f);  
        prefab = Resources.Load<GameObject>("Level" + level.ToString());
        prefab = Instantiate(prefab);
        prefab.transform.position = Vector3.zero;
    }
}
