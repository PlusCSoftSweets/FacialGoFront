using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LoginSceneManagerController : MonoBehaviour {

    public GameObject phone_input;
    public GameObject check_input;

    private bool ifChangeScene = false;

    /*
     * 更改登陆账号
     */
    public void OnChangeAccountClick() {
        Debug.Log("Change Account Click");
        ifChangeScene = false;  // 取消转场
        GameObject.Find("WaittingObject").transform.localPosition = new Vector3(0, 0, -1001);
        GameObject.Find("LoginObject").transform.localPosition = new Vector3(0, 0, 0);
    }

    /*
     * 获取验证码
     */
    public void OnCheckClick() {
        Debug.Log("Check Button Click");
    }

    /*
     * 登陆按钮
     */
    public void OnLoginClick() {
        Debug.Log("Login Button Click");
        string phone = phone_input.GetComponent<Text>().text;
        string check = check_input.GetComponent<Text>().text;

        if (phone.Length == 0) {
            Debug.Log("please input your phone number!");
            return;
        }
        else if (check.Length == 0) {
            Debug.Log("please input your check number!");
            return;
        }
        else {
            if (phone.Length != 11) {
                Debug.Log("please input the correct phone number!");
                return;
            }
            ifChangeScene = true;  // For Test
            StartCoroutine(SleepForSeconds());  // For Test
            // StartCoroutine(PostLoginInfo(phone, check));
            GameObject.Find("WaittingObject").transform.localPosition = new Vector3(0, 0, 0);
            GameObject.Find("LoginObject").transform.localPosition = new Vector3(0, 0, -1001);
        }
    }

    /*
     * 网络通信
     */
    IEnumerator PostLoginInfo(string phone, string check) {
        WWWForm userInfo = new WWWForm();
        userInfo.AddField("phoneNumber", phone);
        userInfo.AddField("checkNumber", check);

        UnityWebRequest www = UnityWebRequest.Post("http://www.my-server.com/myform", userInfo);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
            // 弹窗提示错误
        }
        else {
            Debug.Log("Form upload complete!");
            if (ifChangeScene) {
                StartCoroutine(FadeScene());
            }
        }
    }

    /*
     * 渐变转场
     */
    IEnumerator FadeScene() {
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("MainScene");
    }

    /*
     * 延迟适当时间 For Debug
     */
    IEnumerator SleepForSeconds() {
        yield return new WaitForSeconds(2);
        if (ifChangeScene) {
            StartCoroutine(FadeScene());
        }
    }
}