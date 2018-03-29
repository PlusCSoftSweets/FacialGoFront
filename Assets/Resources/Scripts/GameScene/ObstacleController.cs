using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class ObstacleController : PunBehaviour {

	private Animator animator;
	private AnimationEvent evt;
	AnimationClip clip;
    AudioSource[] m_MyAudioSource = new AudioSource[1];

    // Use this for initialization
    void Start () {
		animator = GetComponent<Animator> ();
        DontDestroyOnLoad(this.gameObject.transform.parent.gameObject);
        DontDestroyOnLoad(this.gameObject);
        evt = new AnimationEvent() {
            functionName = "FlyEnd",
            objectReferenceParameter = gameObject,
            time = 1
        };
		
		clip = animator.runtimeAnimatorController.animationClips [0];
		clip.AddEvent (evt);
        m_MyAudioSource = GetComponents<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.tag == "Player") {
			collider.transform.GetComponent<HAHAController> ().PauseForMilliSeconds(1);
			animator.SetBool ("Fly", true);
            m_MyAudioSource[0].Play();
        }
	}

	public void FlyEnd(GameObject obj) {
        if (obj == gameObject)
            PhotonNetwork.RPC(photonView, "SetActive", PhotonTargets.All, false, null);
	}

    [PunRPC]
    private void SetActive()
    {
        this.gameObject.SetActive(false);
    }
}
