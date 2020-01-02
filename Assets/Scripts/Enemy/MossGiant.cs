using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MossGiant : Enemy
{
    private Vector3 _currentTarget;
    private Animator _animator;
    private bool _facingRight = true;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _currentTarget = pointB.position;
    }

    public override void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            return;
        }

        CheckDirection();

        Movement();
    }
    
    private void Movement()
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

    private void CheckDirection()
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

    private void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
