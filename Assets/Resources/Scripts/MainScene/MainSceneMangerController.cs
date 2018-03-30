using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class MainSceneMangerController : Photon.PunBehaviour {

    #region Public Variables
    public GameObject usernameGO;
    public GameObject levelGO;
    public GameObject diamondGO;
    public GameObject userFace;
    public GameObject accountDetail;
    public GameObject detailManager;
    public GameObject friendCanvas;
    public GameObject rankCanvas;
    public GameObject RandomCanvas;
    public GameObject ExplainCanvas;
    public GameObject friendContent;
    public GameObject friendItemPrefab;
    public GameObject randomDialog;
    public GameObject dialogCanvas;
    public GameObject matchingDialog;

    public bool isNameChange = false;
    public bool isAvatorChange = false;

    [System.Serializable]
    public class ResponseItem {
        public int status { get; set; }
        public string msg { get; set; }
        public DataItem data { get; set; }
    }

    [System.Serializable]
    public class DataItem {
        public UserItem user { get; set; }
    }

    [System.Serializable]
    public class UserItem {
        public string user_id { get; set; }
        public string nickname { get; set; }
        public string avatar { get; set; }
        public int exp { get; set; }
        public int diamand { get; set; }
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
    #endregion

    #region Private Variables
    private bool acceptedInvitation = false;
    private bool polling = false;
    private InvitationItem invitation = null;
    private string roomId;
    #endregion

    void Start() {
        UpdateUserInfo();
        UpdateUserAvator();
        StartCoroutine(PollInvitation());
    }

    void Update() {
        if (isNameChange) {
            UpdateUserInfo();
        }
    }

    #region Private Methods
    private void UpdateUserInfo() {
        string url = "http://123.207.93.25:9001/user/";
        url += GlobalUserInfo.userInfo.user_id;
        StartCoroutine(GetUserInfo(url));
        isNameChange = false;
        detailManager.GetComponent<DetailManagerController>().isNicknameChange = false;
    }

    private void ShowUpUserInfo() {
        usernameGO.GetComponent<Text>().text = GlobalUserInfo.userInfo.nickname;
        diamondGO.GetComponent<Text>().text = GlobalUserInfo.userInfo.diamand.ToString();
        levelGO.GetComponent<Text>().text = "Lv." + Assets.Resources.Scripts.LevelCalculation.ExpToLevel(GlobalUserInfo.userInfo.exp).ToString();
    }

    private void UpdateUserAvator() {
        StartCoroutine(GetAvatar(GlobalUserInfo.userInfo.user_id, userFace.GetComponent<CircleImage>(), 100, 100));
    }

    private void OnRandomJoinRoom() {
        matchingDialog.SetActive(true);
    }
    #endregion

    #region Public Methods
    public void AcceptInvitation() {
        acceptedInvitation = true;
        dialogCanvas.SetActive(false);
        StartCoroutine(DeleteInvitation());
    }

    public void RejectInvitation() {
        acceptedInvitation = false;
        dialogCanvas.SetActive(false);
        StartCoroutine(DeleteInvitation());
    }

    public void OnOpenRoomButtonClick() {
        RoomOptions options = new RoomOptions() {
            MaxPlayers = 2
        };
        String roomStr = GlobalUserInfo.tokenInfo.account + DateTime.Now.ToFileTime().ToString();
        roomId = roomStr;
        PhotonNetwork.CreateRoom(roomStr, options, TypedLobby.Default);
    }

    public void OnOpenToolButtonClick() {
        StartCoroutine(FadeScene("ToolScene"));
    }

    public void OnRandomButtonClick() {
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnAcceptButtonClick() {
        randomDialog.SetActive(false);
    }

    public void OnHelpButtonClick() {
        ExplainCanvas.SetActive(true);
    }

    public void OnExplainCloseButtonClick() {
        ExplainCanvas.SetActive(false);
    }

    public void OnAvatorClick() {
        accountDetail.SetActive(true);
    }

    public void OnFriendClick() {
        friendCanvas.SetActive(true);
        StartCoroutine(GetFriendList());
    }

    public void OnRankClick() {
        rankCanvas.SetActive(true);
    }

    public void CloseFriendList() {
        friendCanvas.SetActive(false);
    }

    public void CloseRankCanvas() {
        rankCanvas.SetActive(false);
    }

    public void UpdateIsChangeName(DetailManagerController detail) {
        isNameChange = detail.isNicknameChange;
    }
    #endregion

    #region IEnmerator Methods
    IEnumerator GetUserInfo(string url) {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError) {
            Debug.Log(request.error);
        }
        else {
            var responseJson = JsonConvert.DeserializeObject<ResponseItem>(request.downloadHandler.text);
            if (responseJson.status == 0) {
                var userJson = responseJson.data.user;
                GlobalUserInfo.userInfo.nickname = userJson.nickname;
                GlobalUserInfo.userInfo.diamand = userJson.diamand;
                GlobalUserInfo.userInfo.exp = userJson.exp;
                GlobalUserInfo.userInfo.user_id = userJson.user_id;
                GlobalUserInfo.userInfo.avatar = userJson.avatar;
                ShowUpUserInfo();
            }
            else {
                Debug.Log(responseJson.msg);
            }
        }
    }

    IEnumerator GetAvatar(string userId, CircleImage img, int width, int height) {
        string url = "http://123.207.93.25:9001/user/" + userId + "/avatar";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError) {
            Debug.LogError(request.error);
        }
        else {
            img.sprite = GetSpriteFromBytes(request.downloadHandler.data, width, height);
        }
    }

    // 轮询查看邀请
    IEnumerator PollInvitation() {
        polling = true;
        string url = "http://123.207.93.25:9001/game/pollInvitation?token=" + GlobalUserInfo.tokenInfo.token;
        while (polling) {
            UnityWebRequest req = UnityWebRequest.Get(url);
            yield return req.SendWebRequest();

            if (req.isNetworkError || req.isHttpError) {
                Debug.LogError(req.downloadHandler.text);
            } else {
                var resp = JsonUtility.FromJson<InvitationResp>(req.downloadHandler.text);
                var invitationList = resp.data;
                if (invitationList.Length > 0) {
                    // 只取第一个邀请
                    invitation = invitationList[0];
                    // 弹窗显示是否接受邀请
                    dialogCanvas.SetActive(true);
                    // 停止轮询
                    Debug.Log("Got invitation, stopping coroutine");
                    polling = false;
                } else {
                    // 没有收到任何邀请，1s之后继续轮询
                    Debug.Log("Polling");
                    yield return new WaitForSeconds(1.0f);
                }
            }
        }
    }

    IEnumerator DeleteInvitation() {
        string url = "http://123.207.93.25:9001/game/deleteInvitation/" + GlobalUserInfo.userInfo.user_id;
        WWWForm form = new WWWForm();
        form.AddField("token", GlobalUserInfo.tokenInfo.token);
        var req = UnityWebRequest.Post(url, form);
        yield return req.SendWebRequest();
        // 拒绝了本次邀请，继续轮询
        if (!acceptedInvitation) {
            StartCoroutine(PollInvitation());
        }
        else {
            roomId = invitation.room_id;
            PhotonNetwork.JoinRoom(invitation.room_id);
        }
    }

    IEnumerator FadeScene(string sceneName) {
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(sceneName);
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
    #endregion

    #region Photon.PunBehaviour CallBacks
    // Join or Create room will call this method
    public override void OnJoinedRoom() {
        GlobalUserInfo.roomInfo.roomIndex = roomId;
        StartCoroutine(FadeScene("OpenRoomScene"));
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg) {
        matchingDialog.SetActive(false);
        randomDialog.SetActive(true);
        base.OnPhotonRandomJoinFailed(codeAndMsg);
    }
    #endregion

    // 图像从二进制流到精灵
    public static Sprite GetSpriteFromBytes(byte[] data, int width, int height) {
        Texture2D tex = new Texture2D(width, height);
        try {
            tex.LoadImage(data);
        }
        catch (Exception) {}
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
    }
}