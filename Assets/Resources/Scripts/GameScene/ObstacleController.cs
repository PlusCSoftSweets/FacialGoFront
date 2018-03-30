using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class ObstacleController : PunBehaviour {

	private Animator animator;
	private AnimationEvent evt;
	AnimationClip clip;
    AudioSource[] m_MyAudioSource = new AudioSource[1];

    void Start () {
		animator = GetComponent<Animator> ();
        DontDestroyOnLoad(this.gameObject.transform.parent.gameObject);
        evt = new AnimationEvent() {
            functionName = "FlyEnd",
            objectReferenceParameter = gameObject,
            time = 1
        };
		
		clip = animator.runtimeAnimatorController.animationClips [0];
		clip.AddEvent (evt);
        m_MyAudioSource = GetComponents<AudioSource>();
    }

	void OnTriggerEnter(Collider collider) {
		if (collider.CompareTag("Player")) {
			collider.transform.GetComponent<HAHAController> ().PauseForMilliSeconds(1);
			animator.SetBool ("Fly", true);
            m_MyAudioSource[0].Play();
        }
	}

	public void FlyEnd(GameObject obj) {
        if (obj == gameObject)
            this.gameObject.transform.position = new Vector3(-20, 0, 0);
            //PhotonNetwork.RPC(photonView, "SetActive", PhotonTargets.All, false, null);
	}

    //[PunRPC]
    //private void SetActive() {
    //    this.gameObject.SetActive(false);
    //}
}
