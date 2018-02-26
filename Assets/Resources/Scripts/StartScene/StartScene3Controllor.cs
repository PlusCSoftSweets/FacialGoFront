using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene3Controllor : MonoBehaviour
{

    private float jumpTime = 0.0f;

    void Update()
    {
        jumpTime += Time.deltaTime;
        if (Mathf.Abs(jumpTime - 2.0f) < 0.05)
        {
            StartCoroutine(FadeScene());
        }
    }

    IEnumerator FadeScene()
    {
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("LoginScene");
    }
}