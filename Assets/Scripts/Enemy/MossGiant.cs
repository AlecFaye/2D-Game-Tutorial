using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MossGiant : Enemy, IDamageable
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
        if (isDead)
        {
            return;
        }

        Health--;

        if (Health < 1)
        {
            _animator.SetTrigger("Death");
            isDead = true;
            GameObject diamond = Instantiate(diamondPrefab, transform.position, Quaternion.identity) as GameObject;
            diamond.GetComponent<Diamond>().gems = base.gems;
        }

        _animator.SetTrigger("Hit");
        _animator.SetBool("InCombat", true);
    }

    public override void CheckDistance()
    {
        base.CheckDistance();

        if (distance > 3.0f)
        {
            _animator.SetBool("InCombat", false);
        }
    }
}
