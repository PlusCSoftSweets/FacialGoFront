using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// 初始化物品
	public InitObject[] inits;
    public Transform playerPrefab;
    public Transform mainCamera;
    public Transform uiCamera;

    // 初始化数量
    public int goldNumber;
    public float rangeMinZ;
    public float rangeMaxZ;
    public float rangeMinY;
    private float[] rangeX = { -4.5f, -1.6f, 1.6f, 4.5f };

    // 场景
    public SpriteRenderer background;
    public SpriteRenderer race;
    public Camera raceCamera;

	void Init() {
        //foreach (InitObject initObj in inits) {
        //	GameObjectPool pool = GameObjectPool.FindPool (initObj.poolName);
        //	for (int i = 0; i < initObj.count; ++i) {
        //		// float x = UnityEngine.Random.Range (initObj.rangeMinVec3.x, initObj.rangeMaxVec3.x),
        //		float y = UnityEngine.Random.Range (initObj.rangeMinVec3.y, initObj.rangeMaxVec3.y),
        //		z = UnityEngine.Random.Range (initObj.rangeMinVec3.z, initObj.rangeMaxVec3.z);

        //              float []arr = { -4.5f, -1.6f, 1.6f, 4.5f };
        //              float x = GetRandom(arr);

        //              Vector3 pos = new Vector3 (x, y, z);

        //		GameObject obj = pool.GetOne ();
        //		obj.transform.position = pos;
        //	}
        //}

        // 玩家离开房间时不会摧毁对象
        PhotonNetwork.autoCleanUpPlayerObjects = false;
        PhotonNetwork.automaticallySyncScene = true;

        for (int i = 0; i < goldNumber; i++)
        {
            float z = UnityEngine.Random.Range(rangeMinZ, rangeMaxZ);
            float x = GetRandom(rangeX);
            float y = rangeMinY;
            PhotonNetwork.InstantiateSceneObject("Gold", new Vector3(x, y, z), Quaternion.identity, 0, null);
        }

        // 恢复之前的位置
        if (Global.instance.playerPosition != new Vector3(0,0,0))
        {
            Debug.Log("recover positon");
            playerPrefab.position = Global.instance.playerPosition;
            for (int i = 1; i <= 10; i++)
            {
                var tree = GameObject.Find("Tree (" + i + ")");
                tree.transform.position = Global.instance.treesPosition[i-1];
            }
            mainCamera.position = Global.instance.mainCameraPosition;
            uiCamera.position = Global.instance.uiCameraPosition;
        }
	}

    // 初始化场景
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
        else if (Global.instance.currentScene == 3)
        {
            setScene("Sprites/Backgound/村庄/村庄-背景",
                     "Sprites/Backgound/村庄/村庄-跑道",
                     "Sprites/Backgound/村庄/村庄-树木",
                     0.651f, 0.424f, 0.388f);
        }
        else {
            setScene("Sprites/Backgound/森林/森林-背景",
                     "Sprites/Backgound/森林/森林-跑道",
                     "Sprites/Backgound/森林/森林-树木",
                     0.196f, 0.608f, 0.749f);
        }
    }

    // 设置场景
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

    public void Awake() {
        if (!PhotonNetwork.connected)
        {
            SceneManager.LoadScene("MainScene");
            return;
        }
        // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
        PhotonNetwork.Instantiate(this.playerPrefab.name, transform.position, Quaternion.identity, 0);
    }
}

[System.Serializable]
public class InitObject {
	public int count;
	public Vector3 rangeMinVec3;
	public Vector3 rangeMaxVec3;
	public string poolName;
};