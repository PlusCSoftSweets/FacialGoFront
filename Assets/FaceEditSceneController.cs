using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

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
        string roomId = GlobalUserInfo.roomId;
        string baseUrl = "http://123.207.93.25:9001/game/getPhoto?";
        baseUrl += "token=" + GlobalUserInfo.tokenInfo.token;
        baseUrl += "&room_id=" + roomId;
        for (int i = 0; i < stageCount; ++i) {
            string url = baseUrl + "&stage=" + i;
            UnityWebRequest req = UnityWebRequest.Get(url);
            yield return req.SendWebRequest();
            if (req.isNetworkError || req.isHttpError) {
                Debug.LogError("Get Photos failed");
            } else {
                var bytes = req.downloadHandler.data;
                var texture = new Texture2D(2, 2);
                texture.LoadImage(bytes);
                images.Add(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)));
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
            btn.onClick.AddListener(delegate () {
                this.onItemClick(i);
            });
            i++;
        }
    }
    public void onItemClick(int index)
    {
        Image image = Face.GetComponent<Image>();
        image.sprite = emotionMap.emotions[index];
        // locate human face
        if (!humanFace.gameObject.activeSelf) {
            humanFace.gameObject.SetActive(true);
        }
        humanFace.rectTransform.sizeDelta = emotionMap.emotionSizes[index];
        humanFace.rectTransform.position = emotionMap.emotionCenters[index];
    }

    public void OnLeftClick() {
        int nextIndex = (curIndex - 1 + stageCount) % stageCount;
        if (images.Count < nextIndex) {
            Debug.Log("Photo hasnt loaded");
        } else {
            curIndex = nextIndex;
            Face.GetComponent<Image>().sprite = images[curIndex];
        }
    }

    public void OnRightClick() {
        int nextIndex = (curIndex + 1) % stageCount;
        if (images.Count < nextIndex) {
            Debug.Log("Photo hasnt loaded");
        } else {
            curIndex = nextIndex;
            Face.GetComponent<Image>().sprite = images[curIndex];
        }
    }

    public void OnDownloadClick() {
        DateTime dateTime = DateTime.Now;
        ScreenCapture.CaptureScreenshot(dateTime.ToString() + ".png");
    }
}
