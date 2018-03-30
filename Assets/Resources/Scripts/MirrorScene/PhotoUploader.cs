using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class PhotoUploader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Postpic(byte[] Roiface, int num)
    {
        var form = new WWWForm();
        form.AddField("token", "f16a163c62492537e19e887f4eefc1e93f2a01dfd3a71e8bb2791b2219d9205b");
        form.AddField("room_id", GlobalUserInfo.roomId);
        form.AddField("stage", num);
        form.AddBinaryData("photo", Roiface);
        UnityWebRequest req = UnityWebRequest.Post("http://123.207.93.25:9001/game/postPhoto", form);
        yield return req.SendWebRequest();
        if (req.isNetworkError || req.isHttpError)
        {
            Debug.LogError("Post Photo error");
        }
        else
        {
            Debug.Log("Post Photo success!");
        }
        Destroy(gameObject);
    }

    public void Upload(byte[] bytes,int num)
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(Postpic(bytes,num));
    }
}
