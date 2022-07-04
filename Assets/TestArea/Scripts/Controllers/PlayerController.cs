using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float NORMAL_FOV = 60f;
    const float GRAPPLING_FOV = 100f;
    [Tooltip("The GameObject acting as the POV of the Player")]
    [SerializeField] Camera playerCamera;
    CameraFOV cameraFov;
    [Tooltip("Character Controller acts as a Rigidbody, this script and the component in question has to be on the same GameObject")]
    [SerializeField] CharacterController controller;
    [Tooltip("Player speed modifier")]
    public float speed = 10f;
    [Tooltip("Gravity amount that affects the Player when falling")]
    public float gravity = -9.8f;
    [Tooltip("Location for the GroundCheck GameObject which is used to detect whether or not standing/colliding with something")]
    public Transform groundCheck;
    [Tooltip("Radius that is checked by the GroundCheck GameObject")]
    public float groundDistance = 0.4f;
    [Tooltip("The Layer that will be checked by the GroundCheck GameObject")]
    public LayerMask groundMask;
    // !!!!! Uncomment this if you wish to have sprint back !!!!!!
    // [Header("Sprint System)]
    // [Tooltip("Sprint speed modifier")]
    // public float sprintSpeed = 20f;
    // [Tooltip("Normal speed modifier, make sure this is always the same as Speed")]
    // public float normalSpeed = 10f;
    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    [Header("Jumping System")]
    [Tooltip("Player jump modifier")]
    public float jumpHeight = 2f;
    [Tooltip("The maximum of times the Player can jump")]
    public int jumpCountMax;
    [Tooltip("The current amount that the Player can jump")]
    public int jumpCountCurrent;
    [Tooltip("Audio being played after the Player jumps")]
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip run;
    Vector3 characterVelocityMomentum;
    Vector3 velocity;
    //Bool check whether or not the Player is on the ground
    bool isGrounded;
    //Bool check whether a double-jump is possible
    bool doubleJumpPossible;
    [Header("Dash System")]
    [Tooltip("Bool check whether or not the Player has pressed the Dash button")]
    public bool isDashing;
    [Tooltip("How far the Dash will move the player")]
    public float dashDistance;
    [Tooltip("How long the Dash takes to recharge")]
    public float dashCooldown = 2f;
    float dashStartTime;
    Vector3 move;
    [Tooltip("Dash Particle for forward motion")]
    [SerializeField] ParticleSystem forwardDashParticleSystem;
    [Tooltip("Dash Particle for backwards motion")]
    [SerializeField] ParticleSystem backwardsDashParticleSystem;
    [Tooltip("Dash Particle for right-side motion")]
    [SerializeField] ParticleSystem rightDashParticleSystem;
    [Tooltip("Dash Particle for left-side motion")]
    [SerializeField] ParticleSystem leftDashParticleSystem;
    Vector3 inputVector = Vector3.zero;
    private AudioSource audio;
    private AudioSource audioContinuous;
    [Tooltip("Audio being played after the Player dashes")]
    [SerializeField] AudioClip dash;

    public static bool isPaused = false;
    public GameObject pauseCanvas;
    private AudioListener listener;

    [Header("Grappling Hook System")]
    [Tooltip("The Grappling Hook Transform")]
    [SerializeField] Transform grapplingHookTransform;
    [Tooltip("How far the Grappling Hook can reach")]
    public float maxGrappleDistance;
    [Tooltip("Audio being played after the Player shoots the grappling hook")]
    [SerializeField] AudioClip grappleHookSound;
    [Tooltip("Audio bieng played after the Player cancels the grappling hook")]
    [SerializeField] AudioClip grappleHookBreak;
    //The size of the elongated part of the Grappling Hook that is thrown
    float grapplingHookSize;
    //Required for switch case in handling states of the Player
    State state;
    //Stored position of the Grappling hook
    Vector3 grapplingHookPosition;

    //Different States of Play
    private enum State{
        Normal,
        GrapplingHookThrown,
        GrapplingHookFlyingPlayer,
    }
    
    private void Start()
    {
        var soundPlayers = GetComponents<AudioSource>();
        audio = soundPlayers[0];
        audioContinuous = soundPlayers[1];
        controller = GetComponent<CharacterController>();
        cameraFov = playerCamera.GetComponent<CameraFOV>();
        state = State.Normal;
        grapplingHookTransform.gameObject.SetActive(false);
        listener = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioListener>();
    }

    void Update(){
        switch(state){
            default:
            case State.Normal:
        //Player Movement======================
        //Player Movement======================
        HandleCharacterMovement();
        //======================================
        // !!!!! Uncomment this if you wish to have sprint back !!!!!!
        //Sprinting=============================
        //HandleSprinting(normalSpeed, sprintSpeed);
        //======================================
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //Dash==================================
        HandleDash();
        //Grapple Hook==========================
        HandleGrapplingHookStart();
        //======================================

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                pauseGame();
            }
        }


        break;
        case State.GrapplingHookThrown:
        HandleGrapplingHookThrow();
        HandleCharacterMovement();
        break;
        case State.GrapplingHookFlyingPlayer:
        HandleGrapplingHookMovement(); 
        break;
        }
    }
    void HandleDash(){
        bool isTryingToDash = Input.GetKeyDown(KeyCode.LeftShift);
        if(isTryingToDash && !isDashing){
            if(Time.time > dashStartTime){
            OnStartDash();
            }
        }
        if(isDashing){
            if(Time.time - dashStartTime <= 0.4f){
                if(move.Equals(Vector3.zero)){
                    controller.Move(transform.forward * dashDistance * Time.deltaTime);
                } else {
                    controller.Move(move.normalized * dashDistance * Time.deltaTime);
                }
            } else {
                OnEndDash();
            }
        }
    }
    void OnStartDash(){
        isDashing = true;
        dashStartTime = Time.time;
        audio.PlayOneShot(dash);
        PlayDashParticles();
    }
    void OnEndDash(){
        isDashing = false;
        dashStartTime = Time.time + dashCooldown;
    }

    void PlayDashParticles(){
        if(inputVector.z > 0 && Mathf.Abs(inputVector.x) <= inputVector.z){
            // Forward and Forward Diagonal
            forwardDashParticleSystem.Play();
            return;
        }
        if(inputVector.z < 0 && Mathf.Abs(inputVector.x) <= Mathf.Abs(inputVector.z)){
            // Backward and Backward Diagonal
            backwardsDashParticleSystem.Play();
            return;
        }
        if(inputVector.x > 0){
            rightDashParticleSystem.Play();
            return;
        }
        if(inputVector.x < 0){
            leftDashParticleSystem.Play();
            return;
        }
        forwardDashParticleSystem.Play();
    }

    void pauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        isPaused = true;
        pauseCanvas.SetActive(true);
        listener.enabled = false;

    }

    void HandleSprinting(float normalSpeed, float sprintSpeed){
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            speed = sprintSpeed;
        } else if (Input.GetKeyUp(KeyCode.LeftShift)){
            speed = normalSpeed;
        }
    }
    void HandleGrapplingHookStart(){
        if(TestInputGrapplingHook()){
            if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit raycastHit, maxGrappleDistance)){
                //Hit something
                grapplingHookPosition = raycastHit.point;
                grapplingHookSize = 0f;
                grapplingHookTransform.gameObject.SetActive(true);
                grapplingHookTransform.localScale = Vector3.zero;
                state = State.GrapplingHookThrown;
                audio.PlayOneShot(grappleHookSound);
            }
        }
    }
    void HandleGrapplingHookMovement(){
        grapplingHookTransform.LookAt(grapplingHookPosition);
        //Where the Grapple Point is located relative to the Player
        Vector3 grapplingHookDirection = (grapplingHookPosition - transform.position).normalized;
        //How fast does the Player move towards the Grapple Point
        float grapplingHookSpeedMin = 10f;
        float grapplingHookSpeedMax = 40f;
        float grapplingHookSpeed = Mathf.Clamp(Vector3.Distance(transform.position, grapplingHookPosition), grapplingHookSpeedMin, grapplingHookSpeedMax);
        float grapplingHookSpeedMultiplier = 5f;
        //Moves the Player towards the Grapple Point
        controller.Move(grapplingHookDirection * grapplingHookSpeed * grapplingHookSpeedMultiplier * Time.deltaTime);
        float reachedGrapplingHookPositionDistance = 2f;
        if(Vector3.Distance(transform.position, grapplingHookPosition) < reachedGrapplingHookPositionDistance){
            //Reached Grappling Hook position
           StopGrapplingHook();
        }
        if(TestInputGrapplingHook()){
            //Cancel Grappling Hook
            StopGrapplingHook();
            audio.PlayOneShot(grappleHookBreak);
        }
        if(TestInputJump()){
            //Cancel Grappling Hook with Jump
            float momentumExtraSpeed = 3f;
            characterVelocityMomentum = grapplingHookDirection * grapplingHookSpeed * momentumExtraSpeed;
            //How powerful the jump afther cancelling is
            float cancelGrappleWithJumpSpeed = 40f;
            characterVelocityMomentum += Vector3.up * cancelGrappleWithJumpSpeed;
            StopGrapplingHook();
            audio.PlayOneShot(jump);
        }
    }
    void StopGrapplingHook(){
        state = State.Normal;
        ResetGravityEffect();
        grapplingHookTransform.gameObject.SetActive(false);
        cameraFov.SetCameraFov(NORMAL_FOV);
    }
    void HandleCharacterMovement(){
        //Check if on ground (Prevents velocity rising when standing on ground)================
        // !!! MIGHT NEED REWORK TO WORK WITH THE LOW POLY MAP !!!
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded && velocity.y < 0){
            velocity.y = 0f;
            jumpCountCurrent = jumpCountMax;
        }
        //=====================================
        //Player Movement======================
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.z = Input.GetAxis("Vertical");
        if (isGrounded && (inputVector.x != 0 || inputVector.z != 0))
        {
            if (!audioContinuous.isPlaying)
            {
                audioContinuous.Play();
            }
        }
        else
        {
            if(audioContinuous.isPlaying)
            {
                audioContinuous.Stop();
            }
        }


        move = transform.right * inputVector.x * speed + transform.forward * inputVector.z * speed;
        move += characterVelocityMomentum;
        controller.Move(velocity * Time.deltaTime);
        //============================================
        //Jumping=====================================
        if(TestInputJump()){
            if(isGrounded){
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            audio.PlayOneShot(jump);
            jumpCountCurrent--;
            } if(!isGrounded && jumpCountCurrent > 0){
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            audio.PlayOneShot(jump);
            jumpCountCurrent--;
            }
        }
        //Gravity===============================
        velocity.y += gravity * Time.deltaTime;
        //Apply Momentum !Important that it is declared before moving the Player!
        
        controller.Move(move * Time.deltaTime);
        //======================================
        //Dampen Character Momentum
        if(characterVelocityMomentum.magnitude >= 0f){
            float momentumDrag = 3f;
            characterVelocityMomentum -= characterVelocityMomentum * momentumDrag * Time.deltaTime;
            if(characterVelocityMomentum.magnitude < .0f){
                characterVelocityMomentum = Vector3.zero;
            }
        }
    }
    void HandleGrapplingHookThrow(){
        grapplingHookTransform.LookAt(grapplingHookPosition);
        float grapplingHookThrowSpeed = 70f;
        grapplingHookSize += grapplingHookThrowSpeed * Time.deltaTime;
        //Scales the Grappling Hook
        grapplingHookTransform.localScale = new Vector3 (1f, 1f, grapplingHookSize);
        if(grapplingHookSize >= Vector3.Distance(transform.position, grapplingHookPosition)){
            state = State.GrapplingHookFlyingPlayer;
            cameraFov.SetCameraFov(GRAPPLING_FOV);
        }
        
    }
    void ResetGravityEffect(){
        //Resets gravity to 0 ,to prevent the Player being fast-dropped to the ground
        velocity.y = 0f;
    }
    bool TestInputGrapplingHook(){
        //Change the KeyCode to change the input button for Grappling Hook
        return Input.GetKeyDown(KeyCode.Q);
    }
    bool TestInputJump(){
        return Input.GetButtonDown("Jump");
    }
}
