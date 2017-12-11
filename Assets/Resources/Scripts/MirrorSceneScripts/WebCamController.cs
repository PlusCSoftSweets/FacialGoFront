using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCamController : MonoBehaviour {

	private bool cameraAvailable;
	private WebCamTexture frontCamera;
	private Texture defaultBackground;

	public RawImage background;
	public AspectRatioFitter fit;

	// Use this for initialization
	void Start () {
		cameraAvailable = false;
		defaultBackground = background.texture;
		WebCamDevice[] devices = WebCamTexture.devices;

		if (devices.Length == 0) {
			Debug.Log ("No camera detected");
			return;
		}
			
		for (int i = 0; i < devices.Length; ++i) {
			if (devices [i].isFrontFacing) {
				frontCamera = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
				break;
			}
		}

		if (frontCamera == null) {
			Debug.Log ("No front camera detected");
			return;
		}

		frontCamera.Play ();
		background.texture = frontCamera;
		cameraAvailable = true;
	}

	// Update is called once per frame
	void Update () {
		if (!cameraAvailable)
			return;

		float ratio = (float)frontCamera.width / frontCamera.height;
		fit.aspectRatio = ratio;

		float scaleY = frontCamera.videoVerticallyMirrored ? -1f : 1f;
		background.rectTransform.localScale = new Vector3 (1f, scaleY, 1f);

		int orient = -frontCamera.videoRotationAngle;
		background.rectTransform.localEulerAngles = new Vector3 (0, 180f, orient);
	}

	public byte[] Snapshot() {
		Texture2D snap = new Texture2D (frontCamera.width, frontCamera.height);

		Color[] colors = frontCamera.GetPixels();
		snap.SetPixels (colors);
		snap.Apply ();
		//return snap.EncodeToPNG ();
		return null;
	}
}
