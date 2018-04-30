using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;

public class ToolSceneManagerController : MonoBehaviour {
    public GameObject DetailCanvas;
    public GameObject ContentCanvas;
    public GameObject FirstLine;
    public GameObject SecondLine;
    public GameObject ThirdLine;
    public GameObject tool1;
    public GameObject tool2;
    public GameObject tool3;
    public GameObject tool4;
    public GameObject tool5;

    void Start () {
        FirstLine.SetActive(true);
        SecondLine.SetActive(false);
        ThirdLine.SetActive(false);
        foreach (Transform child in  ContentCanvas.transform)
        {
            Button btn = child.GetComponent<Button>();
            Transform grand = child.Find("Top");
            Transform grandBottom = child.Find("Bottom");
            Transform grandgrand = grand.Find("Name");
            Transform grandgrandImage = grand.Find("Image");
            Transform grandgrandGoldNum = grandBottom.Find("GoldNum");
            Image i = grandgrandImage.GetComponent<Image>();
            string text = grandgrand.GetComponent<Text>().text;
            string num = grandgrandGoldNum.GetComponent<Text>().text;
            btn.onClick.AddListener(delegate () {
                this.onItemClick(text, i, num);
            });
        }
    }
    public void onCloseButtonClick()
    {
        DetailCanvas.SetActive(false);
    }
    public void onMainCloseButtonClick()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void onItemClick(string text, Image i, string num)
    {
        DetailCanvas.SetActive(true);
        Transform father = DetailCanvas.transform;
        Transform title = father.Find("Title");
        title.GetComponent<Text>().text = text;
        Transform image = father.Find("Image");
        Image _image = image.GetComponent<Image>();
        _image.sprite = i.sprite;
        Transform goldNum = father.Find("Gold_num");
        goldNum.GetComponent<Text>().text = num;
        Transform desciption = father.Find("Description");
        if (text == "磁铁") desciption.GetComponent<Text>().text = "吸引经过之处的所有金币";
        else if (text == "任意门") desciption.GetComponent<Text>().text = "随机穿越到前方X距离";
        else if (text == "加速器") desciption.GetComponent<Text>().text = "快速前进10s再恢复原速";
        else if (text == "路障") desciption.GetComponent<Text>().text = "放置在对方赛道阻碍其前行";
        else if (text == "动作转置") desciption.GetComponent<Text>().text = "使对方运动方向与手势相反";
    }
    public void otherTool()
    {
        FirstLine.SetActive(true);
        SecondLine.SetActive(false);
        ThirdLine.SetActive(false);
        tool4.SetActive(true);
        tool5.SetActive(true);
    }
    public void myTool()
    {
        FirstLine.SetActive(false);
        SecondLine.SetActive(true);
        ThirdLine.SetActive(false);
        tool4.SetActive(false);
        tool5.SetActive(false);
    }
    public void advancedTool()
    {
        FirstLine.SetActive(false);
        SecondLine.SetActive(false);
        ThirdLine.SetActive(true);
        tool4.SetActive(false);
        tool5.SetActive(false);
    }
}
