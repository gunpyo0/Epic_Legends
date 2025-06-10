using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : Hp
{

    private Reward reward;

    private void Start()
    {
        reward = GetComponent<Reward>();
    }

    protected override void Die()
    {
        reward.GiveReward();
        Destroy(gameObject);
    }

}
