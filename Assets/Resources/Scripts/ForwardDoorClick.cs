using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForwardDoorClick : MonoBehaviour {

    public GameObject haha;
    public Text moveText;
    bool isClick = false;
    float CreatTime = 3f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
        // 获取所有的树和相机
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        GameObject camera = GameObject.FindGameObjectWithTag("BackGroundCamera");

        //前进或者后退300以内
        float dis = Random.Range(0f, 300f);
        Vector3 target = new Vector3(0, 0, dis);
        haha.transform.position += target;
        moveText.text = "你前进了" + (int)dis + "米！";
        isClick = true;
        foreach (GameObject tree in trees)
            tree.transform.position += target;
    }
}
