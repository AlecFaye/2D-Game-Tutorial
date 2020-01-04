using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Enemy, IDamageable
{
    public GameObject acidEffectPrefab;

    public int Health { get; set; }

    // Used for initialization
    public override void Init()
    {
        base.Init();

        Health = base.health;
    }

    public override void Update() {}

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
    }

    public void Attack()
    {
        Instantiate(acidEffectPrefab, transform.position, Quaternion.identity);
    }
}
