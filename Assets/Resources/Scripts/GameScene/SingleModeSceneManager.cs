using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleModeSceneManager : MonoBehaviour {

    #region Public Variables
    public Text goldTextField;

    public SpriteRenderer background;
    public SpriteRenderer race;
    public Camera raceCamera;
    public Transform mainCamera;
    public Transform uiCamera;
    public Transform[] trees;
    #endregion

    #region Private Variables
    private bool ifUpdate = false;
    private HAHAController instance;
    #endregion

    void Start () {
        InitScene();
    }
	
    // 1. 更新用户的金币数
    // 2. 在进入魔镜的时候保存玩家当前位置
	void Update () {
        goldTextField.text = Global.instance.coinNumber.ToString();
        if (instance == null) instance = HAHAController.GetHaHaInstance();
        else if (!ifUpdate && instance.isEnterMirror) {
            ifUpdate = true;
            UpdateGlobalPosition();
        }
    }

    #region Private Method
    // 初始化场景
    private void InitScene() {
        if (Global.instance.currentScene == 0) {
            SetScene("Sprites/Backgound/森林/森林-背景",
                     "Sprites/Backgound/森林/森林-跑道",
                     "Sprites/Backgound/森林/森林-树木",
                     0.196f, 0.608f, 0.749f);
        }
        else if (Global.instance.currentScene == 1) {
            SetScene("Sprites/Backgound/沙漠/沙漠-背景",
                     "Sprites/Backgound/沙漠/沙漠-跑道",
                     "Sprites/Backgound/沙漠/沙漠-仙人掌右",
                     0.957f, 0.780f, 0.514f);
        }
        else if (Global.instance.currentScene == 2) {
            SetScene("Sprites/Backgound/雪地/雪地-背景",
                     "Sprites/Backgound/雪地/雪地-跑道",
                     "Sprites/Backgound/雪地/雪地-树木",
                     0.424f, 0.596f, 0.749f);
        }
        else if (Global.instance.currentScene == 3) {
            SetScene("Sprites/Backgound/村庄/村庄-背景",
                     "Sprites/Backgound/村庄/村庄-跑道",
                     "Sprites/Backgound/村庄/村庄-树木",
                     0.651f, 0.424f, 0.388f);
        }
        else if (Global.instance.currentScene == 4) {
            SetScene("Sprites/Backgound/村庄-变色/村庄-背景",
                     "Sprites/Backgound/村庄-变色/村庄-跑道",
                     "Sprites/Backgound/村庄-变色/村庄-树木",
                     0.643f, 0.675f, 0.447f);
        }
        else if (Global.instance.currentScene == 5) {
            SetScene("Sprites/Backgound/森林-变色/森林-背景",
                     "Sprites/Backgound/森林-变色/森林-跑道",
                     "Sprites/Backgound/森林-变色/森林-树木",
                     0.431f, 0.388f, 0.553f);
        }
        else if (Global.instance.currentScene == 6) {
            SetScene("Sprites/Backgound/沙漠-变色/沙漠-背景",
                     "Sprites/Backgound/沙漠-变色/沙漠-跑道",
                     "Sprites/Backgound/沙漠-变色/沙漠-树木",
                     0.851f, 0.757f, 0.757f);
        }
        else if (Global.instance.currentScene == 7) {
            SetScene("Sprites/Backgound/雪地-变色/雪地-背景",
                     "Sprites/Backgound/雪地-变色/雪地-跑道",
                     "Sprites/Backgound/雪地-变色/雪地-树木",
                     0.631f, 0.855f, 0.792f);
        }
        else {
            SetScene("Sprites/Backgound/森林/森林-背景",
                     "Sprites/Backgound/森林/森林-跑道",
                     "Sprites/Backgound/森林/森林-树木",
                     0.196f, 0.608f, 0.749f);
        }
    }

    // 设置场景
    private void SetScene(String bg, String ra, String tr, float r, float g, float b) {
        background.sprite = Resources.Load(bg, typeof(Sprite)) as Sprite;
        race.sprite = Resources.Load(ra, typeof(Sprite)) as Sprite;
        foreach (Transform tree in trees) {
            SpriteRenderer temp = tree.gameObject.GetComponent<SpriteRenderer>();
            temp.sprite = Resources.Load(tr, typeof(Sprite)) as Sprite;
        }
        raceCamera.backgroundColor = new Color(r, g, b);
    }

    // 当碰撞到Mirror前面的墙的时候，记录目标位置
    private void UpdateGlobalPosition() {
        if (instance.gameObject.GetPhotonView().isMine) {
            Global.instance.playerPosition = instance.gameObject.transform.position + new Vector3(0, 2, 7);
            Global.instance.mainCameraPosition = mainCamera.position + new Vector3(0, 0, 7);
            Global.instance.uiCameraPosition = uiCamera.position + new Vector3(0, 0, 7);
            for (int i = 0; i < 10; i++) {
                Global.instance.treesPosition[i] = trees[i].position + new Vector3(0, 0, 7);
            }
        }
    }
    #endregion
}