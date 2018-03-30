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
        experience.text = "经验+20";
        diamond.text = "X0";
    }

    public void OnDetermineClick()
    {
        StartCoroutine(FadeScene());
    }

    IEnumerator FadeScene()
    {
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("MainScene");
    }
}
