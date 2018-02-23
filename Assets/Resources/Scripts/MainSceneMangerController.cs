using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneMangerController : MonoBehaviour {

    public void OnOpenRoomButtonClick() {
        Debug.Log("Open Room Button Click");
        StartCoroutine(FadeScene());
    }

    IEnumerator FadeScene()
    {
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("SingelModelScene");
    }
}