using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineController : Photon.PunBehaviour {

    #region Public Variables
    public GameObject gameSceneManager;
    #endregion

    void Awake() {
        PhotonNetwork.OnEventCall += OnEvent;
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "localHAHA") {
            HAHAController.GetHaHaInstance().isFinish = true;
            if (PhotonNetwork.RaiseEvent(11, "FINISH", true, null)) {
                PhotonNetwork.RaiseEvent(1, new int[] { int.Parse(GlobalUserInfo.userInfo.user_id), int.Parse(GlobalUserInfo.userInfo.user_id)}, true, null);
                GameOverCalled();
            }
        }
    }

    #region Private Methods
    private void OnEvent(byte eventcode, object content, int senderid) {
        if (eventcode == 11) {
            if (((string)content).Equals("FINISH")) {
                Debug.Log(HAHAController.GetHaHaInstance().gameObject.name);
                SetPlayerResult();
                GameOverCalled();
            }
        }
    }

    private void SetPlayerResult() {
        gameSceneManager.GetComponent<NetworkManager>().isFailed = true;
    }

    private void GameOverCalled() {
        gameSceneManager.GetComponent<NetworkManager>().GameOverChangeScene();
    }
    #endregion
}
