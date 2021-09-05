using UnityEngine;
using EnemyNamespace;

public class Mole : Enemy
{
    protected override void Start()
    {
        base.Start();
        EntityHeartCount = 10;
        InflictedDamage = 2;

        StartCoroutine(GetPath());
    }

    protected override void Move()
    {
        if (Path == null)
            return;

        if (PathIndex < Path.Count)
        {
            Vector3 Target = FloorTilemap.GetCellCenterWorld(FloorTilemap.WorldToCell(Path[PathIndex].Position));
            MovementVector = (Target - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, Target, EntitySpeed * Time.deltaTime);
            if (transform.position == Target)
                PathIndex++;
        }
    }
}
