using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MirrorSceneManager : MonoBehaviour {

	public GameObject FacialGameObject;
	public GameObject MirrorMidGameObject;
	public GameObject WitchGameObject;
	public GameObject PercentGameObject;
	public WebCamController WebcamCtl;
	public GameObject PassedOrFailedLabel;
	public Texture[] Faces;

	private int CurFaceIndex = 0;

	public float FacialMoveSpeed = 500.0f;
	public float SampleInterval = 1.0f;
	[Range(0.0f, 1.0f)]
	public float PassPercent = 0.8f;

	private GUIStyle buttonStyle;
	private Vector3 FacialGameObjectOriginPos;
	private float SampleTime;

	double percent = 0.0;

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
		StartFaceMove ();
	}
	
	// Update is called once per frame
	void Update () {
		switch (facialState) {
		case FacialState.Moving:
			// Move Facial
			Vector3 curPos = FacialGameObject.transform.position;
			if (Vector3.Distance (curPos, MirrorMidGameObject.transform.position) < float.Epsilon) {
				//facialState = FacialState.Stopped;
				if (percent > PassPercent)
					facialState = FacialState.Passed;
				else
					facialState = FacialState.Failed;
				
				if (CurFaceIndex < Faces.Length)
					StartCoroutine (WaitAndNextFace ());
				else
					SceneManager.LoadScene("SingelModelScene");
			}
			FacialGameObject.transform.position =
				Vector3.MoveTowards (curPos, MirrorMidGameObject.transform.position, Time.deltaTime * FacialMoveSpeed);

			// Check Match
			if (Time.fixedTime - SampleTime >= SampleInterval) {
				SampleTime = Time.fixedTime;
				StartCoroutine (CheckMatch ());
			}
			break;
		}

		// GUI
		if (facialState == FacialState.Passed) {
			if (PassedOrFailedLabel.activeSelf == false) {
				PassedOrFailedLabel.GetComponent<Text> ().text = "PASSED!";
				PassedOrFailedLabel.SetActive (true);
			}
		} else if (facialState == FacialState.Failed) {
			if (PassedOrFailedLabel.activeSelf == false) {
				PassedOrFailedLabel.GetComponent<Text> ().text = "FAILED!";
				PassedOrFailedLabel.SetActive (true);
			}
		} else if (PassedOrFailedLabel.activeSelf == true) {
			PassedOrFailedLabel.SetActive (false);
		}
	}

	void OnGUI() {
	}

	IEnumerator CheckMatch() {
		yield return null;
		Texture2D texture = WebcamCtl.Snapshot ();
        int face_number = 0;
		yield return null;

		double rand = Random.Range (0.5f, 0.9f);

		// TODO: EncodeTo can be only called in main thread, consider a way to make it in other thread
		//byte[] imageBytes = texture.EncodeToPNG();

		var thread = UnityThreadHelper.CreateThread(()=>{
			// TODO: Use the real interface to cal percent
			percent = rand;

			//percent = FaceDiscr.uniqueInstance.Discriminent(imageBytes,face_number);
			Debug.Log(percent);
			UnityThreadHelper.Dispatcher.Dispatch(()=>{
				PercentGameObject.GetComponent<Text> ().text = ((int)(percent * 100)).ToString () + "%";
				if (percent > PassPercent) {
					facialState = FacialState.Passed;
					if (CurFaceIndex < Faces.Length)
						StartCoroutine (WaitAndNextFace ());
					else
						SceneManager.LoadScene("SingelModelScene");
				}
			});
		});
		//thread.Start ();
	}

	IEnumerator WaitAndNextFace() {
		yield return new WaitForSeconds (1);
		StartFaceMove ();
	}

	void StartFaceMove() {
		if (CurFaceIndex >= Faces.Length) {
			Debug.Log ("No next face");
			return;
		}
		FacialGameObject.GetComponent<RawImage> ().texture = Faces [CurFaceIndex];
		CurFaceIndex++;
		// Facial fly out
		if (facialState != FacialState.Moving) {
			FacialGameObject.transform.position = FacialGameObjectOriginPos;
			FacialGameObject.SetActive (true);
			facialState = FacialState.Moving;
		}
	}
}
