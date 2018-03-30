using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HAHAController : Photon.PunBehaviour {
    #region Private Variables
    // 移动相关
    private bool isMove = false;              // 判断正在左右移动
    private bool isGround = true;             // 判断在地面
    private Vector3 lastPosition;             // 记录上一个位置
    private Vector3 moveVerticalVec = new Vector3();    // 纵向移动的速度
    private Vector3 moveHorizontalVec = new Vector3();  // 横向移动的速度

    // 道具相关
    private float pauseTimer;                 // pause计时器
    private new SpriteRenderer renderer;      // 磁铁发光

    // 玩家相关
    private Rigidbody hahaRB;                 // haha刚体
    private bool isGameOn = false;            // 游戏是否开始
    private float waittingTime = 0;           // 等待游戏开始时间

    // 声音
    private AudioSource[] m_MyAudioSource = new AudioSource[1];

    // 游戏相关
    
    #endregion

    #region Public Variables
    public bool isPausing = false;            // 判断是否被暂停
    public bool isReverse = false;            // 判断是否转置
    public bool isMagnet = false;             // 判断是否正在磁铁状态
    public float accelerateSpeed = 20f;       // 加速度
    public float forwardSpeed = 30f;          // 前进速度
    public float magnetTime = 0;              // 磁铁生效时间
    public Material playerInit;

    // 魔镜相关
    public Transform mirror;                  // 最近的镜子
    public Transform castle;                  // 城堡
    public bool isEnterMirror = false;        // 判断是否进入魔镜

    // 多人游戏相关
    public bool isOtherReady = false;         // 对方是否加入房间
    public bool isSinglePlayer = false;      // 是否是单人游戏

    // 游戏结束
    public bool isFinish = false;
    public Transform finishLine;
    public bool isEnterCastle = false;        // 判断是否进入城堡
    #endregion

    #region Static Variables
    private static HAHAController playerInstance;  // 控制器实例
    public static GameObject LocalPlayerInstance;  // 当前玩家实例，不销毁
    #endregion

    #region Static Method
    static public HAHAController GetHaHaInstance() {
        return playerInstance;
    }  // 获取控制器实例
    #endregion

    // 1. 设置LocalPlayerInstance实例，并且不销毁
    // 2. 注册监听事件
    void Awake() {
        // 设置当前玩家实例，并且不销毁
        if (photonView.isMine) {
            LocalPlayerInstance = this.gameObject;
        }
        DontDestroyOnLoad(this.gameObject);

        // 注册监听事件
        PhotonNetwork.OnEventCall += this.OnListenGameOn;
        byte evCode = 2;
        byte content = 1;
        bool reliable = true;
        PhotonNetwork.RaiseEvent(evCode, content, reliable, null);
    }

    // 1. 初始化控制器
    // 2. 设置跟随的相机
    // 3. 判断是单人游戏还是多人游戏
    void Start() {
        InitController();
        SetUpMyCamera();
        if (PhotonNetwork.room.PlayerCount == 1) {
            isSinglePlayer = true;
        } else {
            isSinglePlayer = false;
        }
    }

    // 1. 判断游戏开始
    // 2. 判断磁铁是否起作用
    void Update() {
        if (photonView.isMine == false && PhotonNetwork.connected == true) return;
        if ((PhotonNetwork.isMasterClient && isOtherReady && !isGameOn) || isSinglePlayer) {
            StateGameOn();
        }
        if (!isGameOn) return;
        if (isMagnet) MagnetWork();
    }

    // 1. 移动判断，是前进还是进入魔镜
    // 2. 触碰雪糕筒暂停
    // 只作用于本地玩家的函数
    // 1. 判断触碰的左右
    void FixedUpdate() {
        if (!isGameOn) return;
        // 以下操作只针对本地玩家
        if (photonView.isMine == false && PhotonNetwork.connected == true) return;
        if (!isEnterMirror && !isEnterCastle)
            MoveForward();
        else if (isEnterMirror)
            MoveToMirror();
        else if (isEnterCastle)
            MoveToCastle();

        if (isPausing) {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer < 0) isPausing = false;
        }

        MoveLeftOrRight();
    }

    #region Private Method
    private void InitController() {
        hahaRB = GetComponent<Rigidbody>();
        lastPosition = transform.position;
        renderer = GetComponent<SpriteRenderer>();
        m_MyAudioSource = GetComponents<AudioSource>();
        GetComponent<SwipeRecognizer>().EventMessageTarget = GameObject.Find("OnSwipeEvent");

        // 静态实例只保存LocalPlayer的
        if (photonView.isMine == false && PhotonNetwork.connected == true) return;
        playerInstance = this;
    }

    private void SetUpMyCamera() {
        MyCameraWork myCamera = GetComponent<MyCameraWork>();
        if (myCamera != null) {
            if (photonView.isMine) {
                myCamera.followOnStart = true;
            }
        }
    }

    private void StateGameOn() {
        waittingTime += Time.deltaTime;
        if (waittingTime > 3) {
            byte evCode = 3;
            byte content = 1;
            bool reliable = true;
            isGameOn = true;
            PhotonNetwork.RaiseEvent(evCode, content, reliable, null);
        }
    }

    private void MagnetWork() {
        //检测以玩家为球心半径是10的范围内的所有的带有碰撞器的游戏对象
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, 10);
        foreach (var item in colliders) {
            if (item.tag.Equals("Gold")) {
                item.GetComponent<CoinController>().isCanMove = true;
            }
        }
    }

    private void MoveForward() {
        Vector3 tempVelocity = hahaRB.velocity;
        tempVelocity.x = moveHorizontalVec.x;
        // 如果没有暂停，且速度不足，加速
        if (!isPausing) {
            if (hahaRB.velocity.z - forwardSpeed > accelerateSpeed * Time.deltaTime) {
                tempVelocity.z -= accelerateSpeed * Time.deltaTime;
            }
            else if (hahaRB.velocity.z - forwardSpeed < accelerateSpeed * Time.deltaTime) {
                tempVelocity.z += accelerateSpeed * Time.deltaTime;
            }
        }
        else if (isPausing) {
            forwardSpeed = 30f;
            accelerateSpeed = 20f;
            tempVelocity.z = 0f;
        }
        hahaRB.velocity = tempVelocity;
    }

    private void MoveToMirror() {
        hahaRB.velocity = EnterObject(mirror);
    }

    private Vector3 EnterObject(UnityEngine.Transform target)
    {
        Vector3 LocalPos = transform.position;                         // 物体所处的世界坐标向量
        Vector3 LocalForward = target.position;                        // 目标所在地
        Vector3 VecSpeed = LocalForward - LocalPos;                    // 物体自身Vector3.forward * speed的世界坐标向量
        return new Vector3(VecSpeed.x, VecSpeed.y, VecSpeed.z);
    }

    private void MoveLeftOrRight() {
        if ((System.Math.Abs(hahaRB.position.x - 1.6f) < 0.20f ||
            System.Math.Abs(hahaRB.position.x + 4.5f) < 0.20f ||
            System.Math.Abs(hahaRB.position.x - 4.5f) < 0.20f ||
            System.Math.Abs(hahaRB.position.x + 1.6f) < 0.20f) &&
            System.Math.Abs(hahaRB.position.x - lastPosition.x) > 2 && isMove) {
            moveHorizontalVec -= moveHorizontalVec;
            isMove = false;
        }
    }

    private void MoveToCastle()
    {
        hahaRB.velocity = EnterObject(castle);
    }

    private void LeftMoving() {
        if (transform.position.x > -4.0f && !isMove) {
            isMove = true;
            lastPosition = transform.position;                                      // 更新我当前位置的坐标
            Vector3 LocalPos = transform.position;                                  // 物体所处的世界坐标向量
            Vector3 LocalForward = transform.TransformPoint(Vector3.left * 7f);     // 物体前方距离为speed的位置的世界坐标向量
            Vector3 VecSpeed = LocalForward - LocalPos;                             // 物体自身Vector3.forward * speed的世界坐标向量
            moveHorizontalVec = new Vector3(VecSpeed.x, VecSpeed.y, VecSpeed.z);
        }
    }

    private void RightMoving() {
        if (transform.position.x < 4.0f && !isMove) {
            isMove = true;
            lastPosition = transform.position;                                      // 更新我当前位置的坐标
            Vector3 LocalPos = transform.position;                                  // 物体所处的世界坐标向量
            Vector3 LocalForward = transform.TransformPoint(Vector3.right * 7f);    // 物体前方距离为speed的位置的世界坐标向量
            Vector3 VecSpeed = LocalForward - LocalPos;                             // 物体自身Vector3.forward * speed的世界坐标向量
            moveHorizontalVec = new Vector3(VecSpeed.x, VecSpeed.y, VecSpeed.z);
        }
    }

    private void Jump() {
        if (isGround && !isMove) {
            isGround = false;
            moveVerticalVec = new Vector3(0, 10f, 0);
            hahaRB.velocity += moveVerticalVec;
            hahaRB.AddForce(Vector3.up * 100);
        }
    }

    private void OnListenGameOn(byte eventcode, object content, int senderid) {
        if (eventcode == 2) {
            if ((byte)content == 1) {
                TellOtherReady();
            }
        }
        else if (eventcode == 3) {
            if ((byte) content == 1) {
                SetGameOn();
            }
        }
    }

    private void TellOtherReady() {
        isOtherReady = true;
    }

    private void SetGameOn() {
        isGameOn = true;
    }

    // 1. 与地面的碰撞检测
    private void OnCollisionEnter(Collision collider) {
        if (collider.gameObject.name.Equals("Forest-Race")) {
            isGround = true;
        }
    }

    // 1. 与金币的碰撞检测
    // 2. 与镜子前的墙壁的碰撞检测
    private void OnTriggerEnter(Collider other) {  
        if (other.tag.Equals("Gold")) {
            Global.instance.coinNumber = Global.instance.coinNumber + 1;
            m_MyAudioSource[0].Play();
        }
        else if (other.name.Equals("Wall")) {
            if (photonView.isMine) {
                mirror = other.transform;
                mirror.position += new Vector3(0, 0, 7f);
                other.gameObject.SetActive(false);
                isEnterMirror = true;
            } 
        }
        else if (other.name.Equals("FinishLine")) {
            if (photonView.isMine)
            {
                castle = other.transform;
                castle.position += new Vector3(0f, 0f, 7f);
                isEnterCastle = true;
                other.gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region Public Method
    // 收到手势脚本控制，只有LocalPlayer才受脚本控制
    public void Move(string direction) {
        if (photonView.isMine == false && PhotonNetwork.connected == true) return;
        if (direction.Equals("Left")) {
            if (!isReverse)
                LeftMoving();
            else
                RightMoving();
        }
        else if (direction.Equals("Right")) {
            if (!isReverse)
                RightMoving();
            else
                LeftMoving();
        }
        else if (direction.Equals("Up")) {
            Jump();
        }
    }

    // 触碰雪糕桶
    public void PauseForMilliSeconds(float seconds) {
        Vector3 tempVelocity = hahaRB.velocity;
        tempVelocity.z = 0;
        hahaRB.velocity = tempVelocity;
        isPausing = true;
        pauseTimer = seconds;
    }

    public void InitData() {
        // 移动相关
        isMove = false;              // 判断正在左右移动
        isGround = true;             // 判断在地面
        isPausing = false;            // 判断是否被暂停
        isReverse = false;            // 判断是否转置
        isMagnet = false;             // 判断是否正在磁铁状态
        accelerateSpeed = 20f;       // 加速度
        forwardSpeed = 30f;          // 前进速度
        magnetTime = 0;              // 磁铁生效时间
        LocalPlayerInstance.GetComponent<SpriteRenderer>().material = playerInit;
    }
    #endregion
}