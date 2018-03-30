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
            if (HAHAController.GetHaHaInstance().isSinglePlayer) {
                GameOverCalled();
            }
            else {
                PhotonNetwork.RaiseEvent(20, PhotonNetwork.time, true, null);
            }
            
        }
    }

    #region Private Methods
    private void OnEvent(byte eventcode, object content, int senderid) {
        if (eventcode == 20) {
            Debug.Log(HAHAController.GetHaHaInstance().gameObject.name + "LOSSER CALLED");
            if (HAHAController.GetHaHaInstance().isFinish) {
                if (PhotonNetwork.time < (double)content) {
                    PhotonNetwork.RaiseEvent(21, null, true, null);
                    GameOverCalled();
                }
                else {
                    PhotonNetwork.RaiseEvent(22, null, true, null);
                    SetPlayerResult();
                    GameOverCalled();
                }
            } 
            else {
                PhotonNetwork.RaiseEvent(22, null, true, null);
                SetPlayerResult();
                GameOverCalled();
            }
        }
        else if (eventcode == 21) {
            SetPlayerResult();
            GameOverCalled();
        }
        else if (eventcode == 22) {
            GameOverCalled();
        }
    }

    private void SetPlayerResult() {
        NetworkManager.instance.isFailed = true;
    }

    private void GameOverCalled() {
        NetworkManager.instance.GameOverChangeScene();
        //gameSceneManager.GetComponent<NetworkManager>().GameOverChangeScene();
    }
    #endregion
}
