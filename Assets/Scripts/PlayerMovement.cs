using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f; // speed at which the player walks
    public float slideSpeed = 10f; // speed at which the player slides
    public float turnSpeed = 200f; // speed at which the player can turn
    public float boredTime = 10f;

    private Rigidbody rb;
    private bool isSliding = false;
    private bool isWounded = false;
    private bool onPlatform = false;
    private bool isBored = false;
    private float inActivityTimer = 0f;
    private int lemonCount = 0; // number of lemons collected
    private Vector3 moveDirection = Vector3.zero;
    public Animator animator;
    private Text lemonCountText; // UI text element to display lemon count

    public Joystick joystick; // reference for joystick component for mobile controls

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        GameObject lemonCountTextObject = GameObject.FindWithTag("LemonCountText");
        if (lemonCountTextObject != null)
        {
            lemonCountText = lemonCountTextObject.GetComponent<Text>();
        }
        UpdateLemonCountText();

        // Find the joystick at runtime
        GameObject joystickObject = GameObject.FindWithTag("Joystick");
        if (joystickObject != null)
        {
            joystick = joystickObject.GetComponent<Joystick>();
        }
    }

    void Update()
    {
        if (isWounded)
        {
            return; // if the player is wounded, dont process movement
        }

        HandleMovement();
        HandleInactivity();
    }

    void HandleMovement()
    {
        if (!isSliding)
        {
            float moveHorizontal = 0f;
            float moveVertical = 0f;

            if (joystick != null && (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer))
            {
                // Handle touch input for mobile devices
                moveHorizontal = joystick.Horizontal;
                moveVertical = joystick.Vertical;
            }
            else
            {
                // Handle keyboard input for desktop
                moveHorizontal = Input.GetAxis("Horizontal");
                moveVertical = Input.GetAxis("Vertical");
            }

            moveDirection = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

            if (moveDirection != Vector3.zero)
            {
                inActivityTimer = 0f;
                isBored = false;
                animator.SetBool("isBored", false);

                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
                animator.SetBool("isWalking", true);
                animator.SetBool("isSliding", false);
                animator.SetBool("isWounded", false);
            }
            else
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isSliding", false);
                animator.SetBool("isWounded", false);
            }

            Vector3 movement = moveDirection * walkSpeed * Time.deltaTime;
            rb.MovePosition(transform.position + movement);
        }
    }

    void FixedUpdate()
    {
        if (isSliding)
        {
            float turn = 0f;

            if (joystick != null && (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer))
            {
                // Handle touch input for mobile devices
                turn = joystick.Horizontal;
            }
            else
            {
                // Handle keyboard input for desktop
                turn = Input.GetAxis("Horizontal");
            }

            Vector3 turnRotation = Vector3.up * turn * turnSpeed * Time.deltaTime;
            transform.Rotate(turnRotation);
            Vector3 slideVelocity = transform.forward * slideSpeed;
            slideVelocity.y = rb.velocity.y;
            rb.velocity = slideVelocity;

            animator.SetBool("isSliding", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isWounded", false);
        }
        else
        {
            animator.SetBool("isSliding", false);
        }
    }

    void HandleInactivity()
    {
        if (!isBored)
        {
            inActivityTimer += Time.deltaTime;
            if (inActivityTimer >= boredTime)
            {
                isBored = true;
                animator.SetBool("isBored", true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlatformBoundary"))
        {
            isSliding = true;
            onPlatform = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlatformBoundary"))
        {
            isSliding = false;
            onPlatform = true;
        }

        if (other.CompareTag("Lemon"))
        {
            CollectLemon(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isSliding && collision.gameObject.CompareTag("Wall"))
        {
            isSliding = false;
            isWounded = true;
            rb.velocity = Vector3.zero; // stop the player
            animator.SetBool("isWounded", true);
            animator.SetBool("isSliding", false);
            animator.SetBool("isWalking", false);
        }
        else if (isWounded && collision.gameObject.CompareTag("Player"))
        {
            isWounded = false;
            animator.SetBool("isWounded", false);

            if (onPlatform)
            {
                // resume walking
                isSliding = false;
                animator.SetBool("isWounded", false);
                animator.SetBool("isWalking", true);
            }
            else
            {
                // resume sliding
                isSliding = true;
                animator.SetBool("isSliding", true);
            }
        }
    }

    void CollectLemon(GameObject Lemon)
    {
        lemonCount++;
        UpdateLemonCountText();
        Destroy(Lemon); // remove lemon from scene
    }

    void UpdateLemonCountText()
    {
        lemonCountText.text = lemonCount.ToString();
    }
}

