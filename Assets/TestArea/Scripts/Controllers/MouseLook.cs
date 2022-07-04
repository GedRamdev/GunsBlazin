using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Tooltip("Mouse sensitivity modifier")]
    public float mouseSensitivity;
    [Tooltip("The Player body GameObject")]
    public Transform playerBody;
    //Starting rotation of the Player body GameObject
    float xRotation = 0f;
    void Start(){
        //Hide the cursor upon play
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update(){
        // Mouse rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        // Clamp player rotation to prevent the camera from rotating on itself
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        // Rotate the player
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
        
    }
}
