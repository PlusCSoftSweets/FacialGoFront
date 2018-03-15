using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HAHAController : Photon.MonoBehaviour
{
    #region Private Variables
    // 移动相关
    private bool isMove = false;              // 判断正在左右移动
    private bool isGround = true;             // 判断在地面
    private Vector3 lastPosition;             // 记录上一个位置
    private Vector3 moveVerticalVec = new Vector3();    // 纵向移动的速度
    private Vector3 moveHorizontalVec = new Vector3();  // 横向移动的速度

    // 道具相关
    private float pauseTimer;                 // pause计时器
    private bool isMagnet = false;            // 是否碰到磁铁
    private float MagnetTime = 10f;           // 磁铁生效时间
    private new SpriteRenderer renderer;      // 磁铁发光

    // 玩家相关
    private Rigidbody hahaRB;                 // haha刚体

    // 声音
    private AudioSource[] m_MyAudioSource = new AudioSource[2];
    #endregion

    #region Public Variables
    public bool isPausing = false;            // 判断是否被暂停
    public bool isReverse = false;            // 判断是否转置
    public bool isEnterMirror = false;        // 判断是否进入魔镜
    public float accelerateSpeed = 20f;       // 加速度
    public float forwardSpeed = 30f;          // 前进速度
    public Transform mirror;                  // 最近的镜子
    #endregion

    #region Static Variables
    private static HAHAController playerInstance;  // 控制器实例
    public static GameObject localPlayerInstance;  // 实例
    #endregion

    #region Static Method
    static public HAHAController GetHaHaInstance()
    {
        return playerInstance;
    }  // 获取控制器实例
    #endregion

    void Start()
    {
        InitController();
        SetUpMineCamera();
    }

    void Update()
    {
        if (photonView.isMine == false && PhotonNetwork.connected == true) return;
        if (isMagnet) MagnetWork();
    }

    void FixedUpdate()
    {
        if (photonView.isMine == false && PhotonNetwork.connected == true) return;

        if (!isEnterMirror)
            MoveForward();
        else
            moveToMirror();

        // 左右判断函数
        if ((System.Math.Abs(hahaRB.position.x - 1.6f) < 0.20f ||
            System.Math.Abs(hahaRB.position.x + 4.5f) < 0.20f ||
            System.Math.Abs(hahaRB.position.x - 4.5f) < 0.20f ||
            System.Math.Abs(hahaRB.position.x + 1.6f) < 0.20f) &&
            System.Math.Abs(hahaRB.position.x - lastPosition.x) > 2 && isMove)
        {
            moveHorizontalVec -= moveHorizontalVec;
            isMove = false;
        }

        // Pause计时
        if (isPausing)
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer < 0)
                isPausing = false;
        }
    }

    #region Private Method
    private void InitController()
    {
        playerInstance = this;
        hahaRB = GetComponent<Rigidbody>();
        lastPosition = transform.position;
        localPlayerInstance = this.gameObject;
        renderer = GetComponent<SpriteRenderer>();
        m_MyAudioSource = GetComponents<AudioSource>();
        GetComponent<SwipeRecognizer>().EventMessageTarget = GameObject.Find("OnSwipeEvent");
    }

    private void SetUpMineCamera()
    {
        MyCameraWork myCamera = GetComponent<MyCameraWork>();
        if (myCamera != null)
        {
            Debug.Log("myCamera");
            if (photonView.isMine)
            {
                Debug.Log("isMine");
                myCamera.followOnStart = true;
            }
        }
    }

    private void MagnetWork()
    {
        //检测以玩家为球心半径是10的范围内的所有的带有碰撞器的游戏对象
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, 10);
        foreach (var item in colliders)
        {
            //如果是金币
            if (item.tag.Equals("Gold"))
            {
                //让金币的开始移动
                item.GetComponent<CoinController>().isCanMove = true;
            }
        }
        MagnetTime -= Time.deltaTime;
        if (MagnetTime <= 0)
        {
            isMagnet = false;
            MagnetTime = 10f;
            renderer.material = (Material)Resources.Load("Materials/init_haha");
        }
    }

    private void MoveForward()
    {
        Vector3 tempVelocity = hahaRB.velocity;
        tempVelocity.x = moveHorizontalVec.x;
        // 如果没有暂停，且速度不足，加速
        if (!isPausing)
        {
            if (hahaRB.velocity.z - forwardSpeed > accelerateSpeed * Time.deltaTime)
            {
                tempVelocity.z -= accelerateSpeed * Time.deltaTime;
            }
            else if (hahaRB.velocity.z - forwardSpeed < accelerateSpeed * Time.deltaTime)
            {
                tempVelocity.z += accelerateSpeed * Time.deltaTime;
            }
        }
        else if (isPausing)
        {
            forwardSpeed = 30f;
            accelerateSpeed = 20f;
            tempVelocity.z = 0f;
        }
        hahaRB.velocity = tempVelocity;
    }

    private void OnCollisionEnter(Collision collider)
    {
        if(collider.gameObject.name.Equals("Forest-Race"))
        {
            isGround = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Gold"))
        {
            other.gameObject.SetActive(false);
            Global.instance.coinNumber = Global.instance.coinNumber + 1;
            m_MyAudioSource[0].Play();
        }
        if (other.tag.Equals("Magnet"))
        {
            isMagnet = true;
            renderer.material = (Material)Resources.Load("Materials/haha");
            m_MyAudioSource[1].Play();
        }
    }

    private void leftMoving()
    {
        if (transform.position.x > -4.0f && !isMove)
        {
            isMove = true;
            lastPosition = transform.position;                                      // 更新我当前位置的坐标
            Vector3 LocalPos = transform.position;                                  // 物体所处的世界坐标向量
            Vector3 LocalForward = transform.TransformPoint(Vector3.left * 7f);     // 物体前方距离为speed的位置的世界坐标向量
            Vector3 VecSpeed = LocalForward - LocalPos;                             // 物体自身Vector3.forward * speed的世界坐标向量
            moveHorizontalVec = new Vector3(VecSpeed.x, VecSpeed.y, VecSpeed.z);
        }
    }

    private void rightMoving()
    {
        if (transform.position.x < 4.0f && !isMove)
        {
            isMove = true;
            lastPosition = transform.position;                                      // 更新我当前位置的坐标
            Vector3 LocalPos = transform.position;                                  // 物体所处的世界坐标向量
            Vector3 LocalForward = transform.TransformPoint(Vector3.right * 7f);    // 物体前方距离为speed的位置的世界坐标向量
            Vector3 VecSpeed = LocalForward - LocalPos;                             // 物体自身Vector3.forward * speed的世界坐标向量
            moveHorizontalVec = new Vector3(VecSpeed.x, VecSpeed.y, VecSpeed.z);
        }
    }

    private void jump()
    {
        if (isGround && !isMove)
        {
            isGround = false;
            moveVerticalVec = new Vector3(0, 10f, 0);
            hahaRB.velocity += moveVerticalVec;
            hahaRB.AddForce(Vector3.up * 100);
        }
    }
    #endregion

    #region Public Method
    // 收到手势脚本控制
    public void Move(string direction) {
        if (direction.Equals("Left")) {
            if (!isReverse)
                leftMoving();
            else
                rightMoving();  
        }
        else if (direction.Equals("Right")) {
            if (!isReverse)
                rightMoving();
            else
                leftMoving();
        }
        else if (direction.Equals("Up")) {
            jump();
        }
    }

    // 触碰雪糕桶
    public void pauseForMilliSeconds(float seconds)
    {
        Vector3 tempVelocity = hahaRB.velocity;
        tempVelocity.z = 0;
        hahaRB.velocity = tempVelocity;
        isPausing = true;
        pauseTimer = seconds;
    }


    #endregion





    
    public Vector3 enterMirror(UnityEngine.Transform mirrorTran) {
        Vector3 LocalPos = transform.position;                                  // 物体所处的世界坐标向量
        Vector3 LocalForward = mirrorTran.position;                    // 镜子所在地
        Vector3 VecSpeed = LocalForward - LocalPos;                             // 物体自身Vector3.forward * speed的世界坐标向量
        return new Vector3(VecSpeed.x, VecSpeed.y, VecSpeed.z);
    }

    void moveToMirror() {
        //hahaRB.velocity = enterMirror(mirror);
    }
}