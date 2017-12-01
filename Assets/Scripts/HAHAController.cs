﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HAHAController : MonoBehaviour
{
    private bool isUp;                        // 判断正在向上
    private bool isMove;                      // 判断正在左右移动
    private Rigidbody hahaRB;                 // haha实体
    private Vector3 lastPosition;             // 记录上一个位置
    private Vector3 moveHorizontalVec;        // 横向移动的速度
    public static HAHAController playerHaha;  // 玩家实例
    public Text  goldNum;
    private int count;

    static public HAHAController getHaHaInstance()
    {
        return playerHaha;
    }

    /*
     * 初始化
     */
    void Start()
    {
        isUp              = false;
        isMove            = false;
        hahaRB            = GetComponent<Rigidbody>();
        lastPosition      = transform.position;
        moveHorizontalVec = new Vector3(0, 0, 0);
        playerHaha        = this;
        count = 0;
        SetCountText();
    }

    void moveForward()
    {
        Vector3 LocalPos = transform.position;                                  // 物体所处的世界坐标向量
        Vector3 LocalForward = transform.TransformPoint(Vector3.forward * 5f);    // 物体前方距离为speed的位置的世界坐标向量
        Vector3 VecSpeed = LocalForward - LocalPos;                             // 物体自身Vector3.forward * speed的世界坐标向量
        moveHorizontalVec = new Vector3(moveHorizontalVec.x, 0, VecSpeed.z);
        hahaRB.velocity = moveHorizontalVec;
    }

    /*
     * 更新函数
     */
    void FixedUpdate()
    {
        moveForward();
        // 左右判断函数
        if ((System.Math.Abs(hahaRB.position.x - 1.6f) < 0.15f ||
            System.Math.Abs(hahaRB.position.x + 4.5f) < 0.15f ||
            System.Math.Abs(hahaRB.position.x - 4.5f) < 0.15f ||
            System.Math.Abs(hahaRB.position.x + 1.6f) < 0.15f) &&
            System.Math.Abs(hahaRB.position.x - lastPosition.x) > 2 && isMove)
        {
            moveHorizontalVec -= moveHorizontalVec;
            isMove = false;
        }

        // 上下判断函数
        //if (isUp)
        //{
        //    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //    hahaRB.velocity = new Vector3(hahaRB.velocity.x, 0, hahaRB.velocity.z);
        //    hahaRB.useGravity = false;
        //}
    }

    /*
     * 移动函数
     */
    public void Move(string direction)
    {
        Debug.Log(direction + transform.position.x + hahaRB.position.x + isMove);
        if (direction.Equals("Left"))
        {
            if (transform.position.x > -4.0f && !isMove)
            {
                isMove = true;
                lastPosition = transform.position;                                      // 更新我当前位置的坐标
                Vector3 LocalPos = transform.position;                                  // 物体所处的世界坐标向量
                Vector3 LocalForward = transform.TransformPoint(Vector3.left * 7f);     // 物体前方距离为speed的位置的世界坐标向量
                Vector3 VecSpeed = LocalForward - LocalPos;                             // 物体自身Vector3.forward * speed的世界坐标向量
                moveHorizontalVec = new Vector3(VecSpeed.x, VecSpeed.y, VecSpeed.z);
            }
        }
        else if (direction.Equals("Right"))
        {
            if (transform.position.x < 4.0f && !isMove)
            {
                Debug.Log("Right");
                isMove = true;
                lastPosition = transform.position;                                      // 更新我当前位置的坐标
                Vector3 LocalPos = transform.position;                                  // 物体所处的世界坐标向量
                Vector3 LocalForward = transform.TransformPoint(Vector3.right * 7f);    // 物体前方距离为speed的位置的世界坐标向量
                Vector3 VecSpeed = LocalForward - LocalPos;                             // 物体自身Vector3.forward * speed的世界坐标向量
                moveHorizontalVec = new Vector3(VecSpeed.x, VecSpeed.y, VecSpeed.z);
            }
        }
        //else if (direction.Equals("Up"))
        //{
        //    if (transform.position.y == 0 && !isUp && !isMove)
        //    {
        //        isUp = true;
        //    }
        //}
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gold"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }

    void SetCountText()
    {
        goldNum.text = count.ToString();
    }
}