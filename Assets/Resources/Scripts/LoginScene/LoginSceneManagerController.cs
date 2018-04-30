using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;

public class LoginSceneManagerController : Photon.PunBehaviour {

    #region Public Variables
    public InputField phoneInputField;
    public InputField checkInputField;
    public Transform waittingObject;
    public Transform loginObject;
    public Text outPutMsg;
    public GameObject dialogArea;

    [System.Serializable]
    public class CheckItem {
        public int status { get; set; }
        public string msg { get; set; }
        public string data { get; set; }
    }

    [System.Serializable]
    public class LoginItem {
        public int status { get; set; }
        public string msg { get; set; }
        public TokenItem data { get; set; }
    }

    [System.Serializable]
    public class TokenItem {
        public UserItem user { get; set; }
        public string token { get; set; }
    }

    [System.Serializable]
    public class UserItem {
        public string user_id { get; set; }
        public string nickname { get; set; }
        public string avatar { get; set; }
        public int exp { get; set; }
        public int diamand { get; set; }
    }
    #endregion

    #region Private Variables
    private bool ifLogin = false;
    private string gameVersion = "v1.0";
    #endregion

    void Start() {
        AutoLogin();
    }

    #region Private Methods
    private void AutoLogin() {
        if (PlayerPrefs.GetString("phone_number") != "") {
            phoneInputField.text = PlayerPrefs.GetString("phone_number");
            if (PlayerPrefs.GetString("password") != "") {
                checkInputField.text = PlayerPrefs.GetString("password");
                OnLoginClick();
            }
        }
    }

    private void ShowOutDialog(string msg) {
        DialogMessage(msg);
        dialogArea.SetActive(true);
    }

    private void DialogMessage(string msg) {
        outPutMsg.text = msg;
    }

    private void ClearLocalInfo() {
        PlayerPrefs.SetString("phone_number", "");
        PlayerPrefs.SetString("password", "");
    }

    private void ConnectToServer() {
        PhotonNetwork.automaticallySyncScene = false;
        PhotonNetwork.autoCleanUpPlayerObjects = false;
        if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated) {
            PhotonNetwork.ConnectUsingSettings(gameVersion);
        }
        if (String.IsNullOrEmpty(PhotonNetwork.playerName)) {
            PhotonNetwork.playerName = GlobalUserInfo.userInfo.user_id;
        }
    }
    #endregion

    #region Public Methods
    public void OnAcceptButton() {
        dialogArea.SetActive(false);
    }

    public void OnCheckClick() {
        string phone = phoneInputField.text;

        if (phone.Length == 0) {
            ShowOutDialog("请输入电话号码！");
            return;
        }
        else {
            if (phone.Length != 11) {
                ShowOutDialog("请输入正确的电话号码！");
                return;
            }
            StartCoroutine(PostCheck(phone));
        }
    }

    public void OnLoginClick() {
        string phone = phoneInputField.text;
        string check = checkInputField.text;

        if (phone.Length == 0) {
            ShowOutDialog("请输入手机号码！");
            return;
        }
        else if (check.Length == 0) {
            ShowOutDialog("请输入验证码！");
            return;
        }
        else {
            if (phone.Length != 11) {
                ShowOutDialog("请输入正确的电话号码！");
                return;
            }
            ifLogin = true;
            PlayerPrefs.SetString("phone_number", phone);
            PlayerPrefs.SetString("password", check);
            StartCoroutine(PostLoginInfo(phone, check));
            waittingObject.localPosition = new Vector3(0, 0, 0);
            loginObject.localPosition = new Vector3(0, 0, -1001);
        }
    }

    private void OnChangeAccountClick() {
        ifLogin = false;
        ClearLocalInfo();
        PhotonNetwork.LeaveLobby();
        waittingObject.localPosition = new Vector3(0, 0, -1001);
        loginObject.localPosition = new Vector3(0, 0, 0);
    }
    #endregion

    #region IEnumerator Methods
    IEnumerator PostLoginInfo(string phone, string check) {
        WWWForm userInfo = new WWWForm();
        userInfo.AddField("phone_number", phone);
        userInfo.AddField("password", check);

        UnityWebRequest www = UnityWebRequest.Post("http://123.207.93.25:9001/user/login", userInfo);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) {
            ShowOutDialog("账号或密码错误！");
            waittingObject.localPosition = new Vector3(0, 0, -1001);
            loginObject.localPosition = new Vector3(0, 0, 0);
        }
        else {
            var loginJson = JsonConvert.DeserializeObject<LoginItem>(www.downloadHandler.text);
            if (loginJson.status == 0) {
                Debug.Log(loginJson.msg);
                var tokenJson = loginJson.data;
                var userJson = tokenJson.user;
                GlobalUserInfo.SetTokenItemInstance(tokenJson.token, phone);
                GlobalUserInfo.SetUserItemInstance(userJson);
                ConnectToServer();
            }
            else {
                ShowOutDialog(loginJson.msg);
                waittingObject.localPosition = new Vector3(0, 0, -1001);
                loginObject.localPosition = new Vector3(0, 0, 0);
            }
        }
    }

    IEnumerator PostCheck(string phone) {
        WWWForm userInfo = new WWWForm();
        userInfo.AddField("phone_number", phone);
        UnityWebRequest www = UnityWebRequest.Post("http://123.207.93.25:9001/user", userInfo);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) {
            ShowOutDialog("网络错误！");
        }
        else {
            var checkJson = JsonConvert.DeserializeObject<CheckItem>(www.downloadHandler.text);
            if (checkJson.status == 0) {
                ShowOutDialog(string.Concat("你的验证码是：", checkJson.data.ToString(), "。请记住您的验证码，它将作为您的登陆密码。"));
                checkInputField.text = checkJson.data;
            }
            else {
                ShowOutDialog(checkJson.msg);
            }
        }
    }

    IEnumerator FadeScene() {
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("MainScene");
    }
    #endregion

    #region Photon.PunBehaviour CallBacks
    public override void OnConnectedToMaster() {
        StartCoroutine(FadeScene());
    }

    public override void OnDisconnectedFromPhoton() {
        ShowOutDialog("服务器崩溃，请退出游戏！");
        waittingObject.localPosition = new Vector3(0, 0, -1001);
        loginObject.localPosition = new Vector3(0, 0, 0);
    }
    #endregion
}