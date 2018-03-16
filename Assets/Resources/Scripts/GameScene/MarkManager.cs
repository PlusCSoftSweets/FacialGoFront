using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkManager : Photon.PunBehaviour {

    #region Private Variables
    private Transform player;
    #endregion

    // Use this for initialization
    void Start() {
	}
	
	// Update is called once per frame
	void Update () {
        if (player == null) player = HAHAController.GetHaHaInstance().transform;
        this.transform.localPosition = new Vector2(
            Global.instance.CalculateBarPosition(player.position.z),
            this.transform.localPosition.y);
	}
}
