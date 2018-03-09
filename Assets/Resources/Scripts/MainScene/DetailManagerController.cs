using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DetailManagerController : MonoBehaviour {

    public GameObject canvas;
    public InputField nickname;
    public InputField account;
    public GameObject avator;

    public bool isNicknameChange { get; set; }

    void Start() {
        Debug.Log("Start");
        isNicknameChange = false;
        nickname.text = GlobalUserInfo.userInfo.nickname;
        account.text = GlobalUserInfo.tokenInfo.account;
        string url = "http://123.207.93.25:9001/user/";
        url += GlobalUserInfo.userInfo.user_id;
        url += "/avatar";
        StartCoroutine(GetUserFace(url, 200, 200));
    }

    // 获取人脸
    IEnumerator GetUserFace(string url, int width, int height) {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError) {
            Debug.Log(request.error);
            // 弹窗提示错误
        }
        else {
            Debug.Log("Form upload complete!");
            avator.GetComponent<CircleImage>().sprite = GetSpriteFromBytes(request.downloadHandler.data, width, height);
        }
    }

    // 图像从二进制流到精灵
    private Sprite GetSpriteFromBytes(byte[] data, int width, int height) {
        Texture2D tex = new Texture2D(width, height);
        try {
            tex.LoadImage(data);
        }
        catch (Exception) {

        }
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
    }

    // 关闭账号详情
    public void OnCloseButtonClick() {
        canvas.SetActive(false);
        string url = "http://123.207.93.25:9001/user/";
        url += GlobalUserInfo.userInfo.user_id;
        string newNickname = nickname.text;
        if (newNickname != null || newNickname != GlobalUserInfo.userInfo.nickname) {
            if (newNickname != GlobalUserInfo.userInfo.nickname)
                isNicknameChange = true;
            StartCoroutine(UpdateNickname(newNickname, url));
            GlobalUserInfo.userInfo.nickname = newNickname;
        }
    }

    // 更改信息
    IEnumerator UpdateNickname(string newNickname, string url) {
        WWWForm userInfo = new WWWForm();
        userInfo.AddField("nickname", newNickname);
        userInfo.AddField("token", GlobalUserInfo.tokenInfo.token);

        UnityWebRequest request = UnityWebRequest.Post(url, userInfo);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError) {
            Debug.Log(request.error);
        }
        else {
            Debug.Log("From upload complete!");
        }
    }

    // 退出游戏
    public void OnQuitButtonClick() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // 切换账号跳转
    public void OnSwitchAccountButtonClick() {
        SceneManager.LoadScene("LoginScene");
    }
}
