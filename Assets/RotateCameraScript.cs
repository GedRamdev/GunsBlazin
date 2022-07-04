using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCameraScript : MonoBehaviour
{
    public float speed = 3f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Time.deltaTime * speed, 0);
    }
}
