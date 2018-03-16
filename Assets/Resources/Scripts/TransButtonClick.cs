using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransButtonClick : Photon.PunBehaviour {

    #region Public Variables
    public Text showHint;
    #endregion

    #region Private Variables
    private float rotateTime = 0;
    private bool isClick = false;
    private GameObject rotator;
    private GameObject player;
    private const int rotatingSpeed = 15;
    private AudioSource[] m_MyAudioSource = new AudioSource[1];
    #endregion

    void Start()
    {
        m_MyAudioSource = GetComponents<AudioSource>();
    }

    void Update () {
        if (rotateTime < 0.001 && isClick) {
            CancelReverse();
            isClick = false;
            Destroy(rotator);
        }
        else if (isClick)
        {
            rotateTime -= Time.deltaTime;
        }
	}

    void LateUpdate()
    {
        if (rotator != null)
        {
            rotator.transform.position = new Vector3(
                player.transform.position.x,
                player.transform.position.y + 1.8f, player.transform.position.z);
            rotator.transform.Rotate(0 * Time.deltaTime,
                0 * Time.deltaTime,
                -90 * Time.deltaTime * rotatingSpeed);
        }
    }

    #region Public Methods
    public void OnReverseCalled()
    {
        if (Global.instance.coinNumber < 5)
        {
            showHint.text = "金币不足！";
            return;
        }
        Global.instance.coinNumber -= 5;
        photonView.RPC("CarryOutReverse", PhotonTargets.Others);
    }
    #endregion

    #region Private Methods
    [PunRPC]
    private void CarryOutReverse()
    {
        if (player == null) player = HAHAController.GetHaHaInstance().gameObject;
        if (!player.GetComponent<HAHAController>().isReverse)
        {
            isClick = true;
            rotateTime = 10;
            player.GetComponent<HAHAController>().isReverse = true;
            rotator = Instantiate<GameObject>(Resources.Load("Prefabs/Rotator") as GameObject);
            m_MyAudioSource[0].Play();
        }
    }

    private void CancelReverse() {
        player.GetComponent<HAHAController>().isReverse = false;
    }
    #endregion
}
