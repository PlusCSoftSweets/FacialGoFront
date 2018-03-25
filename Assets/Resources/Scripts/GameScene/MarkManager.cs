using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkManager : Photon.PunBehaviour {

    #region Private Variables
    private Transform player;
    #endregion

    #region Public Static Variables
    public static GameObject LocalMarkInstance;
    #endregion
    
    void Awake() {
        if (photonView.isMine) {
            LocalMarkInstance = this.gameObject;
        }
        DontDestroyOnLoad(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        if (player == null) player = HAHAController.GetHaHaInstance().transform;
        this.transform.localPosition = new Vector2(
            Global.instance.CalculateBarPosition(player.position.z),
            this.transform.localPosition.y);
	}
}
