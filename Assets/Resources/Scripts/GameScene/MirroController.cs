﻿using System;
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
           AudioCallBack callbackTest = new AudioCallBack(changeScene);
           PlayClipData(callbackTest);
        }
    }
    public void PlayClipData(AudioCallBack callback)
    {
        m_MyAudioSource[0].Play();
        StartCoroutine(DelayedCallback(m_MyAudioSource[0].clip.length, callback));
    }

    private IEnumerator DelayedCallback(float time, AudioCallBack callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }
    void changeScene()
    {
        Global.instance.currentScene++;
        PhotonNetwork.LoadLevel("MirrorScene");
    }
}