using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    Vector3 Dir;                 //摄像机于要跟随物体的距离
    public GameObject m_Player;  //要跟随的物体

    // Use this for initialization
    void Start()
    {
        // 获取玩家位置
        m_Player = GameObject.FindGameObjectWithTag("Player");
        //获取到摄像机于要跟随物体之间的距离
        Dir = m_Player.transform.position - transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //摄像机的位置
        transform.position = m_Player.transform.position - Dir;
    }
}