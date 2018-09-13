using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

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
    [SerializeField]
    public GameObject canvas;

    private Transform trans;

    public int Level { get { return level; } }

	// Use this for initialization
	void Start () {

        level = 1;
        prefab = Resources.Load<GameObject>("C" + SceneLoadManager.currentChapter.ToString() + "L" + level.ToString());
        prefab = Instantiate(prefab, gameObject.transform);
        prefab.transform.position = Vector3.zero;
        prefab.GetComponentInChildren<Canvas>().enabled = false;
        prefab.GetComponentInChildren<Click>().enabled = false;

        canvas.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
	}  

    public void LoadNewLevel()
    {
        if (level >= numOfLevel)
        {
            Camera.main.orthographicSize = 5;
            Camera.main.GetComponent<GameWinMove>().MoveWin();
            StartCoroutine(Hide());
            StartCoroutine(WinAnim());
            return;
        }

        if (level!=0&&prefab!=null)
        {
            DestroyImmediate(prefab);
            if (SceneLoadManager.currentChapter >= 4)
                StartCoroutine(ResetCamera());
            else
                bg.transform.DOLocalMove(new Vector3(bg.transform.position.x - 10f, 0, 0), 1f);
        }
       
        level++;
        StartCoroutine(Load()); 
    }

    public void ReStart()
    {
        bg = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().Find_HealBG();
        DestroyImmediate(prefab);

        prefab = Resources.Load<GameObject>("C"+SceneLoadManager.currentChapter.ToString()+"L"+level.ToString());
        prefab = Instantiate(prefab, gameObject.transform);
        prefab.transform.position = Vector3.zero;
    }

    //TODO增加显示效果
    IEnumerator Load()
    {
        prefab = Resources.Load<GameObject>("C"+SceneLoadManager.currentChapter.ToString()+"L"+level.ToString());
        yield return new WaitForSeconds(1f);         
        if (prefab != null)
        {
            prefab = Instantiate(prefab);
            prefab.transform.position = Vector3.zero;
        }
    }

    public void check()
    {
        prefab.GetComponentInChildren<Canvas>().enabled = true;
        canvas.SetActive(false);
        prefab.GetComponentInChildren<Click>().enabled = true;
        Camera.main.DOOrthoSize(5f,1f);
    }


    public void PreviewNextLevel()
    {
        if (level >= numOfLevel)
            return;

        DisActivate();
        StartCoroutine(Activate());

        if (prefab != null)
        {
            //prefab.transform.DOLocalMove(new Vector3(-10, 0, 0), 1f);
            //Destroy(prefab, 1f);
            DestroyImmediate(prefab);
            level++;
        }

        prefab = Resources.Load<GameObject>("C" + SceneLoadManager.currentChapter.ToString() + "L" + level.ToString());
        if (prefab != null)
        {
            prefab = Instantiate(prefab,gameObject.transform);
            prefab.GetComponentInChildren<Canvas>().enabled = false;
            prefab.GetComponentInChildren<Click>().enabled = false;
            prefab.transform.position = new Vector3(10f, 0, 0);
            prefab.transform.DOLocalMove(Vector3.zero, 1f);
            bg.transform.DOLocalMove(new Vector3(bg.transform.position.x-10f, 0, 0), 1f);
        }
    }

    public void PreviewLastLevel()
    {
        if (level <= 1)
            return;

        DisActivate();
        StartCoroutine(Activate());

        if (prefab != null)
        {
            //prefab.transform.DOLocalMove(new Vector3(10, 0, 0), 1f);
            //Destroy(prefab, 1f);
            DestroyImmediate(prefab);
            level--;
        }

        prefab = Resources.Load<GameObject>("C" + SceneLoadManager.currentChapter.ToString() + "L" + level.ToString());
        if (prefab != null)
        {
            prefab = Instantiate(prefab);
            prefab.GetComponentInChildren<Canvas>().enabled = false;
            prefab.GetComponentInChildren<Click>().enabled = false;
            prefab.transform.position = new Vector3(-10f, 0, 0);
            prefab.transform.DOLocalMove(Vector3.zero, 1f);
            bg.transform.DOLocalMove(new Vector3(bg.transform.position.x+10f, 0, 0), 1f);
        }
    }

    private void DisActivate()
    {
        Button[] buttons = canvas.GetComponentsInChildren<Button>();
        foreach (Button b in buttons)
        {
            b.enabled = false;
        }
    }

    IEnumerator Activate()
    {
        yield return new WaitForSeconds(1f);
        Button[] buttons = canvas.GetComponentsInChildren<Button>();
        foreach(Button b in buttons)
        {
            b.enabled = true;
        }
    }

    IEnumerator WinAnim()
    {
        yield return new WaitForSeconds(14f);
        SceneLoadManager.aimChoose = 3;
        SceneLoadManager.LoadScene(0);
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(12.5f);
        HideAnim();
    }

    public void HideAnim()
    {
        canvas.SetActive(true);
        Image[] buttons = canvas.GetComponentsInChildren<Image>();
        Animator shade = canvas.GetComponentInChildren<Animator>();
        foreach (Image b in buttons)
            b.gameObject.SetActive(false);
        shade.gameObject.SetActive(true);
        shade.Play(Animator.StringToHash("Shade"));
    }

    IEnumerator ResetCamera()
    {
        Camera.main.transform.DOLocalMove(Vector3.zero, 1f);
        Camera.main.DOOrthoSize(5f, 1f);
        yield return new WaitForSeconds(1f);
        bg.transform.DOLocalMove(new Vector3(bg.transform.position.x - 10f, 0, 0), 1f);
    }
 }
