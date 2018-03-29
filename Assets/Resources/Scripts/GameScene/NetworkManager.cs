using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkManager : PunBehaviour, IPunObservable
{

    #region Public Static Variables
    public static NetworkManager instance;
    #endregion

    #region Public Variables
    public GameObject playerPrefab;
    public Transform mainCamera;
    public Transform uiCamera;
    public Transform[] trees;
    public GameObject bar;
    public GameObject mark;
    public CircleImage avator;

    // 初始化数量
    public int goldNumber;
    public int obstacleNumber;
    public float rangeMinZ;
    public float rangeMaxZ;
    public float rangeMinY;
    public bool isFailed = false;
    #endregion

    #region Private Variables
    private float[] rangeX = { -4.5f, -1.6f, 1.6f, 4.5f };
    private Transform mirror;
    private bool isAbort = false;
    private float markPosition = 0;
    private bool findMark = false;
    private GameObject[] coinGroup;
    private GameObject[] obstacleGroup;
    #endregion

    // Use this for initialization
    void Start()
    {
        instance = this;
        StartCoroutine(GetAvatar(GlobalUserInfo.userInfo.user_id, avator, 100, 100));
        if (playerPrefab == null)
        {
            Debug.Log("Error! No prefabs");
        }
        else
        {
            if (HAHAController.LocalPlayerInstance == null)
            {
                // 下落高度为2，防止玩家粘合
                PhotonNetwork.Instantiate(playerPrefab.name,
                                          new Vector3(GetRandom(rangeX), 3.35f, -182f),
                                          Quaternion.identity, 0);

                HAHAController.LocalPlayerInstance.name = "localHAHA";
                PhotonNetwork.Instantiate(mark.name, new Vector2(0, 0), Quaternion.identity, 0);
                MarkManager.LocalMarkInstance.name = "localMark";
                MarkManager.LocalMarkInstance.transform.parent = bar.transform;
                MarkManager.LocalMarkInstance.GetComponent<RectTransform>().localPosition = new Vector2(-300, 0);
                // 只有房主才能生成金币和障碍
                if (PhotonNetwork.isMasterClient)
                {
                    InitSceneObject();
                }
            }
            else
            {
                MarkManager.LocalMarkInstance = PhotonNetwork.Instantiate(mark.name, new Vector2(0, 0), Quaternion.identity, 0);
                MarkManager.LocalMarkInstance.name = "localMark";
                RecoverPosition();
            }
        }
    }

    void Update()
    {
        if (!findMark)
        {
            GameObject otherMark = GameObject.Find("mark(Clone)");
            if (otherMark != null)
            {
                Debug.Log("Find it");
                findMark = true;
                otherMark.transform.parent = bar.transform;
            }
            Debug.Log("not found");
        }
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

    public void GameOverChangeScene()
    {
        for (int i = 0; i < goldNumber; i++)
        {
            PhotonNetwork.Destroy(coinGroup[i]);
        }
        for (int i = 0; i < obstacleNumber; i++)
        {
            PhotonNetwork.Destroy(obstacleGroup[i]);
        }
        UnityEngine.Object.Destroy(Global.instance.gameObject);
        UnityEngine.Object.Destroy(FingerGestures.Instance.gameObject);
        PhotonNetwork.LeaveRoom();

        if (isFailed)
        {
            SceneManager.LoadScene("FailScene");
        }
        else
        {
            SceneManager.LoadScene("SuccessScene");
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
        CreateCoins();
        CreateObstacles();
    }

    private void RecoverPosition()
    {
        HAHAController.LocalPlayerInstance.transform.position = Global.instance.playerPosition;
        HAHAController.LocalPlayerInstance.GetComponent<Rigidbody>().useGravity = true;
        HAHAController.LocalPlayerInstance.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        for (int i = 0; i < 10; i++)
        {
            trees[i].position = Global.instance.treesPosition[i];
        }
        mainCamera.position = Global.instance.mainCameraPosition;
        uiCamera.position = Global.instance.uiCameraPosition;
        MarkManager.LocalMarkInstance.transform.parent = bar.transform;
        MarkManager.LocalMarkInstance.GetComponent<RectTransform>().localPosition = new Vector2(Global.instance.CalculateBarPosition(Global.instance.playerPosition.z), 0);
    }
    private void CreateCoins()
    {
        coinGroup = new GameObject[goldNumber];
        for (int i = 0; i < goldNumber; i++)
        {
            float z = UnityEngine.Random.Range(rangeMinZ, rangeMaxZ);
            float x = GetRandom(rangeX);
            float y = rangeMinY;
            coinGroup[i] = PhotonNetwork.InstantiateSceneObject("Gold", new Vector3(x, y, z), Quaternion.identity, 0, null);
        }
    }

    private void CreateObstacles()
    {
        obstacleGroup = new GameObject[obstacleNumber];
        for (int i = 0; i < obstacleNumber; i++)
        {
            float z = UnityEngine.Random.Range(rangeMinZ, rangeMaxZ);
            float x = GetRandom(rangeX);
            float y = rangeMinY;
            obstacleGroup[i] = PhotonNetwork.InstantiateSceneObject("ObstacleWrapper", new Vector3(x, y, z), Quaternion.identity, 0, null);
        }
    }
    #endregion

    #region IEnumerator Methods
    IEnumerator GetAvatar(string userId, CircleImage img, int width, int height)
    {
        string url = "http://123.207.93.25:9001/user/" + userId + "/avatar";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError(request.error);
            // 弹窗提示错误
        }
        else
        {
            img.sprite = MainSceneMangerController.GetSpriteFromBytes(request.downloadHandler.data, width, height);
            #endregion
        }
    }
}