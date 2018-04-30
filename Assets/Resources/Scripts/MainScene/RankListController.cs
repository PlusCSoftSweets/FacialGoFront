using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankListController : MonoBehaviour {

   List<GameObject> messages = new List<GameObject>(); 
	public GameObject RankList;  
    GameObject item;
    GameObject parent;  
    Vector3 itemLocalPos;  
    Vector2 contentSize;  
    float itemHeight;  
  
    void Start()  
    {  
		item = (GameObject)Instantiate(Resources.Load("Prefabs/RankItem"));  
		//GameObject mUICanvas = GameObject.Find("FriendCanvas");
       //item.transform.parent = mUICanvas.transform;  
        parent = GameObject.Find("Content");  
        contentSize = parent.GetComponent<RectTransform>().sizeDelta;
		Debug.Log("contentSize" + contentSize.x + "and" + contentSize.y);
        itemHeight = item.GetComponent<RectTransform>().rect.height;  
        itemLocalPos = item.transform.localPosition;  
		//GameObject b = Instantiate(item) as GameObject;
		//b.transform.position = FriendList.transform.position;
        for(int i = 0; i < 14; i++){  
            AddItem ();  
        }  
    }  
  
    //添加列表项  
    public void AddItem()  
    {  
        GameObject a = Instantiate(item) as GameObject;  
		Debug.Log("info" + a.transform.Find ("Text").GetComponent<Text> ().text);
        a.transform.Find ("Text").GetComponent<Text> ().text = "asd";  
		
       // a.transform.FindChild ("cancel").GetComponent<Button> ().onClick.AddListener(  
      //    delegate() {  
        //        RemoveItem(a);  
        //    }  
       // );  
        a.transform.parent = parent.transform;
		Debug.Log("info" + a.transform.Find ("Text").GetComponent<Text> ().text);
        a.transform.localPosition = new Vector3(itemLocalPos.x, itemLocalPos.y - messages.Count * itemHeight, 0);  
		Debug.Log("x of item" + itemLocalPos.x);
		Debug.Log("y of item"+itemLocalPos.y + messages.Count * itemHeight);
        messages.Add(a);  
  
        if (contentSize.y <= messages.Count * itemHeight)//增加内容的高度  
        {  
            parent.GetComponent<RectTransform>().sizeDelta = new Vector2(contentSize.x, messages.Count * itemHeight);  
			Debug.Log("增加高度");
        }  
    }  
  
    //移除列表项  
    public void RemoveItem(GameObject t)  
    {  
        int index = messages.IndexOf(t);  
        messages.Remove(t);  
        Destroy(t);  
  
        for (int i = index; i < messages.Count; i++)//移除的列表项后的每一项都向前移动  
        {  
            messages[i].transform.localPosition += new Vector3(0, itemHeight, 0);  
        }  
  
        if (contentSize.y <= messages.Count * itemHeight)//调整内容的高度  
            parent.GetComponent<RectTransform>().sizeDelta = new Vector2(contentSize.x, messages.Count * itemHeight);  
        else  
            parent.GetComponent<RectTransform>().sizeDelta = contentSize;  
    }  
    public void CancleOnClick(){  
        RemoveItem (this.gameObject);  
    }  
}
