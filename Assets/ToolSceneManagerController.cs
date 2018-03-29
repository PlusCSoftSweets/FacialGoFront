using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class ToolSceneManagerController : MonoBehaviour {
    public GameObject DetailCanvas;
    public GameObject ContentCanvas;
    public GameObject FirstLine;
    public GameObject SecondLine;
    public GameObject ThirdLine;

    void Start () {
        FirstLine.SetActive(true);
        SecondLine.SetActive(false);
        ThirdLine.SetActive(false);
        foreach (Transform child in  ContentCanvas.transform)
        {
            Button btn = child.GetComponent<Button>();
            Transform grand = child.Find("Top");
            Transform grandgrand = grand.Find("Name");
            string text = grandgrand.GetComponent<Text>().text;
            btn.onClick.AddListener(delegate () {
                this.onItemClick(text);
            });
        }
    }
    public void onCloseButtonClick()
    {
        DetailCanvas.SetActive(false);
        Debug.Log("关闭按钮");
    }
    public void onItemClick(string text)
    {
        //Destroy(this.gameObject);
        DetailCanvas.SetActive(true);
        Transform father = DetailCanvas.transform;
        Transform title = father.Find("Title");
        title.GetComponent<Text>().text = text;
    }
    public void otherTool()
    {
        FirstLine.SetActive(true);
        SecondLine.SetActive(false);
        ThirdLine.SetActive(false);
    }
    public void myTool()
    {
        FirstLine.SetActive(false);
        SecondLine.SetActive(true);
        ThirdLine.SetActive(false);
    }
    public void advancedTool()
    {
        FirstLine.SetActive(false);
        SecondLine.SetActive(false);
        ThirdLine.SetActive(true);
    }
}
