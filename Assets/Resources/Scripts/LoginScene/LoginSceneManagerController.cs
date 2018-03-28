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
    private bool ifChangeScene = false;
    private string gameVersion = "v1.0";
#endregion

    void Start()
    {
        // 实现自动登陆
        if (PlayerPrefs.GetString("phone_number") != null)
        {
            phoneInputField.text = PlayerPrefs.GetString("phone_number");
            if (PlayerPrefs.GetString("password") != null)
            {
                checkInputField.text = PlayerPrefs.GetString("password");
                OnLoginClick();
            }
        }
            
    }

    public void Update()
    {
        if (PhotonNetwork.connected)
        {
            if (ifChangeScene)
            {
                StartCoroutine(FadeScene());
            }else
            {
                PhotonNetwork.LeaveLobby();
            }
        }
    }

    /*
     * 更改登陆账号
     */
    public void OnChangeAccountClick() {
        Debug.Log("Change Account Click");
        ifChangeScene = false;  // 取消转场
        GameObject.Find("WaittingObject").transform.localPosition = new Vector3(0, 0, -1001);
        GameObject.Find("LoginObject").transform.localPosition = new Vector3(0, 0, 0);
    }

    /*
     * 获取验证码
     */
    public void OnCheckClick() {
        Debug.Log("Check Button Click");
        string phone = phoneInputField.text;

        if (phone.Length == 0) {
            Debug.Log("please input your phone number!");
            return;
        }
        else {
            if (phone.Length != 11) {
                Debug.Log("please input the correct phone number!");
                return;
            }
            StartCoroutine(PostCheck(phone));
        }

    }

    /*
     * 登陆按钮
     */
    public void OnLoginClick() {
        string phone = phoneInputField.text;
        string check = checkInputField.text;

        if (phone.Length == 0) {
            Debug.Log("please input your phone number!");
            return;
        }
        else if (check.Length == 0) {
            Debug.Log("please input your check number!");
            return;
        }
        else {
            if (phone.Length != 11) {
                Debug.Log("please input the correct phone number!");
                return;
            }
            ifChangeScene = true;
            PlayerPrefs.SetString("phone_number", phone);
            PlayerPrefs.SetString("password", check);
            StartCoroutine(PostLoginInfo(phone, check));
            GameObject.Find("WaittingObject").transform.localPosition = new Vector3(0, 0, 0);
            GameObject.Find("LoginObject").transform.localPosition = new Vector3(0, 0, -1001);
        }
    }

    /*
     * 网络通信
     */
    IEnumerator PostLoginInfo(string phone, string check) {
        WWWForm userInfo = new WWWForm();
        userInfo.AddField("phone_number", phone);
        userInfo.AddField("password", check);

        UnityWebRequest www = UnityWebRequest.Post("http://123.207.93.25:9001/user/login", userInfo);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
            Debug.Log("error");
            // 弹窗提示错误
            GameObject.Find("WaittingObject").transform.localPosition = new Vector3(0, 0, -1001);
            GameObject.Find("LoginObject").transform.localPosition = new Vector3(0, 0, 0);
        }
        else {
            Debug.Log("Form upload complete!");
            Debug.Log(www.downloadHandler.text);
            var loginJson = JsonConvert.DeserializeObject<LoginItem>(www.downloadHandler.text);
            if (loginJson.status == 0) {
                // 输出登陆成功信息
                Debug.Log(loginJson.msg);
                var tokenJson = loginJson.data;
                var userJson = tokenJson.user;
                GlobalUserInfo.SetTokenItemInstance(tokenJson.token, phone);
                GlobalUserInfo.SetUserItemInstance(userJson);
                ConnectToServer();
            }
            else {
                Debug.Log(loginJson.msg);
                // 弹窗显示错误信息
                GameObject.Find("WaittingObject").transform.localPosition = new Vector3(0, 0, -1001);
                GameObject.Find("LoginObject").transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }

    // 接入游戏大厅
    void ConnectToServer() {
        PhotonNetwork.automaticallySyncScene = false;
        PhotonNetwork.autoCleanUpPlayerObjects = false;
        if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated) {
            PhotonNetwork.ConnectUsingSettings(gameVersion);
        }
        if (String.IsNullOrEmpty(PhotonNetwork.playerName)) {
            PhotonNetwork.playerName = GlobalUserInfo.userInfo.user_id;
        }
    }

    /*
     * 获取验证码
     */
    IEnumerator PostCheck(string phone) {
        WWWForm userInfo = new WWWForm();
        userInfo.AddField("phone_number", phone);
        UnityWebRequest www = UnityWebRequest.Post("http://123.207.93.25:9001/user", userInfo);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
            // 弹窗提示错误
        }
        else {
            Debug.Log("Form upload complete!");
            var checkJson = JsonConvert.DeserializeObject<CheckItem>(www.downloadHandler.text);
            if (checkJson.status == 0) {
                checkInputField.text = checkJson.data;
            }
            else {
                // 弹窗显示错误
                Debug.Log(checkJson.msg);
            }
        }
    }

    /*
     * 渐变转场
     */
    IEnumerator FadeScene() {
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("MainScene");
    }

    #region Photon.PunBehaviour CallBacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN");
    }

    public override void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("OnDisconnectedFromPhoton() was called by PUN");
    }
    #endregion
}