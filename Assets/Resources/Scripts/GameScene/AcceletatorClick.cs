using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AcceletatorClick : MonoBehaviour {

    #region Public Variables
    public GameObject player;
    public float forwardSpeed = 30f;  // 前进速度
    public float targetSpeed = 200f;  //加速器能够到达的最大速度
    public GameObject LoadingBar;
    public Text showHint;
    #endregion

    #region Private Variables
    [SerializeField] private float currentAmount;
    [SerializeField] private float timeSpeed;
    private GameObject rocket;
    private float creatTime = 15f;
    private int isSpeed = 2;
    private bool isClick = false;
    private float DispearTime = 3f;
    private AudioSource[] m_MyAudioSource = new AudioSource[1];
    #endregion

    // Use this for initialization
    void Start () {
        // Loading Bar Init
        timeSpeed = 1 / creatTime;
        currentAmount = 1;

        m_MyAudioSource = GetComponents<AudioSource>();
    }
	
	void Update () {
        // 只获得LoaclPlayer
        if (player == null) player = HAHAController.GetHaHaInstance().gameObject;
        if (isClick)
        {
            LaunchOutRocket();
            isClick = false;
        }
        if (isSpeed == 1)
        {
            LoadingBarWorking();
        }
    }

    #region Public Methods
    public void ButtonClick() {
        if (Global.instance.coinNumber < 10)
        {
            showHint.text = "金币不足！";
            return;
        }
        if ((isSpeed == 0 || isSpeed == 2) && creatTime == 15f) {
            isClick = true;
            Global.instance.coinNumber -= 10;
            m_MyAudioSource[0].Play();
        }
    }
    #endregion

    #region Private Methods
    private void LaunchOutRocket()
    {
        Debug.Log("加速器");
        rocket = Instantiate<GameObject>(Resources.Load("Prefabs/Accelerator") as GameObject);

        // 设置火箭的初始位置
        Vector3 startLocation = new Vector3(0, player.transform.position.y, player.transform.position.z);
        rocket.transform.position = startLocation;

        // 设置火箭的速度
        Rigidbody rb = rocket.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(rb.velocity.x, 20f, 200f);

        DispearTime -= Time.deltaTime;
        player.GetComponent<HAHAController>().forwardSpeed = targetSpeed;
        isSpeed = 1;
    }

    private void LoadingBarWorking()
    {
        currentAmount -= timeSpeed * Time.deltaTime;
        LoadingBar.GetComponent<Image>().fillAmount = currentAmount;

        creatTime -= Time.deltaTime;//加速时间
        if (creatTime <= 12)
        {
            if (rocket != null)
            {
                Destroy(rocket);
            }
        }
        if (creatTime <= 0)
        {
            player.GetComponent<HAHAController>().forwardSpeed = forwardSpeed;
            isSpeed = 0;
            creatTime = 15f;
            currentAmount = 1;
            LoadingBar.GetComponent<Image>().fillAmount = 0;
        }
    }
    #endregion
}
