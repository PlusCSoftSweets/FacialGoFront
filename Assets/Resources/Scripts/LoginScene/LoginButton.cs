using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginButton : MonoBehaviour {

	public void OnLoginClick() {
		SceneManager.LoadScene ("SingelModelScene");
	}
}
