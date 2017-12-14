using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransButtonClick : MonoBehaviour {

    private bool isClick = false;
    private float time = 0;
	
	// Update is called once per frame
	void Update () {
        if (time < 0.001 && isClick) {
            cancelReverse();
            isClick = false;
        }
        else if (isClick)
            time -= Time.deltaTime;

	}

    public void buttonClick() {
        HAHAController player = HAHAController.getHaHaInstance();
        player.isReverse = true;
        time = 10;
        isClick = true;
    }

    private void cancelReverse() {
        HAHAController player = HAHAController.getHaHaInstance();
        player.isReverse = false;
    }
}
