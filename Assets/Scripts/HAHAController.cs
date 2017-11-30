using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HAHAController : MonoBehaviour
{
    private Rigidbody player;

    void Start()
    {
        //player = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        //{
        //    Vector2 movement = Input.GetTouch(0).deltaPosition;
        //    if (movement.x < 0)
        //    {
        //        if (transform.position.x > -1.0f)
        //            transform.position = new Vector3(transform.position.x - 0.7f, transform.position.y, transform.position.z);
        //    }
        //    else if (movement.x > 0)
        //    {
        //        if (transform.position.x < 1.0f)
        //            transform.position = new Vector3(transform.position.x + 0.7f, transform.position.y, transform.position.z);
        //    }
        //}
    }
}
