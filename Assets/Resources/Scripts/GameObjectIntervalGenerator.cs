﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectIntervalGenerator : MonoBehaviour {

	static Hashtable _GameObjectGenerators = new Hashtable();

	float curTimer = 0f;
	GameObjectPool pool;

	public float generateInterval = 5f;
	public Vector3 rangeMinVec3 = Vector3.zero;
	public Vector3 rangeMaxVec3 = Vector3.zero;
	public string generatorName;
	public string poolName;

	// Use this for initialization
	void Start () {
		_GameObjectGenerators.Add (generatorName, this);
		pool = GameObjectPool.FindPool (poolName);
		Init ();
	}
	
	// Update is called once per frame
	void Update () {

		curTimer += Time.deltaTime;
		if (curTimer >= generateInterval) {
			GenerateOne ();
			curTimer = 0f;
		}
	}

	protected void Init() {

	}

	public void GenerateOne(Vector3 pos) {
		GameObject obj = pool.GetOne ();
		Debug.Log (obj + " 坐标: " + pos);
		obj.transform.position = pos;
		Debug.Log (obj.transform.position);
	}

	public void GenerateOne() {
		float x = Random.Range (rangeMinVec3.x, rangeMaxVec3.x),
		      y = Random.Range (rangeMinVec3.y, rangeMaxVec3.y),
		      z = Random.Range (rangeMinVec3.z, rangeMaxVec3.z);
		GenerateOne (new Vector3 (x, y, z));
	}

	public static GameObjectIntervalGenerator FindGenerator(string name) {
		return (GameObjectIntervalGenerator)_GameObjectGenerators [name];
	}
}
