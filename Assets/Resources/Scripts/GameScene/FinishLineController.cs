using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineController : Photon.PunBehaviour {

    [SerializeField]
    private float dis;


    #region Public Variables
    public GameObject gameSceneManager;
    #endregion


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "localHAHA")
        {
            HAHAController.GetHaHaInstance().isFinish = true;
            photonView.RPC("SetPlayerResult", PhotonTargets.Others);
            photonView.RPC("GameOverCalled", PhotonTargets.All);
        }
    }

    #region Private Methods
    [PunRPC]
    private void SetPlayerResult()
    {
        gameSceneManager.GetComponent<NetworkManager>().isFailed = true;
    }

    [PunRPC]
    private void GameOverCalled()
    {
        gameSceneManager.GetComponent<NetworkManager>().GameOverChangeScene();
    }
    #endregion
}
