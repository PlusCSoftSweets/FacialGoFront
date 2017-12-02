using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMagnet : MonoBehaviour {
    float CreatTime = 2f;
    GameObject magnet;
    public GameObject haha;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CreatTime -= Time.deltaTime;    //开始倒计时
        if (CreatTime <= 0)    //如果倒计时为0 的时候
        {
            CreatTime = Random.Range(5f, 7f);    
            GameObject obj = (GameObject)Resources.Load("Prefabs/Magnet");    //加载预制体到内存
            magnet = Instantiate<GameObject>(obj);    //实例化金币
            magnet.transform.position = new Vector3(Random.Range(-6.0f, 5.5f), 0f, 
                Random.Range(-200, 150f));
        }

    }
}
