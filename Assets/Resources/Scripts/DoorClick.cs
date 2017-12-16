using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorClick : MonoBehaviour {

    public GameObject haha;
    public Text moveText;
    bool isClick = false;
    float CreatTime = 3f;
    AudioSource[] m_MyAudioSource = new AudioSource[1];

    // Use this for initialization
    void Start () {
        m_MyAudioSource = GetComponents<AudioSource>();
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
        // 获取所有的树和相机
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        GameObject camera = GameObject.FindGameObjectWithTag("BackGroundCamera");

        if (haha.transform.position.z + dis < -200)
        {
            // target = new Vector3(haha.transform.position.x, haha.transform.position.y, -200);
            // dis = -200f - haha.transform.position.z;
            dis = -200 - haha.transform.position.z;
            target = new Vector3(0, 0, dis);
        }
        else
        {
            // target = new Vector3(haha.transform.position.x, haha.transform.position.y, haha.transform.position.z + dis);
            target = new Vector3(0, 0, dis);
        }
        haha.transform.position += target;
        foreach (GameObject tree in trees) {
            tree.transform.position += target;
        }

        if (dis > 0){
            moveText.text = "你前进了" + (int)dis + "米！";
        }
        else{
            moveText.text = "你后退了" + (int)(-dis) + "米！";
        }
        isClick = true;
        m_MyAudioSource[0].Play();


    }
}
