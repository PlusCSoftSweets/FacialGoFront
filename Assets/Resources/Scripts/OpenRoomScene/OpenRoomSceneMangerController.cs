using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenRoomSceneMangerController : MonoBehaviour {

    public void OnInviteButtonClick() {
        // 邀请好友
    }

    public void OnBackButtonClick() {
        Debug.Log("Back Button Click");
        StartCoroutine(LoadLastScene());
    }

    public void OnStartButtonClick() {
        Debug.Log("Start Button Click");
        StartCoroutine(LoadSingelModelScene());
    }

    IEnumerator LoadSingelModelScene() {
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("SingelModelScene");
    }

    IEnumerator LoadLastScene() {
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("MainScene");
    }
}