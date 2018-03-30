using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankItemController : MonoBehaviour {
    List<GameObject> messages = new List<GameObject>();
    GameObject item;
    GameObject parent;
    Vector3 itemLocalPos;
    Vector2 contentSize;
    float itemHeight;
    public GameObject AllLine;
    public GameObject FriendLine;

    void Start()
    {
        FriendLine.SetActive(false);
       // Debug.Log("排行榜");
        item = (GameObject)Instantiate(Resources.Load("Prefabs/RankItem"));
        parent = GameObject.Find("RankContent");
        contentSize = parent.GetComponent<RectTransform>().sizeDelta;
        itemHeight = item.GetComponent<RectTransform>().rect.height;
        itemLocalPos = item.transform.localPosition;
        for (int i = 0; i < 14; i++)
        {
            AddItem();
        }
    }

    //添加列表项  
    public void AddItem()
    {
        GameObject a = Instantiate(item) as GameObject;
        a.transform.Find("Text").GetComponent<Text>().text = "蛙儿子回家了吗";
        a.transform.parent = parent.transform;
        a.transform.localPosition = new Vector3(itemLocalPos.x, itemLocalPos.y - messages.Count * itemHeight, 0);
        messages.Add(a);
        if (contentSize.y <= messages.Count * itemHeight)//增加内容的高度  
        {
            parent.GetComponent<RectTransform>().sizeDelta = new Vector2(contentSize.x, messages.Count * itemHeight);
        }
    }

    //移除列表项  
    //public void RemoveItem(GameObject t)
    //{
    //    int index = messages.IndexOf(t);
    //    messages.Remove(t);
    //    Destroy(t);

    //    for (int i = index; i < messages.Count; i++)//移除的列表项后的每一项都向前移动  
    //    {
    //        messages[i].transform.localPosition += new Vector3(0, itemHeight, 0);
    //    }

    //    if (contentSize.y <= messages.Count * itemHeight)//调整内容的高度  
    //        parent.GetComponent<RectTransform>().sizeDelta = new Vector2(contentSize.x, messages.Count * itemHeight);
    //    else
    //        parent.GetComponent<RectTransform>().sizeDelta = contentSize;
    //}
    //public void CancleOnClick()
    //{
    //    RemoveItem(this.gameObject);
    //}
    public void onAllClick()
    {
        AllLine.SetActive(true);
        FriendLine.SetActive(false);
        //加载全部排行榜，利用AddItem函数
    }
    public void onFriendClick()
    {
        AllLine.SetActive(false);
        FriendLine.SetActive(true);
        //加载好友排行榜
    }
}
