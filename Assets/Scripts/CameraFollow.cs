using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smooth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void LateUpdate()
    {
        if (target != null && !Global.isGameOver)
        {
            if (transform.position != target.position)
            {
                Vector3 vec = target.position;
                transform.position = Vector3.Lerp(transform.position, vec, smooth);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
