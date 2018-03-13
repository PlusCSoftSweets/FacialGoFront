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
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.automaticallySyncScene = true;
        // the following line checks if this client was just created (and not yet online). if so, we connect
        if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated)
        {
            // Connect to the photon master-server. We use the settings saved in PhotonServerSettings (a .asset file in this project)
            PhotonNetwork.ConnectUsingSettings("v1.0");
        }

        // generate a name for this player, if none is assigned yet
        if (String.IsNullOrEmpty(PhotonNetwork.playerName))
        {
            PhotonNetwork.playerName = GlobalUserInfo.tokenInfo.account;
        }
        // if you wanted more debug out, turn this on:
        // PhotonNetwork.logLevel = NetworkLogLevel.Full;
    }

    void Start() {
        isNameChange = false;
        isAvatorChange = false;

        // 通过Http从数据库拿到三个条目再赋值
        usernameGO.GetComponent<Text>().text = GlobalUserInfo.userInfo.nickname;
        diamondGO.GetComponent<Text>().text = GlobalUserInfo.userInfo.diamand.ToString();
        levelGO.GetComponent<Text>().text = "Lv." + Assets.Resources.Scripts.LevelCalculation.ExpToLevel(GlobalUserInfo.userInfo.exp).ToString();
        string url = "http://123.207.93.25:9001/user/";
        url += GlobalUserInfo.userInfo.user_id;
        url += "/avatar";
        StartCoroutine(_GetUserFace(url, 100, 100));
    }

    void Update() {
        if (isNameChange) {
            UpdateUserInfo();
        }
    }

    // 开房间场景
    public void OnOpenRoomButtonClick() {
        Debug.Log("Open Room Button Click");
        StartCoroutine(FadeScene());
    }

    // 账号详情
    public void OnAvatorClick(){
        accountDetail.SetActive(true);
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
    private Sprite GetSpriteFromBytes(byte[] data, int width, int height) {
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