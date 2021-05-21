using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    const float GROUND_DIST_ERROR_MARGIN = .1f; // How far away from collider ground can be before onPlatform == true

    [Header("ComponentReferences")]
    [SerializeField] Rigidbody2D rb; // Reference to rigid body


    [Header("Variables")]
    [SerializeField] float maxHSpeed; // Maximum horizontal speed
    [SerializeField] float jumpForce; // How high player jump is
    [SerializeField] LayerMask platformLayer; // What layer jumpable platforms are on


    float groundDistance; // How far ground would be if player was on it
    bool onPlatform = false;


    void Start()
    {
        groundDistance = GetComponent<Collider2D>().bounds.extents.y + GROUND_DIST_ERROR_MARGIN;
    }


    // Update is called once per frame
    void Update()
    {
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

        if(onPlatform && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            onPlatform = false;
        }
    }


    bool CheckOnPlatform()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, groundDistance, platformLayer);
    }
}
