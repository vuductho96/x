using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonControl : MonoBehaviour
{
   
    public float moveSpeed = 5f;
    public float sprintSpeedMultiplier = 1.5f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpForce = 5f;
    private Animator animator;
    private CharacterController controller;
    private Vector3 moveDirection;
    private bool isJumping;
    private bool isSprinting;
    AudioSource audioSource;
    public AudioClip Jump;
   public  AudioClip FootStep;
    public AudioClip Run;
    public AudioClip Whistle;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        // Get input axes
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        // Calculate movement vector
        Vector3 movement = new Vector3(horizontalAxis, 0f, verticalAxis).normalized * moveSpeed;
        float currentMoveSpeed = isSprinting ? moveSpeed * sprintSpeedMultiplier : moveSpeed;
        movement *= currentMoveSpeed;

        // Rotate the character towards the movement direction
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // Apply movement to the character controller
        controller.Move(movement * Time.deltaTime);

        // Apply gravity
        moveDirection.y += gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
        // Handle Attack
        if (Input.GetKeyDown(KeyCode.K))
        {
            Kick();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Punch();
        }
        // Handle jumping
        if (controller.isGrounded)
        {
            moveDirection.y = 0f;

            if (Input.GetButtonDown("Jump"))
            {
                if (!isJumping)
                {
                    moveDirection.y = jumpForce;
                    isJumping = true;
                    animator.SetTrigger("Jump");
                    audioSource.PlayOneShot(Jump);
                }
            }
            else
            {
                isJumping = false;
            }
        }

        // Update animator parameters based on movement speed
        float moveSpeedMagnitude = movement.magnitude;
        if (moveSpeedMagnitude > 0f)
        {
            if (isSprinting)
            {
                Run1();
            }
            else
            {
                Walk();
            }
        }
        else
        {
            Idle();
        }

        // Handle sprinting input
        HandleSprintingInput();
    }
    private void PlayIdleSound()
    {
        // Play the idle sound
        audioSource.PlayOneShot(Whistle);
    }
    private void Walk()
    {
   
        animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }

    private void Idle()
    {
        animator.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
       

    }

    private void Run1()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = FootStep;// DONT HAVE SOUND FOR RUN USING FOOTSTEP INSTEAD
            audioSource.Play();
        }

        animator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
    }


    private void HandleSprintingInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
        }
    }
    

    private void Punch()
    {
        animator.SetTrigger("Punch");
    }
    private void Kick()
    {
        animator.SetTrigger("Kick");
    }
   
}




