using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchRotation : MonoBehaviour
{
    public Transform obj;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = obj.rotation;
    }
}
