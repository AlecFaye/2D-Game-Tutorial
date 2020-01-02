using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy, IDamageable
{
    public int Health { get; set; }

    // Used for initialization
    public override void Init()
    {
        base.Init();

        Health = base.health;
    }

    public void Damage()
    {
        Health--;

        if (Health < 1)
        {
            Destroy(this.gameObject);
        }

        _animator.SetTrigger("Hit");
        _animator.SetBool("InCombat", true);
    }

    public override void CheckDistance()
    {
        base.CheckDistance();
        
        if (distance > 2.0f)
        {
            _animator.SetBool("InCombat", false);
        }
    }
}
