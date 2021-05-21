using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    const float PLATFORM_DIST_ERROR_MARGIN = .01f; // How far away from collider extent platform can be before onPlatform == true, used to extend raycast slightly past player collider
    const float JUMP_DELAY_LENIENCE = .15f; // How long the jump button can be pressed above the ground before forgetting the input (used to jump when player eventually hits ground)
    const float COYOTE_TIME_LENIENCE = .15f; // How long after falling off platform the player can still jump


    [Header("ComponentReferences")]
    [SerializeField] Rigidbody2D rb; // Reference to rigid body


    [Header("Variables")]
    [SerializeField] float maxHSpeed; // Maximum horizontal speed
    [SerializeField] float jumpForce; // How high player jump is
    [SerializeField] LayerMask platformLayer; // What layer jumpable platforms are on


    float platformDistance; // How far ground would be if player was on it
    bool onPlatform = false;

    float timeSinceLastJumpInput = JUMP_DELAY_LENIENCE;
    float timeSinceLastOnPlatform = COYOTE_TIME_LENIENCE;


    void Start()
    {
        platformDistance = GetComponent<Collider2D>().bounds.extents.y + PLATFORM_DIST_ERROR_MARGIN;
    }


    void Update()
    {
        timeSinceLastJumpInput += Time.deltaTime;
        timeSinceLastOnPlatform += Time.deltaTime;

        HandleMovement();
        HandleJump();
    }


    void HandleMovement()
    {
        float hInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(hInput * maxHSpeed, rb.velocity.y);
    }


    void HandleJump()
    {
        onPlatform = CheckOnPlatform();

        if(onPlatform == true)
        {
            timeSinceLastOnPlatform = 0;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            timeSinceLastJumpInput = 0;
        }

        if(timeSinceLastOnPlatform < COYOTE_TIME_LENIENCE && timeSinceLastJumpInput < JUMP_DELAY_LENIENCE)
        {
            timeSinceLastJumpInput = JUMP_DELAY_LENIENCE;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            onPlatform = false;
        }
    }


    bool CheckOnPlatform()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, platformDistance, platformLayer);
    }
}
