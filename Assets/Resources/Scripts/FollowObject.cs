using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour {

    float Dir;                 //摄像机于要跟随物体的距离
    public GameObject obj;  //要跟随的物体

    // Use this for initialization
    void Start()
    {
        //获取到摄像机于要跟随物体之间的距离
        Dir = obj.transform.position.z - transform.position.z;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //摄像机的位置
        transform.position = new Vector3(transform.position.x, transform.position.y, obj.transform.position.z - Dir);
    }
}
