using UnityEngine;

public class Map : MonoBehaviour {

    private static Map s_Instance = null;
    public static Map Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(Map)) as Map;
            }
            if (s_Instance == null)
                Debug.Log("Can't find Map");
            return s_Instance;
        }
    }

    public static NodeQueue nodes;

    private GameObject[] nodeList;

	// Use this for initialization
	void Awake () {
        nodeList = GameObject.FindGameObjectsWithTag("Node");
        foreach(GameObject n in nodeList)
        {
            Node node = new Node(n.transform.position);
            if (!nodes.Contains(node))
            {
                nodes.Add(node);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
