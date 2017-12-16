using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MirroController : MonoBehaviour {

    public GameObject player;
    public GameObject mirror;
    [SerializeField] private float dis;

    private bool isAbsort = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        dis = mirror.transform.position.z - player.transform.position.z;
        if (dis < 5.0 && !isAbsort) {
            isAbsort = true;
            HAHAController.getHaHaInstance().isEnterMirror = true;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            Global.instance.coinNumber = HAHAController.getHaHaInstance().count;
            Global.instance.currentScene++;
            SceneManager.LoadScene("MirrorScene");
        }
    }

}
