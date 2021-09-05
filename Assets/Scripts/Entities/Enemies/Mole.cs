using UnityEngine;
using EnemyNamespace;

public class Mole : Enemy
{
    protected override void Start()
    {
        base.Start();
        EntityHeartCount = 10;
        InflictedDamage = 2;
    }
}
