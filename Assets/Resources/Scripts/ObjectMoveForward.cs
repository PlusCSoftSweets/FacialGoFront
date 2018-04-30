using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMoveForward : MonoBehaviour {

    public GameObject obj;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void LateUpdate () {
		obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z + 20*Time.deltaTime);
    }
}
