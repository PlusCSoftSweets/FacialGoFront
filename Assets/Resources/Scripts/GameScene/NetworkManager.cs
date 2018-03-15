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
    // 初始化数量
    public int goldNumber;
    public float rangeMinZ;
    public float rangeMaxZ;
    public float rangeMinY;
    #endregion

    #region Private Variables
    private float[] rangeX = { -4.5f, -1.6f, 1.6f, 4.5f };
    private int nextSceneNumber = 0;
    private Transform mirror;
    private bool isAbort = false;
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
            if (HAHAController.localPlayerInstance == null)
            {
                PhotonNetwork.Instantiate(localPlayer.name, new Vector3(GetRandom(rangeX), 1.35f, -182f), Quaternion.identity, 0);
                InitSceneObject();
            }
            else
            {
                Debug.Log("We had create the object before!");
                RecoverPosition();
            }
        }
    }

    void Update()
    {
        //if (nextSceneNumber == Global.instance.currentScene)
        //{
        //    nextSceneNumber++;
        //    mirror = GameObject.Find("Mirror(" + Global.instance.currentScene + ")").transform;
        //    HAHAController.GetHaHaInstance().mirror = mirror;
        //}

        //if (Mathf.Abs(mirror.position.z - localPlayer.transform.position.z) < 5.0 && !isAbort)
        //{
        //    Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        //    HAHAController.GetHaHaInstance().isEnterMirror = true;
        //    Global.instance.playerPosition = localPlayer.transform.position + new Vector3(0, 0, 10);
        //    Global.instance.uiCameraPosition = uiCamera.position + new Vector3(0, 0, 10);
        //    Global.instance.mainCameraPosition = mainCamera.position + new Vector3(0, 0, 10);
        //    for (int i = 1; i <= 10; i++)
        //    {
        //        Debug.Log("find tree");
        //        Debug.Log("Tree (" + i + ")");
        //        var tree = GameObject.Find("Tree (" + i + ")");
        //        Debug.Log(tree);
        //        Global.instance.treesPosition[i - 1] = tree.transform.position + new Vector3(0, 0, 10);
        //    }
        //    Debug.Log("mark down position");
        //    isAbort = true;
        //}
    }

    #region Public Method
    // IPunObservable Implement
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
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
        //for (int i = 0; i < goldNumber; i++)
        //{
        //    float z = UnityEngine.Random.Range(rangeMinZ, rangeMaxZ);
        //    float x = GetRandom(rangeX);
        //    float y = rangeMinY;
        //    PhotonNetwork.InstantiateSceneObject("Gold", new Vector3(x, y, z), Quaternion.identity, 0, null);
        //}
    }

    private void RecoverPosition()
    {
        Debug.Log("recover positon");
        Debug.Log(Global.instance.playerPosition);
        localPlayer.transform.position = Global.instance.playerPosition;
        for (int i = 1; i <= 10; i++)
        {
            var tree = GameObject.Find("Tree (" + i + ")");
            tree.transform.position = Global.instance.treesPosition[i - 1];
        }
        mainCamera.position = Global.instance.mainCameraPosition;
        uiCamera.position = Global.instance.uiCameraPosition;
    }
    #endregion
}
