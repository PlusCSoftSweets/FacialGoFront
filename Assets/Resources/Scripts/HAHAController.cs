using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HAHAController : MonoBehaviour
{
    private bool isMove;                      // 判断正在左右移动
    private bool isGround;                    // 判断在地面
	public bool isPausing = false;            // 判断是否被暂停
	private float pauseTimer;                 // pause计时器
	private Rigidbody hahaRB;                 // haha实体
    private Vector3 lastPosition;             // 记录上一个位置
    private Vector3 moveVerticalVec;          // 纵向移动的速度
    private Vector3 moveHorizontalVec;        // 横向移动的速度

    public static HAHAController playerHaha;  // 玩家实例
    public Text  goldNum;                     // 显示金币数量的文本框
    private int count;                        // 金币数量
    private bool isMagnet = false;            // 是否碰到磁铁
    float MagnetTime = 10f;                   // 磁铁生效时间

    public float accelerateSpeed = 20f;       // 加速度
	public float forwardSpeed = 30f;          // 前进速度

    static public HAHAController getHaHaInstance()
    {
        return playerHaha;
    }

    /*
     * 初始化
     */
    void Start()
    {
        isGround          = true;
        isMove            = false;
        hahaRB            = GetComponent<Rigidbody>();
        lastPosition      = transform.position;
        moveVerticalVec   = new Vector3(0, 0, 0);
        moveHorizontalVec = new Vector3(0, 0, 0);
        playerHaha        = this;
        count = 0;
        SetCountText();
    }

    void moveForward()
    {
        //Vector3 LocalPos = transform.position;                                  // 物体所处的世界坐标向量
        //Vector3 LocalForward = transform.TransformPoint(Vector3.forward * 5f);    // 物体前方距离为speed的位置的世界坐标向量
        //Vector3 VecSpeed = LocalForward - LocalPos;                             // 物体自身Vector3.forward * speed的世界坐标向量
        //moveHorizontalVec = new Vector3(moveHorizontalVec.x, hahaRB.velocity.y, VecSpeed.z);
        // Debug.Log("haha" + isPausing);
        Vector3 tempVelocity = hahaRB.velocity;
		tempVelocity.x = moveHorizontalVec.x;
		// 如果没有暂停，且速度不足，加速
		if (!isPausing) {
            if (hahaRB.velocity.z - forwardSpeed > accelerateSpeed * Time.deltaTime)
            {
                tempVelocity.z -= accelerateSpeed * Time.deltaTime;
                // Debug.Log(accelerateSpeed);
                // Debug.Log(Time.deltaTime);
                // Debug.Log(tempVelocity.z);
            }
            else if (hahaRB.velocity.z - forwardSpeed < accelerateSpeed * Time.deltaTime)
            {
                tempVelocity.z += accelerateSpeed * Time.deltaTime;
            }
            else
            {
            }
        }
        else if (isPausing)
        {
            forwardSpeed = 30f;
            accelerateSpeed = 20f;
            tempVelocity.z = 0f;
        }
		hahaRB.velocity = tempVelocity;
		// Debug.Log (hahaRB.velocity);
    }

    void Update()
    {
        if (isMagnet)
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
            if(MagnetTime <= 0)
            {
                isMagnet = false;
                MagnetTime = 10f;
            }
        }
    }
    /*
     * 更新函数
     */
    void FixedUpdate()
    {
        moveForward();
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
		if (isPausing) {
			pauseTimer -= Time.deltaTime;
			if (pauseTimer < 0)
				isPausing = false;
		}

        // 上下判断函数
        //if ()
        //{
        //    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //    hahaRB.velocity = new Vector3(hahaRB.velocity.x, 0, hahaRB.velocity.z);
        //    hahaRB.useGravity = false;
        //}
    }

    void OnCollisionEnter(Collision collider)
    {
        isGround = true;
        // Debug.Log(isGround);
    }

    /*
     * 移动函数
     */
    public void Move(string direction)
    {
        // Debug.Log(direction + transform.position.x + hahaRB.position.x + isMove);
        if (direction.Equals("Left"))
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
        else if (direction.Equals("Right"))
        {
            if (transform.position.x < 4.0f && !isMove)
            {
                // Debug.Log("Right");
                isMove = true;
                lastPosition = transform.position;                                      // 更新我当前位置的坐标
                Vector3 LocalPos = transform.position;                                  // 物体所处的世界坐标向量
                Vector3 LocalForward = transform.TransformPoint(Vector3.right * 7f);    // 物体前方距离为speed的位置的世界坐标向量
                Vector3 VecSpeed = LocalForward - LocalPos;                             // 物体自身Vector3.forward * speed的世界坐标向量
                moveHorizontalVec = new Vector3(VecSpeed.x, VecSpeed.y, VecSpeed.z);
            }
        }
        else if (direction.Equals("Up"))
        {
            if (isGround && !isMove)
            {
                // Debug.Log("Up");
                isGround = false;
                moveVerticalVec = new Vector3(0, 10f, 0);
                hahaRB.velocity += moveVerticalVec;
                hahaRB.AddForce(Vector3.up * 100);
            }
        }
    }

	public void pauseForMilliSeconds(float seconds) {
		Vector3 tempVelocity = hahaRB.velocity;
		tempVelocity.z = 0;
		hahaRB.velocity = tempVelocity;
		isPausing = true;
		pauseTimer = seconds;
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gold"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
        if (other.tag.Equals("Magnet"))
        {
            //设置玩家可以吸取周围的金币
            isMagnet = true;
            //销毁吸铁石
            //Destroy(other.gameObject);
        }
    }

    void SetCountText()
    {
        goldNum.text = count.ToString();
    }
}