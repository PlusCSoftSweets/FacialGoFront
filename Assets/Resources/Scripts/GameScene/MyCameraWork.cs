using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCameraWork : MonoBehaviour {

    #region Private Variables
    private float Dir;                   //摄像机于要跟随物体的距离
    private bool isFollowing = false;
    Transform cameraTransform;
    #endregion

    #region Public Variables
    [Tooltip("如果预设的组件正在被 Photon Netword 实例化把这个属性设置为false，并在需要的时候手动调用 OnStartFollowing()")]
    public bool followOnStart = false;
    #endregion

    // Use this for initialization
    void Start()
    {
        if (followOnStart)
        {
            OnStartFollowing();
            //获取到摄像机于要跟随物体之间的距离
            Dir = cameraTransform.position.z - transform.position.z;
        }
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (cameraTransform == null && isFollowing)
        {
            OnStartFollowing();
        }
        if (isFollowing)
        {
            //摄像机的位置
            cameraTransform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, transform.position.z + Dir);
        }
    }

    #region Public Method
    public void OnStartFollowing()
    {
        cameraTransform = GameObject.Find("Main Camera").transform;
        isFollowing = true;
    }
    #endregion
}