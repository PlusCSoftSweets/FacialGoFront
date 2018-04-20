using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using OpenCVForUnity;

public class FaceEditSceneController : MonoBehaviour {

    private int curIndex = 0;
    private static readonly int stageCount = 4;

    List<Sprite> images;

    public GameObject leftLine;
    public GameObject rightLine;
    public GameObject FacePanel;
    public GameObject WordPanel;
    public GameObject Face;

    [System.Serializable]
    public class EmotionMap {
        public Sprite[] emotions;
        public Vector2[] emotionCenters;
        public Vector2[] emotionSizes;
    };

    public EmotionMap emotionMap;
    public Image humanFace;


    void Awake() {
        images = new List<Sprite>();
        StartCoroutine(GetPhotos());
    }

    IEnumerator GetPhotos() {
        string roomId = GlobalUserInfo.roomInfo.roomIndex;
        Debug.Log(GlobalUserInfo.roomInfo.roomIndex);
        string baseUrl = "http://123.207.93.25:9001/game/getPhoto?";
        baseUrl += "token=" + GlobalUserInfo.tokenInfo.token;
        baseUrl += "&room_id=" + roomId;
        for (int i = 0; i < stageCount; ++i) {
            string url = baseUrl + "&stage=" + i;
            UnityWebRequest req = UnityWebRequest.Get(url);
            yield return req.SendWebRequest();
            if (req.isNetworkError || req.isHttpError) {
                Debug.LogError("Get Photos failed");
                Debug.LogError(req.downloadHandler.text);
            } else {
                var bytes = req.downloadHandler.data;
                var texture = new Texture2D(2, 2);
                texture.LoadImage(bytes);

                // 素描化
                Mat mat = new Mat(texture.height, texture.width, CvType.CV_8UC1);
                Utils.texture2DToMat(texture, mat);
                Mat newMat = SuMiaoFilter(mat);
                Utils.matToTexture2D(newMat, texture);
                images.Add(Sprite.Create(texture, new UnityEngine.Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)));
            }
        }

        yield return null;
    }

    // Use this for initialization
    void Start () {
        leftLine.SetActive(true);
        rightLine.SetActive(false);
        FacePanel.SetActive(true);
        WordPanel.SetActive(false);
        FaceItemClick();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void onFacePanelClick()
    {
        leftLine.SetActive(true);
        rightLine.SetActive(false);
        WordPanel.SetActive(false);
        FacePanel.SetActive(true);
    }
    public void onWordPanelClick()
    {
        leftLine.SetActive(false);
        rightLine.SetActive(true);
        FacePanel.SetActive(false);
        WordPanel.SetActive(true);
    }
    public void backToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void FaceItemClick()
    {
        int i = 0;
        foreach (Transform child in FacePanel.transform)
        {
            Button btn = child.GetComponent<Button>();
            Image image = child.GetComponent<Image>();
            Sprite sprite = image.sprite;
            image.sprite = emotionMap.emotions[i];
            int j = i;
            btn.onClick.AddListener(delegate () {
                this.onItemClick(j);    
            });
            i++;
        }
    }
    public void onItemClick(int index)
    {
        Debug.Log(index);
        Image image = Face.GetComponent<Image>();
        image.sprite = emotionMap.emotions[index];
        // locate human face
        if (!humanFace.gameObject.activeSelf) {
            humanFace.gameObject.SetActive(true);
        }
        humanFace.rectTransform.sizeDelta = emotionMap.emotionSizes[index];
        humanFace.rectTransform.localPosition = emotionMap.emotionCenters[index];
    }

    public void OnLeftClick() {
        if (!humanFace.gameObject.activeSelf) {
            humanFace.gameObject.SetActive(true);
        }
        int nextIndex = (curIndex - 1 + stageCount) % stageCount;
        if (images.Count < nextIndex) {
            Debug.Log("Photo hasnt loaded");
        } else {
            curIndex = nextIndex;
            humanFace.GetComponent<Image>().sprite = images[curIndex];
        }
    }

    public void OnRightClick() {
        if (!humanFace.gameObject.activeSelf) {
            humanFace.gameObject.SetActive(true);
        }
        int nextIndex = (curIndex + 1) % stageCount;
        if (images.Count < nextIndex) {
            Debug.Log("Photo hasnt loaded");
        } else {
            curIndex = nextIndex;
            humanFace.GetComponent<Image>().sprite = images[curIndex];
        }
    }

    public void OnDownloadClick() {
        DateTime dateTime = DateTime.Now;
        ScreenCapture.CaptureScreenshot(dateTime.ToString("yyyy-MM-dd HH-mm-ss") + ".png");
    }

    Mat SuMiaoFilter(Mat srcImage) {
        Mat gray0 = new Mat();
        Mat gray1 = new Mat();

        // 传过来的图已经是灰度图
        gray0 = srcImage.clone();
        
        Scalar maxScalar = new Scalar(255);
        gray1 = gray0.clone();

        for (int y = 0; y < srcImage.height(); y++) {
            for (int x = 0; x < srcImage.width(); x++) {
                byte[] temp0 = new byte[1];
                gray0.get(y, x, temp0);
                
                byte[] input = new byte[1];
                input[0] = (byte)((byte)255 - temp0[0]);
                gray1.put(y, x, input);
            }
        }

        // 高斯模糊
        Imgproc.GaussianBlur(gray1, gray1, new Size(13, 13), 0);

        Mat dstImage = new Mat(gray1.size(), CvType.CV_8UC1);
        for (int y = 0; y < srcImage.height(); y++) {
            for (int x = 0; x < srcImage.width(); x++) {
                byte[] temp0 = new byte[1], temp1 = new byte[1];
                gray0.get(y, x, temp0);
                gray1.get(y, x, temp1);
                byte[] input = new byte[1];
                input[0] = (byte)Math.Min((temp0[0] + (temp0[0] * temp1[0]) / (256 - temp1[0])), (byte)255);
                if (input[0] < (byte)240) input[0] -= (byte)40;
                dstImage.put(y, x, input);
            }
        }

        return dstImage;
    }
}
