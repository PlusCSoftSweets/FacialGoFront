using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginButton : MonoBehaviour {

    public void OnChangeAccountClick() {
        Debug.Log("Change Account Click");
    }

    public void OnCheckClick() {
        Debug.Log("Check Button Click");
    }

	public void OnLoginClick() {
        Debug.Log("Login Button Click");
        GameObject.Find("WaittingObject").transform.localPosition = new Vector3(0, 0, 0);
        GameObject.Find("LoginObject").transform.localPosition = new Vector3(0, 0, -1001);
        StartCoroutine(FadeScene());
    }

    IEnumerator FadeScene() {
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("SingelModelScene");
    }
}