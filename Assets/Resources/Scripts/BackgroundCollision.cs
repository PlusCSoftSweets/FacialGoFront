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
        if (collider.name.Equals("Tree (3)"))
        {
            hidenTree = GameObject.FindGameObjectsWithTag("Tree");
            foreach (GameObject tree in hidenTree)
                tree.transform.SetPositionAndRotation(
                    new Vector3(tree.transform.position.x, tree.transform.position.y, tree.transform.position.z + 50),
                    tree.transform.rotation);
        }
        else if (collider.tag.Equals("HidenTree")) {
            collider.gameObject.layer = 0;
        }
        else if (collider.tag.Equals("Obstacle")) {
            collider.gameObject.layer = 0;
        }
        else if (collider.tag.Equals("Gold")) {
            collider.gameObject.layer = 0;
        }
        else if (collider.tag.Equals("Magnet")) {
            collider.gameObject.layer = 0;
        }
        else if (collider.tag.Equals("Mirror")) {
            collider.gameObject.layer = 0;
        }
     }
}
