using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class FailSceneManagerController : MonoBehaviour {

    #region Public Variables
    public Text experience;
    public Text diamond;
    #endregion

    #region Private Variables
    private float exp;
    private float dia;
    #endregion

    void Start() {
        experience.text = "经验+";
        diamond.text = "X";
    }

    public void OnDetermineClick()
    {
        StartCoroutine(FadeScene());
    }

    /*
    * 网络通信
    */
    IEnumerator GetUserInfo(string phone, string check)
    {
        WWWForm userInfo = new WWWForm();
        userInfo.AddField("phoneNumber", phone);

        UnityWebRequest request = UnityWebRequest.Post("http://www.my-server.com/myform", userInfo);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
            // 弹窗提示错误
        }
        else
        {
            Debug.Log("Form upload complete!");
            Debug.Log(request.downloadHandler.text);
            // ulong results = request.downloadedBytes;
        }
    }

    IEnumerator FadeScene()
    {
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("MainScene");
    }
}
