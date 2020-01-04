using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    // Variables
    private Animator m_playerAnimator;
    private Animator m_swordAnimator;

    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        m_playerAnimator = transform.GetChild(0).GetComponent<Animator>();
        m_swordAnimator = transform.GetChild(1).GetComponent<Animator>();
    }

    public void Move(float move)
    {
        m_playerAnimator.SetFloat("Speed", Mathf.Abs(move));
    }

    public void Jump(bool jumping)
    {
        m_playerAnimator.SetBool("IsJumping", jumping);
    }

    public void Attack()
    {
        m_playerAnimator.SetTrigger("Attack");
        m_swordAnimator.SetTrigger("SwordAnimation");
    }

    public void Hit()
    {
        m_playerAnimator.SetTrigger("Hit");
    }

    public void Death()
    {
        m_playerAnimator.SetTrigger("Death");
        isDead = true;
    }

    public bool PlayerDead()
    {
        return isDead;
    }
}
