using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OpenRoomSceneMangerController : Photon.PunBehaviour {

    public GameObject friendCanvas;
    public GameObject friendContent;
    public GameObject friendItemPrefab;

    bool isCreatedRoom = false;
    void Awake()
    {
        PhotonNetwork.OnEventCall += this.OnEvent;
    }

    void Start() {
        // 开房
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        String roomStr = GlobalUserInfo.tokenInfo.account + DateTime.Now.ToFileTime().ToString();
        PhotonNetwork.CreateRoom(roomStr, options, TypedLobby.Default);
    }

    #region Photon Messages
    private void OnEvent(byte eventcode, object content, int senderid)
    {
        if (eventcode == 0)
        {
            PhotonPlayer sender = PhotonPlayer.Find(senderid);
            byte response = (byte)content;
            if (response == 0)
            {
                Debug.Log("Refuse");
                // 弹窗，您的好友拒绝邀请
            }
            else if(response == 1)
            {
                Debug.Log("Agree");
                LoadArena();
            }
        }
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer otherPlayer)
    {
        Debug.Log("OnPhotonPlayerConnected() " + otherPlayer.NickName); //如果你是正在连接的玩家则看不到
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient);
            
            // 如果达到人数就开始游戏
            if (PhotonNetwork.room.PlayerCount == PhotonNetwork.room.MaxPlayers)
            {
                //LoadArena();
            }
        }
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debug.Log("OnPhotonPlayerDisconnected() " + otherPlayer.NickName);
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("OnPhotonPlayerDisConnected isMasterClient " + PhotonNetwork.isMasterClient); //在 OnPhotonPlayerDisconnected 之前调用
            // LoadArena();
        }
    }

    public override void OnCreatedRoom() {
        isCreatedRoom = true;
    }


    #endregion

    #region Public Methods
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnInviteButtonClick()
    {
        if (!isCreatedRoom) {
            Debug.Log("Creating room, wait for finishing");
            return;
        }
        friendCanvas.SetActive(true);
        StartCoroutine(GetFriendList());
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
        } else {
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

    public void CloseFriendList() {
        friendCanvas.SetActive(false);
    }

    public void OnBackButtonClick()
    {
        Debug.Log("Back Button Click");
        LeaveRoom();
        StartCoroutine(LoadLastScene());
    }

    public void OnStartButtonClick()
    {
        Debug.Log("Start Button Click");
        LoadArena();
    }
    #endregion

    #region Private Methods //私有方法区域
    // 只有房主才可以加载场景
    private void LoadArena()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            return;
        }
        Debug.Log("PhotonNetwork : Loading Level : " + PhotonNetwork.room.PlayerCount);
        StartCoroutine(LoadSingelModelScene());
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
    #endregion

}