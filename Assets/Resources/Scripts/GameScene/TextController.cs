using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour {

    private float delateTime = 0;

    void Start () {
        this.gameObject.GetComponent<Text>().text = "";
	}
	
	void Update () {
		if (this.gameObject.GetComponent<Text>().text != "" && delateTime < 0.001) {
            delateTime = 3;
        }
        else if (delateTime < 0.001) {
            this.gameObject.GetComponent<Text>().text = "";
            delateTime = 0;
        }
        else if (this.gameObject.GetComponent<Text>().text != "") {
            delateTime -= Time.deltaTime;
        }
	}
}
