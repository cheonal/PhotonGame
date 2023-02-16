using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Rigidbody2D rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("11");
            rigid.AddForce(transform.up * 10 , ForceMode2D.Impulse);
        }
    }
}
