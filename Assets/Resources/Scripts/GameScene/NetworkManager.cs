using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class NetworkManager : PunBehaviour, IPunObservable {

    #region Public Static Variables
    public static NetworkManager instance;
    #endregion

    #region Public Variables
    public GameObject localPlayer;
    public Transform mainCamera;
    public Transform uiCamera;
    public Transform[] trees;
    public GameObject bar;
    public GameObject mark;

    // 初始化数量
    public int goldNumber;
    public int obstacleNumber;
    public float rangeMinZ;
    public float rangeMaxZ;
    public float rangeMinY;
    #endregion

    #region Private Variables
    private float[] rangeX = { -4.5f, -1.6f, 1.6f, 4.5f };
    private int nextSceneNumber = 0;
    private Transform mirror;
    private bool isAbort = false;
    private float markPosition = 0;
    #endregion

    // Use this for initialization
    void Start () {
        instance = this;
        if (localPlayer == null)
        {
            Debug.Log("Error! No prefabs");
        }
        else
        {
            if (!Global.instance.isCreateBefore)
            {   
                Global.instance.isCreateBefore = true;
                localPlayer = PhotonNetwork.Instantiate(localPlayer.name, new Vector3(GetRandom(rangeX), 1.35f, -182f), Quaternion.identity, 0);
                mark = PhotonNetwork.Instantiate(mark.name, new Vector2(0, 0), Quaternion.identity, 0);
                localPlayer.name = "localHAHA";
                InitSceneObject();
            }
            else
            {
                localPlayer = PhotonNetwork.Instantiate(localPlayer.name, new Vector3(GetRandom(rangeX), 1.35f, -182f), Quaternion.identity, 0);
                mark = PhotonNetwork.Instantiate(mark.name, new Vector2(0, 0), Quaternion.identity, 0);
                localPlayer.name = "localHAHA";
                RecoverPosition();
            }
        }
    }

    void Update()
    {
        
    }

    #region Public Method
    // IPunObservable Implement
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 同步mark的位置
        if (stream.isWriting)
        {
            stream.SendNext(markPosition);
        }
        else
        {
            this.markPosition = (float)stream.ReceiveNext();
        }
    }
    #endregion

    #region Private Methed
    // 随机选择
    private float GetRandom(float[] arr)
    {
        System.Random ran = new System.Random();
        int n = ran.Next(arr.Length);
        return arr[n];
    }

    private void InitSceneObject()
    {
        mark.transform.parent = bar.transform;
        mark.GetComponent<RectTransform>().localPosition = new Vector2(-300, 0);
        CreateCoins();
        CreateObstacles();
    }

    private void RecoverPosition()
    {
        Debug.Log("recover positon");
        localPlayer.transform.position = Global.instance.playerPosition;
        for (int i = 0; i < 10; i++)
        {
            trees[i].position = Global.instance.treesPosition[i];
        }
        mainCamera.position = Global.instance.mainCameraPosition;
        uiCamera.position = Global.instance.uiCameraPosition;
        mark.transform.parent = bar.transform;
        mark.GetComponent<RectTransform>().localPosition = new Vector2(Global.instance.CalculateBarPosition(Global.instance.playerPosition.z), 0);
    }

    private void CreateCoins()
    {
        for (int i = 0; i < goldNumber; i++)
        {
            float z = UnityEngine.Random.Range(rangeMinZ, rangeMaxZ);
            float x = GetRandom(rangeX);
            float y = rangeMinY;
            DontDestroyOnLoad(PhotonNetwork.InstantiateSceneObject("Gold", new Vector3(x, y, z), Quaternion.identity, 0, null));
        }
    }

    private void CreateObstacles()
    {
        for (int i = 0; i < obstacleNumber; i++)
        {
            float z = UnityEngine.Random.Range(rangeMinZ, rangeMaxZ);
            float x = GetRandom(rangeX);
            float y = rangeMinY;
            DontDestroyOnLoad(PhotonNetwork.InstantiateSceneObject("ObstacleWrapper", new Vector3(x, y, z), Quaternion.identity, 0, null));
        }
    }
    #endregion
}
