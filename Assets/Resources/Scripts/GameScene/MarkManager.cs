using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkManager : Photon.PunBehaviour {

    #region Private Variables
    private Transform player;
    #endregion

    #region Static
    public static GameObject LocalMarkInstance;
    public static GameObject OtherMarkInstance;
    #endregion

    void Awake() {
        if (photonView.isMine)
            LocalMarkInstance = this.gameObject;
        else
            OtherMarkInstance = this.gameObject;
    }
    // Update is called once per frame
    void Update () {
        if (!photonView.isMine) return;
        if (player == null) player = HAHAController.GetHaHaInstance().transform;
        this.transform.localPosition = new Vector2(
            Global.instance.CalculateBarPosition(player.position.z),
            this.transform.localPosition.y);
	}
}
