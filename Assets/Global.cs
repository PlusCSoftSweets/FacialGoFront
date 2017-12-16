using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour {

    public static Global instance;
    public int coinNumber = 0;
    public int currentScene = 0;



    public void DoSomeThings()
    {
        Debug.Log("DoSomeThings");
    }

    // Use this for initialization
    void Start()
    {
        instance = this;
        Debug.Log("Global Start");
    }
}
