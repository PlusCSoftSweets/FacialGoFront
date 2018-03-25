using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextItemController : MonoBehaviour {
    List<GameObject> messages = new List<GameObject>();
    GameObject item;
    GameObject parent;
    Vector3 itemLocalPos;
    Vector2 contentSize;
    float itemHeight;
    string[] array = { "那你很棒哦", "是在下输了", "扎心了老铁" ,"那又关我什么事","都可以，没关系"};
    public GameObject Content;
    public GameObject Description;

    void Start()
    {
        item = (GameObject)Instantiate(Resources.Load("Prefabs/TextItem"));
        parent = GameObject.Find("Content");
        contentSize = parent.GetComponent<RectTransform>().sizeDelta;
        itemHeight = item.GetComponent<RectTransform>().rect.height;
        itemLocalPos = item.transform.localPosition;
        for (int i = 0; i < 5; i++)
        {
            AddItem(i);
        }
        WordItemClick();
    }

    //添加列表项  
    public void AddItem(int i)
    {
        GameObject a = Instantiate(item) as GameObject;
        a.transform.Find("Text").GetComponent<Text>().text = array[i];
        a.transform.parent = parent.transform;
        a.transform.localPosition = new Vector3(itemLocalPos.x, itemLocalPos.y - messages.Count * itemHeight, 0);
        messages.Add(a);
        if (contentSize.y <= messages.Count * itemHeight)//增加内容的高度  
        {
            parent.GetComponent<RectTransform>().sizeDelta = new Vector2(contentSize.x, messages.Count * itemHeight);
        }
    }
    //添加评论点击监听器
    public void WordItemClick()
    {
        foreach (Transform child in Content.transform)
        {
            Button btn = child.GetComponent<Button>();
            Transform grand = child.Find("Text");
            string text = grand.GetComponent<Text>().text;
            btn.onClick.AddListener(delegate () {
                this.onItemClick(text);
            });
        }
    }
    public void onItemClick(string text)
    {
        Description.GetComponent<Text>().text = text;
    }
}
