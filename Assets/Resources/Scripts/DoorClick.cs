using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorClick : MonoBehaviour {

    public GameObject haha;
    public Text moveText;
    bool isClick = false;
    float CreatTime = 3f;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (isClick)
        {
            CreatTime -= Time.deltaTime;
            if (CreatTime <= 0)
            {
                moveText.text = "";
                isClick = false;
                CreatTime = 3f;
            }
        }
    }
    public void ButtonClick()
    {
        float dis = Random.Range(-300f, 300f);//前进或者后退300以内
        Vector3 target = Vector3.zero;
        if (haha.transform.position.z + dis < -200)
        {
            target = new Vector3(haha.transform.position.x, haha.transform.position.y, -200);
            dis = -200f - haha.transform.position.z;
        }
        else
        {
            target = new Vector3(haha.transform.position.x, haha.transform.position.y, haha.transform.position.z + dis);
        }
        haha.transform.position = target;
        if (dis > 0){
            moveText.text = "你前进了" + (int)dis + "米！";
        }
        else{
            moveText.text = "你后退了" + (int)(-dis) + "米！";
        }
        isClick = true;
    }
}
