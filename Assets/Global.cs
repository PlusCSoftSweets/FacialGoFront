using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour {

    public static Global instance;
    public int coinNumber = 0;
    public int currentScene = 0;

    static Global() {
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
    }
}
