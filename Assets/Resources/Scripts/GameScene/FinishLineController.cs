using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineController : Photon.PunBehaviour {

    #region Public Variables
    public GameObject gameSceneManager;
    #endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("called finishline controller");
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
