using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< Updated upstream
public class EnemyHp : Hp
{
    protected override void Die()
    {

    }
=======
public class EnemyHp : Hp, IRewardable
{
    protected override void Die()
    {
        Reward();
    }

    public void Reward()
    {
        
    }

>>>>>>> Stashed changes
}
