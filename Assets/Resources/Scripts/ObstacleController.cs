using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour {

	private Animator animator;
	private AnimationEvent evt;
	AnimationClip clip;
    AudioSource[] m_MyAudioSource = new AudioSource[1];

    // Use this for initialization
    void Start () {
		animator = GetComponent<Animator> ();

		evt = new AnimationEvent();
		evt.functionName = "FlyEnd";
		evt.objectReferenceParameter = gameObject;
		evt.time = 1;
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
            PhotonNetwork.Destroy(obj);
	}
}
