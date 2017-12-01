using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCollision : MonoBehaviour {

    public GameObject player;  // 玩家
    public GameObject mainCamera;  // 主摄像机
    public GameObject UICamera;    // UI摄像机
    public GameObject[] hidenTree; // 树

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Trigger: " + collider.name + "进入！");
        if (collider.name.Equals("Tree (15)") || collider.name.Equals("Tree (14)"))
        {
            player     = GameObject.FindGameObjectWithTag("Player");
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            UICamera   = GameObject.FindGameObjectWithTag("BackGroundCamera");
            hidenTree  = GameObject.FindGameObjectsWithTag("HidenTree");
            player.transform.SetPositionAndRotation(
                new Vector3(player.transform.position.x, player.transform.position.y, -182f),
                player.transform.rotation);
            mainCamera.transform.SetPositionAndRotation(
                new Vector3(0f, 8f, -200),
                mainCamera.transform.rotation);
            UICamera.transform.SetPositionAndRotation(
                new Vector3(0f, 0f, -38.5f),
                UICamera.transform.rotation);
            foreach (GameObject tree in hidenTree)
                tree.layer = 5;
        }
        else if (collider.tag.Equals("HidenTree"))
            collider.gameObject.layer = 0;
        
    }
}
