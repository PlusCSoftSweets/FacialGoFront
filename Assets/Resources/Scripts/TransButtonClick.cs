using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransButtonClick : MonoBehaviour {

    private bool isClick = false;
    private float time = 0;
    GameObject rotator;
    public GameObject m_Player;  //要跟随的物体
    int rotatingSpeed = 15;
    AudioSource[] m_MyAudioSource = new AudioSource[1];

    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_MyAudioSource = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        if (time < 0.001 && isClick) {
            cancelReverse();
            isClick = false;
            Destroy(rotator);
        }
        else if (isClick)
            time -= Time.deltaTime;

	}

    public void buttonClick() {
        if (rotator == null)
        {
            HAHAController player = HAHAController.getHaHaInstance();
            player.isReverse = true;
            time = 10;
            isClick = true;
            m_MyAudioSource[0].Play();
            GameObject obj = (GameObject)Resources.Load("Prefabs/Rotator");    //加载预制体到内存
            rotator = Instantiate<GameObject>(obj);    //实例化加速器
        }
    }

    private void cancelReverse() {
        HAHAController player = HAHAController.getHaHaInstance();
        player.isReverse = false;
    }
    void LateUpdate()
    {
        if (rotator != null)
        {
            rotator.transform.position = new Vector3(m_Player.transform.position.x, 
                m_Player.transform.position.y + 1.8f, m_Player.transform.position.z);
            //rotator.transform.Rotate(Vector3.back* rotatingSpeed);
            rotator.transform.Rotate(0* Time.deltaTime, 0 * Time.deltaTime, -90 * Time.deltaTime * rotatingSpeed);
        }
    }
}
