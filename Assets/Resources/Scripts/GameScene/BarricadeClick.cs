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

    void Awake() {
        PhotonNetwork.OnEventCall += OnEvent;
    }

    #region Public Methods
    public void OnBarricadeButtonClick() {
        if (Global.instance.coinNumber < 5) {
            showHint.text = "金币不足！";
            return;
        }
        Global.instance.coinNumber -= 5;
        PhotonNetwork.RaiseEvent(10, "BARRICADE", true, null);
    }
    #endregion

    #region Private Methods
    private void OnEvent(byte eventcode, object content, int senderid) {
        if (eventcode == 10) {
            if (((string)content).Equals("BARRICADE")) {
                LayUpBarricade();
            }
        }
    }

    private void LayUpBarricade() {
        if (player == null) player = HAHAController.GetHaHaInstance().gameObject;
        Vector3 targetPosition = player.transform.position + new Vector3(0, -player.transform.position.y, 3);
        Global.instance.obstacleGroup.Add(PhotonNetwork.InstantiateSceneObject("ObstacleWrapper", targetPosition, Quaternion.identity, 0, null));
    }
    #endregion
}
