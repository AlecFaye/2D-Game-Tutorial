using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    // Variables
    private float move = 0f;
    private bool _resetJump = false;
    private bool _grounded = false;
    private bool _facingRight = true;

    private Rigidbody2D _playerRigidbody;
    private PlayerAnimation _playerAnimation;
    private SpriteRenderer _playerSpriteRenderer;

    [SerializeField] private float runSpeed = 40f;
    [SerializeField] private float jumpForce = 5.0f;

    [SerializeField] private float rayCastDistance = 0.75f;
    [SerializeField] private LayerMask _groundLayer;

    public int Health { get; set; }
    
    void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    
    void Update()
    {
        Movement();

        // Allows the player to attack
        if (Input.GetMouseButtonDown(0) && IsGrounded() == true)
        {
            _playerAnimation.Attack();
        }
    }

    private void Movement()
    {
        // Get horizontal keys A, D, Left Arrow, Right Arrow
        move = Input.GetAxisRaw("Horizontal");

        // Always checks if you are grounded
        _grounded = IsGrounded();

        // Flips the sprite
        if (move > 0.01f && !_facingRight)
        {
            Flip();
            _facingRight = true;
        }
        else if (move < -0.01f && _facingRight)
        {
            Flip();
            _facingRight = false;
        }

        // Call method from Player Animation by giving it the move value
        _playerAnimation.Move(move);

        // Allows the player to jump if the space key is pressed and the player is grounded
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() == true)
        {
            _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, jumpForce);
            StartCoroutine(ResetJumpRoutine());
            _playerAnimation.Jump(true);
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
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, rayCastDistance, _groundLayer.value);
        Debug.DrawRay(transform.position, Vector2.down, Color.green);

        if (hitInfo.collider != null)
        {
            if (_resetJump == false)
            {
                _playerAnimation.Jump(false);
                return true;
            }
        }
        return false;
    }

    // Resets the jump after a certain amount of time to allow the player to jump again after a certain amount of time
    IEnumerator ResetJumpRoutine()
    {
        _resetJump = true;
        yield return new WaitForSeconds(0.1f);
        _resetJump = false;
    }

    // Moves the player
    private void FixedUpdate()
    {
        _playerRigidbody.velocity = new Vector2(move * Time.fixedDeltaTime * 10f * runSpeed, _playerRigidbody.velocity.y);
    }

    // Damages the player
    public void Damage()
    {
        Debug.Log("Player Damage Called");
    }
}
