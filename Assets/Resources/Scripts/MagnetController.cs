using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetController : MonoBehaviour {

	GameObjectPool pool;

	// Use this for initialization
	void Start () {
		pool = GameObjectPool.FindPool ("Magnet");
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(15, 0, 45) * Time.deltaTime);
    }

	void OnTriggerEnter(Collider collider) {
		if (collider.tag == "Player") {
			pool.FreeObject (gameObject);
		}
	}
}
