using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSceneManagerController : MonoBehaviour {
    public GameObject DetailCanvas;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void onCloseButtonClick()
    {
        DetailCanvas.SetActive(false);
    }
    public void onItemClick()
    {
        DetailCanvas.SetActive(true);
    }
}
