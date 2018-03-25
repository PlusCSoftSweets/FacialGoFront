using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class ToolSceneManagerController : MonoBehaviour {
    public GameObject DetailCanvas;
    public GameObject ContentCanvas;

	// Use this for initialization
	void Start () {
        //找到按钮，并且获取按钮的Button组件  
        //Button btn = GameObject.Find("ToolItem (1)").GetComponent<Button>();
        //注册按钮的点击事件  
        foreach (Transform child in  ContentCanvas.transform)
        {
            Debug.Log("所有该脚本的物体下的子物体名称:" + child.name);
            Button btn = child.GetComponent<Button>();
            btn.onClick.AddListener(delegate () {
                this.onItemClick();
            });
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void onCloseButtonClick()
    {
        DetailCanvas.SetActive(false);
        Debug.Log("关闭按钮");
    }
    public void onItemClick()
    {
        //Destroy(this.gameObject);
        DetailCanvas.SetActive(true);
        //this.gameObject.SetActive(false);
    }
}
