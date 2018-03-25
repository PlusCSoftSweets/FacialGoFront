using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineController : MonoBehaviour {

    [SerializeField]
    private float dis;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "localHAHA") {
            HAHAController.GetHaHaInstance().isFinish = true;
        }
    }
}
