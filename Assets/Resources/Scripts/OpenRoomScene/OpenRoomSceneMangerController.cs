using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenRoomSceneMangerController : Photon.PunBehaviour {

    #region Photon Messages
    public override void OnPhotonPlayerConnected(PhotonPlayer otherPlayer)
    {
        Debug.Log("OnPhotonPlayerConnected() " + otherPlayer.NickName); //如果你是正在连接的玩家则看不到
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient);
            
            // 如果达到人数就开始游戏
            if (PhotonNetwork.room.PlayerCount == PhotonNetwork.room.MaxPlayers)
            {
                LoadArena();
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
    #endregion

    #region Public Methods
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnInviteButtonClick()
    {
        // 邀请好友

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