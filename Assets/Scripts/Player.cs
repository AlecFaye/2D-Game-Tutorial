using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Variables
    private float move = 0f;
    private bool m_resetJump = false;
    private bool m_grounded = false;
    private bool facingRight = true;

    private Rigidbody2D m_playerRigidbody;
    private PlayerAnimation m_playerAnimation;
    private SpriteRenderer m_playerSpriteRenderer;

    [SerializeField] private float runSpeed = 40f;
    [SerializeField] private float jumpForce = 5.0f;

    [SerializeField] private float rayCastDistance = 0.75f;
    [SerializeField] private LayerMask m_groundLayer;
    
    void Start()
    {
        m_playerRigidbody = GetComponent<Rigidbody2D>();
        m_playerAnimation = GetComponent<PlayerAnimation>();
        m_playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    
    void Update()
    {
        Movement();

        // Allows the player to attack
        if (Input.GetMouseButtonDown(0) && IsGrounded() == true)
        {
            m_playerAnimation.Attack();
        }
    }

    private void Movement()
    {
        // Get horizontal keys A, D, Left Arrow, Right Arrow
        move = Input.GetAxisRaw("Horizontal");

        // Always checks if you are grounded
        m_grounded = IsGrounded();

        // Flips the sprite
        if (move > 0.01f && !facingRight)
        {
            Flip();
            facingRight = true;
        }
        else if (move < -0.01f && facingRight)
        {
            Flip();
            facingRight = false;
        }

        // Call method from Player Animation by giving it the move value
        m_playerAnimation.Move(move);

        // Allows the player to jump if the space key is pressed and the player is grounded
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() == true)
        {
            m_playerRigidbody.velocity = new Vector2(m_playerRigidbody.velocity.x, jumpForce);
            StartCoroutine(ResetJumpRoutine());
            m_playerAnimation.Jump(true);
        }
    }

    // Flips the sprite the correct direction
    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // Returns a bool value if the player is grounded based on a raycast
    bool IsGrounded()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, rayCastDistance, m_groundLayer.value);
        Debug.DrawRay(transform.position, Vector2.down, Color.green);

        if (hitInfo.collider != null)
        {
            if (m_resetJump == false)
            {
                m_playerAnimation.Jump(false);
                return true;
            }
        }
        return false;
    }

    // Resets the jump after a certain amount of time to allow the player to jump again after a certain amount of time
    IEnumerator ResetJumpRoutine()
    {
        m_resetJump = true;
        yield return new WaitForSeconds(0.1f);
        m_resetJump = false;
    }

    // Moves the player
    private void FixedUpdate()
    {
        m_playerRigidbody.velocity = new Vector2(move * Time.fixedDeltaTime * 10f * runSpeed, m_playerRigidbody.velocity.y);
    }
}
