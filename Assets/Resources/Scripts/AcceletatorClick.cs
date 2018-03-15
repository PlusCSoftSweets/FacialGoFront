using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AcceletatorClick : MonoBehaviour {

    GameObject accelerator;
    public GameObject haha;
    public float forwardSpeed = 30f;           // 前进速度
    public float targetSpeed = 200f;  //加速器能够到达的最大速度
    float CreatTime = 15f;
    private int isSpeed = 2;
    bool isClick = false;
    float DispearTime = 3f;


    // Loading Bar
    public GameObject LoadingBar;
    [SerializeField] private float currentAmount;
    [SerializeField] private float timeSpeed;
    AudioSource[] m_MyAudioSource = new AudioSource[1];

    // Use this for initialization
    void Start () {
        // Loading Bar Init
        timeSpeed = 1 / CreatTime;
        currentAmount = 1;
        m_MyAudioSource = GetComponents<AudioSource>();
    }
	
	
	void Update () {
        if (isClick)
        {
            GameObject obj = (GameObject)Resources.Load("Prefabs/Accelerator");    //加载预制体到内存
            accelerator = Instantiate<GameObject>(obj);    //实例化加速器
           
            Vector3 start = new Vector3(0, haha.transform.position.y, haha.transform.position.z);
            accelerator.transform.position = start;
            Rigidbody rb = accelerator.GetComponent<Rigidbody>();
            Vector3 temp = rb.velocity;
            temp.z = 200f;
            temp.y = 20f;
            rb.velocity = temp;
            DispearTime -= Time.deltaTime;
            Debug.Log("加速器");
            haha.GetComponent<HAHAController>().forwardSpeed = targetSpeed;
            isSpeed = 1;
            isClick = false;
        }
        if (isSpeed == 1)
        {
            // Loading Bar
            currentAmount -= timeSpeed * Time.deltaTime;
            LoadingBar.GetComponent<Image>().fillAmount = currentAmount;

            CreatTime -= Time.deltaTime;//加速时间
            if(CreatTime <= 12)
            {
                if(accelerator != null)
                {
                    Destroy(accelerator);
                }
            }
            if (CreatTime <= 0)
            {
                haha.GetComponent<HAHAController>().forwardSpeed = forwardSpeed;
                isSpeed = 0;
                CreatTime = 15f;
                currentAmount = 1;
                LoadingBar.GetComponent<Image>().fillAmount = 0;
            }
        }
    }

     public void ButtonClick() {
        if ((isSpeed == 0 || isSpeed == 2) && CreatTime == 15f) {
            isClick = true;
            m_MyAudioSource[0].Play();
        }
    }

}
