using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagnetClick : MonoBehaviour {

    #region Public Variables
    public float magnetTime;
    public Text showHint;
    public GameObject player;
    public GameObject LoadingBar;
    #endregion

    #region Private Variables
    [SerializeField] private float currentAmount;
    [SerializeField] private float timeSpeed;
    private AudioSource[] m_MyAudioSource = new AudioSource[1];
    private bool isLoadingBarWorking = false;
    #endregion

    void Start()
    {
        // Loading Bar Init
        timeSpeed = 1 / magnetTime;
        currentAmount = 1;

        m_MyAudioSource = GetComponents<AudioSource>();
    }

    void Update()
    {
        // 只获得LoaclPlayer
        if (player == null) player = HAHAController.GetHaHaInstance().gameObject;
        if(isLoadingBarWorking) LoadingBarWorking();
    }

    #region Public Methods
    public void OnMagnetButtonClick()
    {
        if (Global.instance.coinNumber < 10)
        {
            showHint.text = "金币不足！";
            return;
        }
        if (!HAHAController.GetHaHaInstance().isMagnet)
        {
            Global.instance.coinNumber -= 10;
            HAHAController.GetHaHaInstance().magnetTime = magnetTime;
            HAHAController.GetHaHaInstance().isMagnet = true;
            isLoadingBarWorking = true;
            player.GetComponent<SpriteRenderer>().material = Resources.Load("Materials/haha") as Material;
            m_MyAudioSource[0].Play();
        }
    }
    #endregion

    #region Private Methods
    private void LoadingBarWorking()
    {
        currentAmount -= timeSpeed * Time.deltaTime;
        LoadingBar.GetComponent<Image>().fillAmount = currentAmount;

        magnetTime -= Time.deltaTime;
        if (magnetTime <= 0)
        {
            RecoverPlayer();
            ResetLoadingBar();
        }
    }

    private void RecoverPlayer()
    {
        HAHAController.GetHaHaInstance().isMagnet = false;
        magnetTime = 5f;
        player.GetComponent<SpriteRenderer>().material = Resources.Load("Materials/init_haha") as Material;
        
    }

    private void ResetLoadingBar()
    {
        isLoadingBarWorking = false;
        currentAmount = 1;
        LoadingBar.GetComponent<Image>().fillAmount = 0;
    }
    #endregion
}
