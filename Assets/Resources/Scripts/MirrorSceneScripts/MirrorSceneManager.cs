using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MirrorSceneManager : MonoBehaviour {

	public GameObject FacialGameObject;
	public GameObject MirrorMidGameObject;
	public GameObject WitchGameObject;
	public GameObject PercentGameObject;
	public WebCamController WebcamCtl;

	public float FacialMoveSpeed = 500.0f;
	public float SampleInterval = 1.0f;

	private GUIStyle buttonStyle;
	private Vector3 FacialGameObjectOriginPos;
	private float SampleTime;

	enum FacialState {
		Stopped,
		Moving,
		Passed,
		Failed
	};

	FacialState facialState = FacialState.Stopped;

	public void Awake() {
		buttonStyle = new GUIStyle ("button");
		buttonStyle.fontSize = 70;
		FacialGameObjectOriginPos = FacialGameObject.transform.position;
		FacialGameObject.SetActive (false);
	}

	// Use this for initialization
	void Start () {
		SampleTime = Time.fixedTime;
	}
	
	// Update is called once per frame
	void Update () {
		switch (facialState) {
		case FacialState.Moving:
			// Move Facial
			Vector3 curPos = FacialGameObject.transform.position;
			if (Vector3.Distance (curPos, MirrorMidGameObject.transform.position) < float.Epsilon) {
				facialState = FacialState.Stopped;
			}
			FacialGameObject.transform.position =
				Vector3.MoveTowards (curPos, MirrorMidGameObject.transform.position, Time.deltaTime * FacialMoveSpeed);

			// Check Match
			if (Time.fixedTime - SampleTime >= SampleInterval) {
				SampleTime = Time.fixedTime;
				StartCoroutine (CheckMatch ());
			}
			Debug.Log ("Moving");
			break;
		}
	}

	void OnGUI() {
		if (GUILayout.Button ("Facial", buttonStyle)) {
			Debug.Log ("Pressed");
			// Facial fly out
			if (facialState != FacialState.Moving) {
				FacialGameObject.transform.position = FacialGameObjectOriginPos;
				FacialGameObject.SetActive (true);
				facialState = FacialState.Moving;
			}
		}
	}

	IEnumerator CheckMatch() {
		yield return null;
		byte[] imageBytes = WebcamCtl.Snapshot ();
        int face_number = 0;
		yield return null;
		// TODO: Use the interface to cal percent
		// Here is just a simple test to UnityThreadHelper

		var thread = UnityThreadHelper.CreateThread(()=>{
			System.Threading.Thread.Sleep(1000);
			UnityThreadHelper.Dispatcher.Dispatch(()=>{
				Vector3 scale = FacialGameObject.transform.localScale;
				FacialGameObject.transform.localScale = scale * 2;
			});
		});
		double percent = FaceDiscr.uniqueInstance.Discriminent(imageBytes,face_number);
		PercentGameObject.GetComponent<Text> ().text = ((int)(percent)).ToString () + "%";
	}
}
