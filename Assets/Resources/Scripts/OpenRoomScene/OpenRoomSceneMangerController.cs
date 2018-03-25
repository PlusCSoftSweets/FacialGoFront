using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using ExitGames.UtilityScripts;

public class OpenRoomSceneMangerController : Photon.PunBehaviour {

    #region Public Variables
    public GameObject friendCanvas;
    public GameObject friendContent;
    public GameObject friendItemPrefab;
    public GameObject[] buttonPlayer;
    #endregion

    #region Private Variables
    private List<string> playerNameList = new List<string>();
    #endregion

    // 在启动的时候注册监听器
    void OnEnable() {
        PhotonNetwork.OnEventCall += this.OnEventCalled;
    }

    void Start() {
        if (!PhotonNetwork.inRoom) {
            StartCoroutine(LoadLastScene());
        }
        else {
            InitButtonListener();
            GetPlayersList();
        }
    }

    // 取消监听器
    void OnDisable() {
        PhotonNetwork.OnEventCall -= this.OnEventCalled;
    }

    #region Photon Messages
    public override void OnPhotonPlayerConnected(PhotonPlayer otherPlayer) {
        Debug.Log("OnPhotonPlayerConnected() " + otherPlayer.NickName);
        GetPlayersList();
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
        Debug.Log("OnPhotonPlayerDisconnected() " + otherPlayer.NickName);
        InitButtonListener();
        GetPlayersList();
    }
    #endregion

    #region Public Methods
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnInviteButtonClick() {
        friendCanvas.SetActive(true);
        StartCoroutine(GetFriendList());
    }

    public void CloseFriendList() {
        friendCanvas.SetActive(false);
    }

    public void OnBackButtonClick() {
        LeaveRoom();
        StartCoroutine(LoadLastScene());
    }

    public void OnStartButtonClick() {
        if (PhotonNetwork.isMasterClient) {
            byte evCode = 0;
            byte content = 0;
            bool reliable = true;
            if (PhotonNetwork.RaiseEvent(evCode, content, reliable, null)) {
                LoadGameArena();
            }
        }
    }
    #endregion

    #region Private Methods
    // Raise Event响应事件
    private void OnEventCalled(byte eventCode, object content, int senderid) {
        if (eventCode == 0) {
            if ((byte)content == 0) {
                LoadGameArena();
            }
        }
    }

    private void LoadGameArena() {
        StartCoroutine(LoadSingelModelScene());
    }

    private void GetPlayersList() {
        int Index = PhotonNetwork.room.PlayerCount - 1;
        Debug.Log(Index);
        PhotonPlayer player = PhotonNetwork.masterClient;
        for (int i = 0; i <= Index; i++) {
            string playerName = player.NickName;
            playerNameList.Add(playerName);
            buttonPlayer[i].SetActive(true);
            buttonPlayer[i].GetComponentInParent<Button>().enabled = false;
            StartCoroutine(GetAvatar(playerName, buttonPlayer[i].GetComponentInChildren<CircleImage>(), 140, 140));
            player = player.GetNext();
        }
    }

    private void InitButtonListener() {
        foreach (GameObject btn in buttonPlayer) {
            btn.GetComponentInParent<Button>().enabled = true;
            btn.GetComponent<CircleImage>().sprite = null;
        }
    }
    #endregion

    #region IEnumerator Methods
    IEnumerator LoadSingelModelScene() {
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        PhotonNetwork.LoadLevel("SingelModelScene");
    }

    IEnumerator LoadLastScene() {
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("MainScene");
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
        }
        else {
            // 清理当前的好友
            foreach (Transform chlid in friendContent.transform) {
                Destroy(chlid.gameObject);
            }
            Debug.Log("Get friend list success!");
            var json = JsonUtility.FromJson<MainSceneMangerController.FriendListItem>(req.downloadHandler.text);
            foreach (var user in json.data) {
                GameObject oneFriend = Instantiate(friendItemPrefab, friendContent.transform);
                oneFriend.GetComponentInChildren<Text>().text = user.nickname;
                MainSceneMangerController.UserItem userItem = new MainSceneMangerController.UserItem();
                userItem.user_id = user.user_id;
                userItem.nickname = user.nickname;
                userItem.avatar = user.avatar;
                userItem.exp = user.exp;
                UserInfo userInfo = oneFriend.GetComponent<UserInfo>();
                userInfo.userItem = userItem;
                userInfo.OnClick += (info) => {
                    Debug.Log(info.userItem.nickname);
                    CloseFriendList();
                    StartCoroutine(PostInvitation(info.userItem.user_id, PhotonNetwork.room.Name));
                };
                // 获取头像
                StartCoroutine(GetAvatar(user.user_id, oneFriend.GetComponentInChildren<CircleImage>(), 150, 150));
            }
        }
    }

    IEnumerator PostInvitation(string invitee_id, string room_id) {
        WWWForm form = new WWWForm();
        form.AddField("token", GlobalUserInfo.tokenInfo.token);
        form.AddField("invitee_id", invitee_id);
        form.AddField("room_id", room_id);

        UnityWebRequest req = UnityWebRequest.Post("http://123.207.93.25:9001/game/inviteToRoom", form);
        yield return req.SendWebRequest();

        if (req.isNetworkError || req.isHttpError) {
            Debug.LogError(req.downloadHandler.text);
        }
        else {
            // TODO: 弹窗提示
            Debug.Log("Invitation sended");
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
            img.sprite = MainSceneMangerController.GetSpriteFromBytes(request.downloadHandler.data, width, height);
        }
    }
    #endregion
}