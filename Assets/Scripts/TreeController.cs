using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{

    public float speed = 1.0f;  // 人物速度一致
    private Rigidbody tree;

    void Start()
    {
        tree = GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.2f * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 24f);
        }
    }
}
