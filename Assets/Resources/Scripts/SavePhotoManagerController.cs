using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePhotoManagerController : MonoBehaviour {

    [SerializeField]
    Image myImage;

    // Use this for initialization
    void Start () {
        Sprite ss = Resources.Load("Textures/test", typeof(Sprite)) as Sprite;


        Texture2D tex = Resources.Load("Textures/blackTexture", typeof(Texture2D)) as Texture2D;

        Sprite s = Sprite.Create(tex, new Rect(0,0,tex.width,tex.height), new Vector2(0,0));
        Debug.Log(tex.width);
        Debug.Log(tex.height);
        myImage.overrideSprite = ss;
        
        

        //Sprite temp = myImage.sprite;
        //Texture2D ttex = temp.texture;
	}
	
	// Update is called once per frame
	void Update () {
		
	}



}
