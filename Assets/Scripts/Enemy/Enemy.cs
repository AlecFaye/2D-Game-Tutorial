using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public GameObject diamondPrefab;

    [SerializeField] protected int health;
    [SerializeField] protected float speed;
    [SerializeField] protected int gems;

    [SerializeField] protected Transform pointA, pointB;

    protected Vector3 _currentTarget;
    protected Animator _animator;
    protected Player _player;

    protected bool _facingRight = true;
    protected float distance;
    protected bool isDead = false;

    private void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        _currentTarget = pointB.position;
        _animator = GetComponentInChildren<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public virtual void Update()
    {
        if (_player.IsPlayerDead())
        {
            _animator.SetBool("InCombat", false);
        }

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Hit") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (_animator.GetBool("InCombat"))
            {
                CheckDistance();

                Vector3 direction = _player.transform.localPosition - transform.localPosition;

                if (direction.x < 0 && _facingRight)
                {
                    Flip();
                }
                else if (direction.x > 0 && !_facingRight)
                {
                    Flip();
                }
            }
            return;
        }

        if (!isDead)
        {
            CheckDirection();

            Movement();
        }
    }

    public virtual void CheckDistance()
    {
        distance = Vector3.Distance(transform.localPosition, _player.transform.localPosition);
    }

    public virtual void CheckDirection()
    {
        if (_currentTarget == pointA.position && _facingRight)
        {
            Flip();
        }
        else if (_currentTarget == pointB.position && !_facingRight)
        {
            Flip();
        }
    }

    public void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        _facingRight = !_facingRight;
    }

    public virtual void Movement()
    {
        if (transform.position == pointA.position)
        {
            _currentTarget = pointB.position;
            _animator.SetTrigger("Idle");
        }
        else if (transform.position == pointB.position)
        {
            _currentTarget = pointA.position;
            _animator.SetTrigger("Idle");
        }

        transform.position = Vector3.MoveTowards(transform.position, _currentTarget, speed * Time.deltaTime);
    }
}
