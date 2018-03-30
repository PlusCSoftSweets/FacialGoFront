using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
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
    public GameObject RankContent;

    void Start()
    {
        FriendLine.SetActive(false);
        item = (GameObject)Instantiate(Resources.Load("Prefabs/RankItem"));
        parent = GameObject.Find("RankContent");
        contentSize = parent.GetComponent<RectTransform>().sizeDelta;
        itemHeight = item.GetComponent<RectTransform>().rect.height;
        itemLocalPos = item.transform.localPosition;
        string url = "http://123.207.93.25:9001/user/";
        StartCoroutine(GetFriendList(url));
    }
    [System.Serializable]
    public class RankItem
    {
        public int status;
        public string msg;
        public _UserItem[] data;

        [System.Serializable]
        public class _UserItem
        {
            public string user_id;
            public string nickname;
            public string avatar;
            public int exp;
        }
    }

    //添加列表项  
    public void AddItem(string id)
    {
        GameObject a = Instantiate(item) as GameObject;
        a.transform.Find("Text").GetComponent<Text>().text = id;
        a.transform.parent = parent.transform;
        a.transform.localPosition = new Vector3(itemLocalPos.x, itemLocalPos.y - messages.Count * itemHeight, 0);
        messages.Add(a);
        a.transform.Find("RankNum").GetComponent<Text>().text = messages.Count.ToString();
        if (contentSize.y <= messages.Count * itemHeight)//增加内容的高度  
        {
            parent.GetComponent<RectTransform>().sizeDelta = new Vector2(contentSize.x, messages.Count * itemHeight);
        }
    }
    public void onAllClick()
    {
        AllLine.SetActive(true);
        FriendLine.SetActive(false);
        //加载全部排行榜，利用AddItem函数
        messages.Clear();
        string url = "http://123.207.93.25:9001/user/";
        StartCoroutine(GetFriendList(url));
    }
    public void onFriendClick()
    {
        AllLine.SetActive(false);
        FriendLine.SetActive(true);
        messages.Clear();
        string url = "http://123.207.93.25:9001/user/" + GlobalUserInfo.userInfo.user_id + "/friend/rank"
                + "?token=" + GlobalUserInfo.tokenInfo.token;
        StartCoroutine(GetFriendList(url));
        //加载好友排行榜
    }

    IEnumerator GetFriendList(string _url)
    {
        string url = _url;
        Debug.Log("Getting " + url);
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.isNetworkError || req.isHttpError)
        {
            Debug.LogError(req.error);
            Debug.Log(req.downloadHandler.text);
            // TODO: 弹窗提示
        }
        else
        {
            foreach (Transform chlid in RankContent.transform)
            {
                Destroy(chlid.gameObject);
            }
            Debug.Log("Get friend list success!");
            var json = JsonUtility.FromJson<RankItem>(req.downloadHandler.text);
            foreach (var user in json.data)
            {
                AddItem(user.nickname);
            }
        }
    }
}
