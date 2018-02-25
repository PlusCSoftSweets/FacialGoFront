using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenRoomSceneMangerController : MonoBehaviour {

    public void OnInviteButtonClick() {
        // 邀请好友
    }

    public void OnStartButtonClick() {
        Debug.Log("Start Button Click");
        StartCoroutine(FadeScene());
    }

    IEnumerator FadeScene() {
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("SingelModelScene");
    }
}