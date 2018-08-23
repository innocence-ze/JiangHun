using UnityEngine;

/// <summary>
/// 生成固定位置的线的数组（二维）
/// </summary>
public class AddLineList : MonoBehaviour
{
    private static AddLineList s_Instance = null;

    public static AddLineList Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(AddLineList)) as AddLineList;
            }
            if (s_Instance == null)
                Debug.Log("Can't find Map");
            return s_Instance;
        }
    }

    /// <summary>
    /// 存放每批线的两个端点
    /// </summary>
    public LineList[] eachLine_node;

}

[System.Serializable]
public class LineList
{
    public GameObject[] Array;
    public GameObject this[int index]
    {
        get
        {
            return Array[index];
        }
    }

    public LineList()
    {
        Array = new GameObject[4];
    }

    public LineList(int index)
    {
        Array = new GameObject[index];
    }
}
