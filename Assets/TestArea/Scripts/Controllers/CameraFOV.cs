using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFOV : MonoBehaviour
{
    //The Camera that will be modified
    Camera playerCamera;
    //The target FOV we will be assigning to the camera
    float targetFOV;
    //The current Camera FOV
    float fov;

    void Start(){
        playerCamera = GetComponent<Camera>();
        targetFOV = playerCamera.fieldOfView;
        fov = targetFOV;
    }

    // Update is called once per frame
    void Update(){
        float fovSpeed = 4f;
        fov = Mathf.Lerp(fov, targetFOV, Time.deltaTime * fovSpeed);
        playerCamera.fieldOfView = fov;
    }

    public void SetCameraFov(float targetFOV){
        this.targetFOV = targetFOV;
    }
}
