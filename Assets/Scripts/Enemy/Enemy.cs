using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected int speed;
    [SerializeField] protected int gems;

    [SerializeField] protected Transform pointA, pointB;

    protected Vector3 _currentTarget;
    protected Animator _animator;
    protected bool _facingRight = true;

    private void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        _currentTarget = pointB.position;
        _animator = GetComponentInChildren<Animator>();
    }

    public virtual void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            return;

        CheckDirection();

        Movement();
    }

    public virtual void CheckDirection()
    {
        if (_currentTarget == pointA.position && _facingRight)
        {
            Flip();
            _facingRight = false;
        }
        else if (_currentTarget == pointB.position && !_facingRight)
        {
            Flip();
            _facingRight = true;
        }
    }

    public void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
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
