using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class CoinController : PunBehaviour
{
    GameObject target;
    public bool isCanMove = false;
    public float speed = 50;

    void Update()
    {
        if (isCanMove)
        {
            if (target == null) target = HAHAController.GetHaHaInstance().gameObject;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            //PhotonNetwork.Destroy(gameObject);
            PhotonNetwork.RPC(photonView, "SetActive", PhotonTargets.All, false, null);
        }
    }

    [PunRPC]
    private void SetActive()
    {
        this.gameObject.SetActive(false);
    }
}