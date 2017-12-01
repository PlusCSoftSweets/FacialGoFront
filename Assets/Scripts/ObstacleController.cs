using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour {

	private Animator animator;
	private AnimationEvent evt;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();

		evt = new AnimationEvent();
		evt.functionName = "FlyEnd";
		evt.intParameter = 1;
		evt.time = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.tag == "Player") {
			collider.transform.GetComponent<HAHAController> ().pauseForMilliSeconds(1);
			// TODO: 踢飞动画
			animator.SetBool ("Fly", true);
			AnimationClip clip = animator.runtimeAnimatorController.animationClips [0];
			clip.AddEvent (evt);
		}
	}

	public void FlyEnd(int i) {
		gameObject.SetActive (false);
	}
}
