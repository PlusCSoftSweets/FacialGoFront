using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGold : MonoBehaviour
{
    float CreatTime = 1f;
    GameObject gold;


    void Update()
    {
        CreatTime -= Time.deltaTime;    //开始倒计时
        if (CreatTime <= 0)    //如果倒计时为0 的时候
        {
            CreatTime = Random.Range(1, 1.5f);     //随机3到9秒内
            GameObject obj = (GameObject)Resources.Load("Prefabs/Gold");    //加载预制体到内存
            gold = Instantiate<GameObject>(obj);    //实例化金币
            gold.transform.position = new Vector3(Random.Range(-6.0f, 5.5f), 0f, Random.Range(20f, 220f));
        }
    }
}
