using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour {

	private Animator animator;
	private AnimationEvent evt;
	private GameObjectPool pool;
	AnimationClip clip;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		pool = GameObjectPool.FindPool ("Obstacle");

		evt = new AnimationEvent();
		evt.functionName = "FlyEnd";
		evt.objectReferenceParameter = gameObject;
		evt.time = 1;
		clip = animator.runtimeAnimatorController.animationClips [0];
		clip.AddEvent (evt);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.tag == "Player") {
			collider.transform.GetComponent<HAHAController> ().pauseForMilliSeconds(1);
			animator.SetBool ("Fly", true);
		}
	}

	public void FlyEnd(GameObject obj) {
		if (obj == gameObject)
			pool.FreeObject (gameObject.transform.parent.gameObject);
	}
}
