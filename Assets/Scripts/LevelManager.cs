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
            return;
        }

        if (level!=0&&prefab!=null)
        {
            DestroyImmediate(prefab);
        }
       
        level++;
        prefab = Resources.Load<GameObject>("Level" + level.ToString());
        prefab = Instantiate(prefab,gameObject.transform);
        prefab.transform.position = Vector3.zero;

        //prefab.transform.position = new Vector3((level - 1) * 20f, 0f ,0f);
        //Camera.main.transform.position = new Vector3((level - 1) * 20f, 0f, -10f);
    }
}
