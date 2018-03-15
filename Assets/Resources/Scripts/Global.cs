using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{

    public static Global instance;
    public int coinNumber = 0;
    public int currentScene = 0;
    public Vector3 playerPosition;
    public Vector3 mainCameraPosition;
    public Vector3 uiCameraPosition;
    public Vector3[] treesPosition;

    static Global()
    {
        GameObject go = new GameObject("Global");
        DontDestroyOnLoad(go);
        instance = go.AddComponent<Global>();
    }

    public void DoSomeThings()
    {
        Debug.Log("DoSomeThings");
    }

    // Use this for initialization
    void Start()
    {
        Debug.Log("Global Start");
        playerPosition = new Vector3(0, 0, 0);
        mainCameraPosition = new Vector3(0, 0, 0);
        uiCameraPosition = new Vector3(0, 0, 0);
        treesPosition = new Vector3[10];
    }
}
