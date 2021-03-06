﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using OpenCVForUnity;

public class MirrorSceneManager : MonoBehaviour
{

    public GameObject FacialGameObject;
    public GameObject MirrorMidGameObject;
    public GameObject WitchGameObject;
    public GameObject PercentGameObject;
    public WebCamController WebcamCtl;
    public GameObject PassedOrFailedLabel;
    public Texture[] Faces;

    private int CurFaceIndex = 0;
    private int random_face_index;

    public float FacialMoveSpeed = 500.0f;
    public float SampleInterval = 5.0f;
    public float TryInterval = 5.0f;
    [Range(0.0f, 1.0f)]
    public float PassPercent = 0.6f;

    private GUIStyle buttonStyle;
    private Vector3 FacialGameObjectOriginPos;
    private float SampleTime;
    private float TryTime;
    double percent = 0.0;
    int frac = 0;

    protected WebCamTexture webCamTexture;
    protected WebCamDevice webCamDevice;
    AudioSource[] m_MyAudioSource = new AudioSource[3];

    enum FacialState
    {
        Stopped,
        Moving,
        Passed,
        Failed,
        Trying
    };

    FacialState facialState = FacialState.Stopped;

    public void Awake()
    {
        buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 70;
        FacialGameObjectOriginPos = FacialGameObject.transform.position;
        FacialGameObject.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {
        SampleTime = Time.fixedTime;
        StartFaceMove();
        m_MyAudioSource = GetComponents<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (facialState)
        {
            case FacialState.Moving:
                // Move Facial
                Vector3 curPos = FacialGameObject.transform.position;
                if (Vector3.Distance(curPos, MirrorMidGameObject.transform.position) < float.Epsilon)
                {
                    //facialState = FacialState.Stopped;
                    if (percent > PassPercent)
                    {
                        facialState = FacialState.Passed;

                        // for testing
                        StartCoroutine(WaitAndNextFace());
                        // SceneManager.LoadScene("SuccessScene");
                    }
                    else
                    {
                        facialState = FacialState.Trying;
                        TryTime = Time.fixedTime;
                        // SceneManager.LoadScene("FailScene");
                    }
                }
                FacialGameObject.transform.position =
                    Vector3.MoveTowards(curPos, MirrorMidGameObject.transform.position, Time.deltaTime * FacialMoveSpeed);
                break;
        }

        if (facialState == FacialState.Moving || facialState == FacialState.Trying)
        {
            if (Time.fixedTime - SampleTime > SampleInterval)
            {
                StartCoroutine(CheckMatch());
                SampleTime = Time.fixedTime;
            }
            if (facialState == FacialState.Trying && Time.fixedTime - TryTime > TryInterval)
            {
                // Stop trying
                facialState = FacialState.Failed;
                StartCoroutine(WaitAndNextFace());
            }
        }

        // GUI
        if (facialState == FacialState.Passed)
        {
            if (PassedOrFailedLabel.activeSelf == false)
            {
                PassedOrFailedLabel.GetComponent<UnityEngine.UI.Text>().text = "PASSED!";
                PassedOrFailedLabel.SetActive(true);
                m_MyAudioSource[1].Play();
            }
        }
        else if (facialState == FacialState.Failed)
        {
            if (PassedOrFailedLabel.activeSelf == false)
            {
                PassedOrFailedLabel.GetComponent<UnityEngine.UI.Text>().text = "FAILED!";
                PassedOrFailedLabel.SetActive(true);
                //m_MyAudioSource[2].Play();
            }
        }
        else if (PassedOrFailedLabel.activeSelf == true)
        {
            PassedOrFailedLabel.SetActive(false);
        }
    }

    void OnGUI()
    {
    }

    IEnumerator CheckMatch()
    {
        Texture2D texture = WebcamCtl.Snapshot();
        m_MyAudioSource[2].Play();
        int face_number = random_face_index;

        // TODO: EncodeTo can be only called in main thread, consider a way to make it in other thread
        byte[] imageBytes = texture.EncodeToPNG();
        //Debug.Log(faceMat);
        Mat faceMat = new Mat(texture.height, texture.width, CvType.CV_8UC4);
        Mat grayMat = new Mat(texture.height, texture.width, CvType.CV_8UC1);
        Utils.texture2DToMat(texture, faceMat);
        Utils.texture2DToMat(texture, grayMat);


        frac++;
        percent = FaceDiscr.GetInstance().Face_Getpercent(face_number, faceMat, grayMat, frac);
        PercentGameObject.GetComponent<UnityEngine.UI.Text>().text = ((int)(percent * 100)).ToString() + "%";
        if (percent > PassPercent)
        {
            facialState = FacialState.Passed;
            StartCoroutine(WaitAndNextFace());
        }
        /*
        var thread = UnityThreadHelper.CreateThread(() => {
            // TODO: Use the real interface to cal percent
            percent = FaceDiscr.GetInstance().Face_Getpercent(face_number, faceMat, grayMat);
            UnityThreadHelper.Dispatcher.Dispatch(() => {
                PercentGameObject.GetComponent<UnityEngine.UI.Text>().text = ((int)(percent * 100)).ToString() + "%";
                if (percent > PassPercent)
                {
                    facialState = FacialState.Passed;
                    StartCoroutine(WaitAndNextFace());
                }
            });
        });
        thread.Start();*/
        yield return null;
    }

    IEnumerator WaitAndNextFace()
    {
        yield return new WaitForSeconds(1);
        StartFaceMove();
    }

    void StartFaceMove()
    {
        string front_face_haar = Utils.getFilePath("haarcascade_frontalface_alt2.xml");
        Debug.Log(front_face_haar);
        if (CurFaceIndex >= 1)
        {
            Debug.Log("No next face");
            // SceneManager.LoadScene("SingelModelScene");
            PhotonNetwork.LoadLevel("SingelModelScene");

            return;
        }
        random_face_index = Random.Range(0, 4);
        FacialGameObject.GetComponent<RawImage>().texture = Faces[random_face_index];
        CurFaceIndex++;
        // Facial fly out
        if (facialState != FacialState.Moving)
        {
            FacialGameObject.transform.position = FacialGameObjectOriginPos;
            FacialGameObject.SetActive(true);
            facialState = FacialState.Moving;
        }
    }
}
