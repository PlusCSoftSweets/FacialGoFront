using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour {

	static Hashtable _GameObjectPools = new Hashtable();

	List<GameObject> pool = new List<GameObject>();
	List<GameObject> toFree = new List<GameObject>();
	List<GameObject> toAdd = new List<GameObject>();

	int curPosToGet = 0;


	public GameObject prefab;
	public int initialPoolSize = 8;
	public int maxPoolSize = -1;
	public string poolName;

	// May be overridden
	protected void initOneGameObject(GameObject obj) {
		obj.SetActive (false);
	}

	// May be overridden
	protected bool freeOneGameObject(GameObject obj) {
		obj.SetActive (false);
		return true;
	}

	// Use this for initialization
	void Start () {
		_GameObjectPools.Add (poolName, this);
		if (prefab == null) {
			Debug.LogError ("No prefab error! Will DESTROY myself.");
			Destroy (this);
		}
		for (int i = 0; i < initialPoolSize; ++i) {
			GameObject obj = Instantiate (prefab);
			initOneGameObject (obj);
			pool.Add (obj);
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < toFree.Count; ++i) {
			if (pool.IndexOf (toFree[i]) == -1) {
				Debug.LogError ("Object to free is not in the pool!");
			}
			freeOneGameObject (toFree[i]);
		}
		toFree.Clear ();
		for (int i = 0; i < toAdd.Count; ++i) {
			pool.Add (toAdd [i]);
		}
		toAdd.Clear ();
	}

	public GameObject GetOne() {
		int lastCur = curPosToGet;
		while (pool [curPosToGet].activeSelf) {
			curPosToGet = (curPosToGet + 1) % pool.Count;
			if (curPosToGet == lastCur) {
				if (maxPoolSize != -1 && pool.Count >= maxPoolSize) {
					Debug.LogError ("Pool FULL!");
					return null;
				}
				// Make new Object
				GameObject obj = Instantiate(prefab);
				initOneGameObject (obj);
				obj.SetActive (true);
				AddObject (obj);
				return obj;
			}
		}
		initOneGameObject (pool [curPosToGet]);
		pool [curPosToGet].SetActive (true);
		return pool [curPosToGet];
	}

	public void AddObject(GameObject obj) {
		toAdd.Add (obj);
	}

	public void FreeObject(GameObject obj) {
		toFree.Add (obj);
	}

	public static GameObjectPool FindPool(string name) {
		return (GameObjectPool)_GameObjectPools [name];
	}
}
