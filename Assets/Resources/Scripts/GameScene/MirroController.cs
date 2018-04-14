using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MirroController : MonoBehaviour {

    [SerializeField]
    private float dis;
    AudioSource[] m_MyAudioSource = new AudioSource[1];

    public delegate void AudioCallBack();
    // Use this for initialization
    void Start () {
        m_MyAudioSource = GetComponents<AudioSource>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "localHAHA") {
            other.gameObject.GetComponent<HAHAController>().isEnterMirror = false;
            other.gameObject.GetComponent<Rigidbody>().useGravity = false;
            other.gameObject.transform.position = new Vector3(-20, other.gameObject.transform.position.y, other.gameObject.transform.position.z);
            other.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
            other.gameObject.GetComponent<HAHAController>().forwardSpeed = 0;
            other.gameObject.GetComponent<HAHAController>().accelerateSpeed = 0;
            other.gameObject.GetComponent<HAHAController>().isUserControl = false;
            Debug.Log("In MirrorController:" + other.gameObject.name + HAHAController.GetHaHaInstance().isEnterMirror);
            AudioCallBack callbackTest = new AudioCallBack(ChangeScene);
            PlayClipData(callbackTest);
        }
    }
    public void PlayClipData(AudioCallBack callback) {
        m_MyAudioSource[0].Play();
        StartCoroutine(DelayedCallback(m_MyAudioSource[0].clip.length, callback));
    }

    private IEnumerator DelayedCallback(float time, AudioCallBack callback) {
        yield return new WaitForSeconds(time);
        callback();
    }
    void ChangeScene() {
        Global.instance.currentScene++;
        SceneManager.LoadScene("MirrorScene");
    }
}
