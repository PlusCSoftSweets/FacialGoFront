using System;
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
				// float x = UnityEngine.Random.Range (initObj.rangeMinVec3.x, initObj.rangeMaxVec3.x),
				float y = UnityEngine.Random.Range (initObj.rangeMinVec3.y, initObj.rangeMaxVec3.y),
				z = UnityEngine.Random.Range (initObj.rangeMinVec3.z, initObj.rangeMaxVec3.z);

                float []arr = { -4.5f, -1.6f, 1.6f, 4.5f };
                float x = GetRandom(arr);

                Vector3 pos = new Vector3 (x, y, z);

				GameObject obj = pool.GetOne ();
				obj.transform.position = pos;
			}
		}
	}

    public float GetRandom(float[] arr) {
        System.Random ran = new System.Random();
        int n = ran.Next(arr.Length);
        return arr[n];
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