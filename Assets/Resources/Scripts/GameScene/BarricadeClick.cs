using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarricadeClick : Photon.PunBehaviour {

    #region Public Variables
    public Text showHint;
    #endregion

    #region Private Variables
    private GameObject barricade;
    private GameObject player;
    #endregion

    #region Public Methods
    public void OnBarricadeButtonClick()
    {
        if (Global.instance.coinNumber < 5)
        {
            showHint.text = "金币不足！";
            return;
        }
        Global.instance.coinNumber -= 5;
        photonView.RPC("LayUpBarricade", PhotonTargets.Others);
    }
    #endregion

    #region Private Methods
    [PunRPC]
    private void LayUpBarricade()
    {
        if (player == null) player = HAHAController.GetHaHaInstance().gameObject;
        Vector3 targetPosition = player.transform.position + new Vector3(0, 0, 3);
        DontDestroyOnLoad(PhotonNetwork.InstantiateSceneObject("ObstacleWrapper", targetPosition, Quaternion.identity, 0, null));
    }
    #endregion
}
