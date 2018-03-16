using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorClick : MonoBehaviour {

    #region Public Variable
    public GameObject player;
    public Text moveText;
    public Transform[] mirrors;
    public Transform[] trees;
    #endregion

    #region Private Variable
    private bool isClick = false;
    private float CreatTime = 3f;
    [SerializeField] private float randomMin;
    [SerializeField] private float randomMax;
    private AudioSource[] m_MyAudioSource = new AudioSource[1];
    #endregion

    void Start () {
        m_MyAudioSource = GetComponents<AudioSource>();
    }
	
	void Update () {
        if (isClick)
        {
            CreatTime -= Time.deltaTime;
            if (CreatTime <= 0)
            {
                moveText.text = "";
                isClick = false;
                CreatTime = 3f;
            }
        }
    }

#region Public Methods
    public void ButtonClick()
    {
        if (player == null) player = HAHAController.GetHaHaInstance().gameObject;
        if (Global.instance.coinNumber < 5)
        {
            moveText.text = "金币不足！";
            return;
        }
        Global.instance.coinNumber -= 5;
        CheckTheState();
    }
    #endregion

    #region Private Methods
    private void CheckTheState()
    {
        if (player == null) player = HAHAController.GetHaHaInstance().gameObject;
        isClick = true;
        m_MyAudioSource[0].Play();

        float[] range = FindValidRange(randomMin, randomMax);
        float dis = Random.Range(range[0], range[1]);
        ForwardOrBackward(dis);
        ReactToPlayer(dis);
    }

    private float[] FindValidRange(float min, float max)
    {
        float[] range = new float[2];
        range[0] = min;
        range[1] = max;

        float[] arr = new float[mirrors.Length];
        for (int i = 0; i < mirrors.Length; i++)
        {
            arr[i] = mirrors[i].position.z - player.transform.position.z;
            Debug.Log("diff: " + arr[i].ToString());
            if (arr[i] < 0)
            {
                if (arr[i] > range[0])
                {
                    range[0] = arr[i];
                }
            }
            else
            {
                if (arr[i] < range[1])
                {
                    range[1] = arr[i];
                }
            }
                
            Debug.Log(i.ToString() + " " + range[0]);
            Debug.Log(i.ToString() + " " + range[1]);
        }

        // 安全距离
        range[0] += 10;
        range[1] -= 10;
        Debug.Log(range[0]);
        Debug.Log(range[1]);
        return range;
    }

    private void ForwardOrBackward(float dis)
    {
        Vector3 target = Vector3.zero;
        if (player.transform.position.z + dis < -200)
        {
            dis = -200 - player.transform.position.z;
            target = new Vector3(0, 0, dis);
        }
        else
            target = new Vector3(0, 0, dis);

        player.transform.position += target;
        foreach (Transform tree in trees)
            tree.position += target;
    }

    private void ReactToPlayer(float dis)
    {
        if (dis > 0)
            moveText.text = "你前进了" + (int)dis + "米！";
        else
            moveText.text = "你后退了" + (int)(-dis) + "米！";
    }
    #endregion
}
