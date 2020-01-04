using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour, IDamageable
{
    // Variables
    private float move = 0f;
    private bool _resetJump = false;
    private bool _grounded = false;
    private bool _facingRight = true;
    private bool _isDead = false;
    private bool _resetAttack = false;

    private Rigidbody2D _playerRigidbody;
    private PlayerAnimation _playerAnimation;
    private BoxCollider2D _playerHitBox;

    [SerializeField] private float runSpeed = 40f;
    [SerializeField] private float jumpForce = 5.0f;
    // [SerializeField] private int health = 5;

    [SerializeField] private float rayCastDistance = 0.75f;
    [SerializeField] private LayerMask _groundLayer;

    public int diamonds = 0;
    public int Health { get; set; }
    
    void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _playerHitBox = GetComponent<BoxCollider2D>();

        Health = 4;
    }
    
    void Update()
    {
        if (_playerAnimation.PlayerDead())
        {
            _isDead = true;
            this.tag = "Untagged";
            return;
        }

        // Updates the UI to show the total gem count
        UIManager.Instance.UpdateGemCount(diamonds);

        // Allows the player to move
        Movement();

        // Allows the player to attack
        if ((Input.GetKeyDown(KeyCode.F) || CrossPlatformInputManager.GetButtonDown("A_Button")) && IsGrounded() == true)
        {
            if (_resetAttack == false)
            {
                _playerAnimation.Attack();
                StartCoroutine(ResetAttackRoutine());
            }
        }
    }

    private void Movement()
    {
        // Get horizontal keys A, D, Left Arrow, Right Arrow
        move = CrossPlatformInputManager.GetAxisRaw("Horizontal"); // Input.GetAxisRaw("Horizontal");

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
        if ((Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("B_Button")) && IsGrounded())
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

    IEnumerator ResetAttackRoutine()
    {
        _resetAttack = true;
        yield return new WaitForSeconds(0.5f);
        _resetAttack = false;
    }

    // Moves the player
    private void FixedUpdate()
    {
        _playerRigidbody.velocity = new Vector2(move * Time.fixedDeltaTime * 10f * runSpeed, _playerRigidbody.velocity.y);
    }

    // Damages the player
    public void Damage()
    {
        // Decrease Health by 1
        Health--;

        // Update UI display to show the current amount of health correctly
        UIManager.Instance.UpdateLives(Health);
        
        if (Health < 1)
        {
            _playerAnimation.Death();
        }
        else
        {
            _playerAnimation.Hit();
        }
    }

    // Returns if the player is dead
    public bool IsPlayerDead()
    {
        return _isDead;
    }
}
