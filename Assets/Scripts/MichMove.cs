using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MichMove : MonoBehaviour
{
    public float walkSpeed = 5f; // speed at which the player walks
    public float slideSpeed = 10f; // speed at which the player slides
    public float turnSpeed = 200f; // speed at which the player can turn
    public float boredTime = 10f; // time after which bored animation starts

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

        // find the lemonCountText element by tag
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

    // Update is called once per frame
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
                animator.SetBool("MichisBored", false);

                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
                animator.SetBool("MichisWalking", true);
                animator.SetBool("MichisSliding", false);
                animator.SetBool("MichisWounded", false);
            }
            else
            {
                animator.SetBool("MichisWalking", false);
                animator.SetBool("MichisSliding", false);
                animator.SetBool("MichisWounded", false);
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

            animator.SetBool("MichisSliding", true);
            animator.SetBool("MichisWalking", false);
            animator.SetBool("MichisWounded", false);
        }
        else
        {
            animator.SetBool("MichisSliding", false);
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
                animator.SetBool("MichisBored", true);
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
            animator.SetBool("MichisWounded", true);
            animator.SetBool("MichisSliding", false);
            animator.SetBool("MichisWalking", false);
        }
        else if (isWounded && collision.gameObject.CompareTag("Player"))
        {
            isWounded = false;
            animator.SetBool("MichisWounded", false);

            if (onPlatform)
            {
                // resume walking
                isSliding = false;
                animator.SetBool("MichisWounded", false);
                animator.SetBool("MichisWalking", true);
            }
            else
            {
                // resume sliding
                isSliding = true;
                animator.SetBool("MichisSliding", true);
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
