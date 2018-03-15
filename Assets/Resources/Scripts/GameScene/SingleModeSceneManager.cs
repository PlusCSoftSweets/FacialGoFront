﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleModeSceneManager : MonoBehaviour {

    #region Public Variables
    public Text goldTextField;                // 显示金币数量的文本框

    public SpriteRenderer background;
    public SpriteRenderer race;
    public Camera raceCamera;
    #endregion

    // Use this for initialization
    void Start () {
        InitScene();
    }
	
	// Update is called once per frame
	void Update () {
        goldTextField.text = Global.instance.coinNumber.ToString();
    }

    #region Private Method
    // 初始化场景
    private void InitScene()
    {
        if (Global.instance.currentScene == 0)
        {
            SetScene("Sprites/Backgound/森林/森林-背景",
                     "Sprites/Backgound/森林/森林-跑道",
                     "Sprites/Backgound/森林/森林-树木",
                     0.196f, 0.608f, 0.749f);
        }
        else if (Global.instance.currentScene == 1)
        {
            SetScene("Sprites/Backgound/沙漠/沙漠-背景",
                     "Sprites/Backgound/沙漠/沙漠-跑道",
                     "Sprites/Backgound/沙漠/沙漠-仙人掌右",
                     0.957f, 0.780f, 0.514f);
        }
        else if (Global.instance.currentScene == 2)
        {
            SetScene("Sprites/Backgound/雪地/雪地-背景",
                     "Sprites/Backgound/雪地/雪地-跑道",
                     "Sprites/Backgound/雪地/雪地-树木",
                     0.424f, 0.596f, 0.749f);
        }
        else if (Global.instance.currentScene == 3)
        {
            SetScene("Sprites/Backgound/村庄/村庄-背景",
                     "Sprites/Backgound/村庄/村庄-跑道",
                     "Sprites/Backgound/村庄/村庄-树木",
                     0.651f, 0.424f, 0.388f);
        }
        else
        {
            SetScene("Sprites/Backgound/森林/森林-背景",
                     "Sprites/Backgound/森林/森林-跑道",
                     "Sprites/Backgound/森林/森林-树木",
                     0.196f, 0.608f, 0.749f);
        }
    }

    // 设置场景
    private void SetScene(String bg, String ra, String tr, float r, float g, float b)
    {
        background.sprite = Resources.Load(bg, typeof(Sprite)) as Sprite;
        race.sprite = Resources.Load(ra, typeof(Sprite)) as Sprite;
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        foreach (GameObject tree in trees)
        {
            SpriteRenderer temp = tree.GetComponent<SpriteRenderer>();
            temp.sprite = Resources.Load(tr, typeof(Sprite)) as Sprite;
        }
        raceCamera.backgroundColor = new Color(r, g, b);
    }
    #endregion
}
