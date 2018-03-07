using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MainSceneMangerController : MonoBehaviour {

    public GameObject usernameGO;
    public GameObject levelGO;
    public GameObject diamondGO;

    void Start() {
        // 通过Http从数据库拿到三个条目再赋值
        usernameGO.GetComponent<Text>().text = GlobalUserInfo.userInfo.nickname;
        diamondGO.GetComponent<Text>().text = GlobalUserInfo.userInfo.diamand.ToString();
        levelGO.GetComponent<Text>().text = Assets.Resources.Scripts.LevelCalculation.ExpToLevel(GlobalUserInfo.userInfo.exp).ToString();
    }

    public void OnOpenRoomButtonClick() {
        Debug.Log("Open Room Button Click");
        StartCoroutine(FadeScene());
    }

    /*
     * 网络通信
     */
    IEnumerator GetUserInfo(string phone, string check) {
        WWWForm userInfo = new WWWForm();
        userInfo.AddField("phoneNumber", phone);

        UnityWebRequest request = UnityWebRequest.Post("http://www.my-server.com/myform", userInfo);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError) {
            Debug.Log(request.error);
            // 弹窗提示错误
        }
        else {
            Debug.Log("Form upload complete!");
            Debug.Log(request.downloadHandler.text);
            // ulong results = request.downloadedBytes;
        }
    }

    IEnumerator FadeScene() {
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("OpenRoomScene");
    }
}