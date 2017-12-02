using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// 初始化物品
	public InitObject[] inits;

	void Init() {
		foreach (InitObject initObj in inits) {
			GameObjectPool pool = GameObjectPool.FindPool (initObj.poolName);

			for (int i = 0; i < initObj.count; ++i) {
				float x = Random.Range (initObj.rangeMinVec3.x, initObj.rangeMaxVec3.x),
				y = Random.Range (initObj.rangeMinVec3.y, initObj.rangeMaxVec3.y),
				z = Random.Range (initObj.rangeMinVec3.z, initObj.rangeMaxVec3.z);
				Vector3 pos = new Vector3 (x, y, z);

				GameObject obj = pool.GetOne ();
				obj.transform.position = pos;
			}
		}
	}

	// Use this for initialization
	void Start () {
		Init ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

[System.Serializable]
public class InitObject {
	public int count;
	public Vector3 rangeMinVec3;
	public Vector3 rangeMaxVec3;
	public string poolName;
};