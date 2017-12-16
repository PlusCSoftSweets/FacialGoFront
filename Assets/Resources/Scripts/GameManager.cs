using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// 初始化物品
	public InitObject[] inits;

    // 场景
    public SpriteRenderer background;
    public SpriteRenderer race;
    public Camera raceCamera;

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

    void InitScene() {
        if (Global.instance.currentScene == 0) {
            setScene("Sprites/Backgound/森林/森林-背景",
                     "Sprites/Backgound/森林/森林-跑道",
                     "Sprites/Backgound/森林/森林-树木",
                     0.196f, 0.608f, 0.749f);
        }
        else if (Global.instance.currentScene == 1) {
            setScene("Sprites/Backgound/沙漠/沙漠-背景",
                     "Sprites/Backgound/沙漠/沙漠-跑道",
                     "Sprites/Backgound/沙漠/沙漠-仙人掌右",
                     0.957f, 0.780f, 0.514f);
        }
        else if (Global.instance.currentScene == 2) {
            setScene("Sprites/Backgound/雪地/雪地-背景",
                     "Sprites/Backgound/雪地/雪地-跑道",
                     "Sprites/Backgound/雪地/雪地-树木",
                     0.424f, 0.596f, 0.749f);
        }
    }

    private void setScene(String bg, String ra, String tr, float r, float g, float b) {
        background.sprite = Resources.Load(bg, typeof(Sprite)) as Sprite;
        race.sprite = Resources.Load(ra, typeof(Sprite)) as Sprite;
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        foreach (GameObject tree in trees) {
            SpriteRenderer temp = tree.GetComponent<SpriteRenderer>();
            temp.sprite = Resources.Load(tr, typeof(Sprite)) as Sprite;
        }
        raceCamera.backgroundColor = new Color(r, g, b);
    }

    public float GetRandom(float[] arr) {
        System.Random ran = new System.Random();
        int n = ran.Next(arr.Length);
        return arr[n];
    }

    // Use this for initialization
    void Start () {
		Init ();
        InitScene();
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