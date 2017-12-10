using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceletatorClick : MonoBehaviour {

    GameObject accelerator;
    public GameObject haha;
    public float forwardSpeed = 30f;           // 前进速度
    public float targetSpeed = 200f;//加速器能够到达的最大速度
    float CreatTime = 15f;
    private bool isSpeed = false;
    bool isClick = false;

    // Use this for initialization
    void Start () {
	}
	
	
	void FixedUpdate () {
        if (isClick)
        {
            CreatTime = 15f;
            GameObject obj = (GameObject)Resources.Load("Prefabs/Accelerator");    //加载预制体到内存
            accelerator = Instantiate<GameObject>(obj);    //实例化加速器
            accelerator.transform.position = new Vector3(0, 10f, haha.transform.position.z);//动画效果
            haha.GetComponent<HAHAController>().forwardSpeed = targetSpeed;
            isSpeed = true;
            isClick = false;
        }
        if (isSpeed)
        {
            CreatTime -= Time.deltaTime;//加速5秒
            if (CreatTime <= 0)
            {
                haha.GetComponent<HAHAController>().forwardSpeed = forwardSpeed;
                isSpeed = false;
            }
        }
    }
     public void ButtonClick() {
        isClick = true;
     }

}
