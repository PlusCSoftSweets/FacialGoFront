using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class MainSceneMangerController : MonoBehaviour {

    public GUISkin Skin;

    public GameObject usernameGO;
    public GameObject levelGO;
    public GameObject diamondGO;
    public GameObject userFace;
    public GameObject accountDetail;
    public GameObject detailManager;
    public GameObject friendCanvas;
    public GameObject rankCanvas;
    public GameObject friendContent;
    public GameObject friendItemPrefab;

    public bool isNameChange;
    public bool isAvatorChange;

    [System.Serializable]
    public class ResponseItem
    {
        public int status { get; set; }
        public string msg { get; set; }
        public DataItem data { get; set; }
    }

    [System.Serializable]
    public class DataItem
    {
        public UserItem user { get; set; }
    }

    [System.Serializable]
    public class UserItem
    {
        public string user_id { get; set; }
        public string nickname { get; set; }
        public string avatar { get; set; }
        public int exp { get; set; }
        public int diamand { get; set; }
    }

    public void Awake()
    {
        
    }

    void Start() {
        isNameChange = false;
        isAvatorChange = false;

        UpdateUserInfo();

        // 通过Http从数据库拿到三个条目再赋值
        usernameGO.GetComponent<Text>().text = GlobalUserInfo.userInfo.nickname;
        diamondGO.GetComponent<Text>().text = GlobalUserInfo.userInfo.diamand.ToString();
        levelGO.GetComponent<Text>().text = "Lv." + Assets.Resources.Scripts.LevelCalculation.ExpToLevel(GlobalUserInfo.userInfo.exp).ToString();
        string url = "http://123.207.93.25:9001/user/";
        url += GlobalUserInfo.userInfo.user_id;
        url += "/avatar";
        StartCoroutine(_GetUserFace(url, 100, 100));

        StartCoroutine(PollInvitation());
    }

    void Update() {
        if (isNameChange) {
            UpdateUserInfo();
        }
    }

    [System.Serializable]
    public class InvitationItem {
        public string inviter_id;
        public string room_id;        
    }

    [System.Serializable]
    public class InvitationResp {
        public int status;
        public string msg;
        public InvitationItem[] data;
    }

    // 轮询查看邀请
    IEnumerator PollInvitation() {
        string url = "http://123.207.93.25:9001/game/pollInvitation?token=" + GlobalUserInfo.tokenInfo.token;
        while (true) {
            UnityWebRequest req = UnityWebRequest.Get(url);
            yield return req.SendWebRequest();

            if (req.isNetworkError || req.isHttpError) {
                Debug.LogError(req.downloadHandler.text);
            } else {
                var resp = JsonUtility.FromJson<InvitationResp>(req.downloadHandler.text);
                var invitationList = resp.data;
                if (invitationList.Length > 0) {
                    // 只取第一个邀请
                    var invitation = invitationList[0];
                    // TODO: 弹窗显示是否接受邀请
                    // 停止轮询
                    Debug.Log("Got invitation, stopping coroutine");
                    StopCoroutine("PollInvitation");
                } else {
                    // 没有收到任何邀请，1s之后继续轮询
                    Debug.Log("Polling");
                    yield return new WaitForSeconds(1.0f);
                }
            }
        }
    }

    // 开房间场景
    public void OnOpenRoomButtonClick() {
        Debug.Log("Open Room Button Click");
        // RoomOptions options = new RoomOptions();
        // options.MaxPlayers = 2;
        // String roomStr = GlobalUserInfo.tokenInfo.account + DateTime.Now.ToFileTime().ToString();
        // PhotonNetwork.JoinOrCreateRoom(roomStr, options, TypedLobby.Default);
        StartCoroutine(FadeScene());
    }

    // 账号详情
    public void OnAvatorClick(){
        accountDetail.SetActive(true);
    }

    // 点击好友图标
    public void OnFriendClick() {
        friendCanvas.SetActive(true);
        StartCoroutine(GetFriendList());
    }
    //点击排行图标
    public void OnRankClick()
    {
        rankCanvas.SetActive(true);
    }

    [System.Serializable]
    public class FriendListItem {
        public int status;
        public string msg;
        public _UserItem[] data;

        [System.Serializable]
        public class _UserItem {
            public string user_id;
            public string nickname;
            public string avatar;
            public int exp;
        }
    }

    IEnumerator GetFriendList() {
        string url = "http://123.207.93.25:9001/user/" + GlobalUserInfo.userInfo.user_id + "/friend"
                        + "?token=" + GlobalUserInfo.tokenInfo.token;
        Debug.Log("Getting " + url);
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.isNetworkError || req.isHttpError) {
            Debug.LogError(req.error);
            Debug.Log(req.downloadHandler.text);
            // TODO: 弹窗提示
        } else {
            // 清理当前的好友
            foreach (Transform chlid in friendContent.transform) {
                Destroy(chlid.gameObject);
            }
            Debug.Log("Get friend list success!");
            var json = JsonUtility.FromJson<FriendListItem>(req.downloadHandler.text);
            foreach (var user in json.data) {
                GameObject oneFriend = Instantiate(friendItemPrefab, friendContent.transform);
                oneFriend.GetComponentInChildren<Text>().text = user.nickname;
                UserItem userItem = new UserItem();
                userItem.user_id = user.user_id;
                userItem.nickname = user.nickname;
                userItem.avatar = user.avatar;
                userItem.exp = user.exp;
                UserInfo userInfo = oneFriend.GetComponent<UserInfo>();
                userInfo.userItem = userItem;
                // userInfo.OnClick += (info) => {
                //     Debug.Log(info.userItem.nickname);
                // };
                // 获取头像
                StartCoroutine(GetAvatar(user.user_id, oneFriend.GetComponentInChildren<CircleImage>(), 150, 150));
            }
        }
    }

    IEnumerator GetAvatar(string userId, CircleImage img, int width, int height) {
        string url = "http://123.207.93.25:9001/user/" + userId + "/avatar";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError) {
            Debug.LogError(request.error);
            // 弹窗提示错误
        }
        else {
            Debug.Log("Form upload complete!");
            Debug.Log(img);
            img.sprite = GetSpriteFromBytes(request.downloadHandler.data, width, height);
        }
    }

    public void CloseFriendList() {
        friendCanvas.SetActive(false);
    }

    // 获取人脸
    IEnumerator _GetUserFace(string url, int width, int height) {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError) {
            Debug.Log(request.error);
            // 弹窗提示错误
        }
        else {
            Debug.Log("Form upload complete!");
            userFace.GetComponent<CircleImage>().sprite = GetSpriteFromBytes(request.downloadHandler.data, width, height);
        }
    }

    // 图像从二进制流到精灵
    public static Sprite GetSpriteFromBytes(byte[] data, int width, int height) {
        Texture2D tex = new Texture2D(width, height);
        try
        {
            tex.LoadImage(data);
        }
        catch (Exception)
        {

        }
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
    }

    // 更新用户姓名
    public void UpdateUserInfo() {
        string url = "http://123.207.93.25:9001/user/";
        url += GlobalUserInfo.userInfo.user_id;
        StartCoroutine(GetUserInfo(url));
        usernameGO.GetComponent<Text>().text = GlobalUserInfo.userInfo.nickname;
        isNameChange = false;
        detailManager.GetComponent<DetailManagerController>().isNicknameChange = false;
    }

    // 更改改名状态信息
    public void UpdateIsChangeName(DetailManagerController detail) {
        isNameChange = detail.isNicknameChange;
        Debug.Log(isNameChange);
    }

    // 获取用户信息
    IEnumerator GetUserInfo(string url) {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError) {
            Debug.Log(request.error);
        }
        else {
            Debug.Log("Get complete!");
            var responseJson = JsonConvert.DeserializeObject<ResponseItem>(request.downloadHandler.text);
            if (responseJson.status == 0) {
                // 输出登陆成功信息
                Debug.Log(responseJson.msg);
                var userJson = responseJson.data.user;
                GlobalUserInfo.userInfo.nickname = userJson.nickname;
            }
            else {
                Debug.Log(responseJson.msg);
                // 弹窗显示错误信息
            }
        }
    }

    IEnumerator FadeScene() {
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("OpenRoomScene");
    }
}