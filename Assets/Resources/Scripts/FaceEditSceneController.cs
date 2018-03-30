using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;

public class FaceEditSceneController : MonoBehaviour {

    public GameObject leftLine;
    public GameObject rightLine;
    public GameObject FacePanel;
    public GameObject WordPanel;
    public GameObject Face;


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
        foreach (Transform child in FacePanel.transform)
        {
            Button btn = child.GetComponent<Button>();
            Image image = child.GetComponent<Image>();
            Sprite sprite = image.sprite;
            btn.onClick.AddListener(delegate () {
                this.onItemClick(sprite);
            });
        }
    }
    public void onItemClick(Sprite sprite)
    {
        Image image = Face.GetComponent<Image>();
        image.sprite = sprite;
    }
}
