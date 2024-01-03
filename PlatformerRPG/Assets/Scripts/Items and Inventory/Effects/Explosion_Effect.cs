using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Freezing effect", menuName = "Data/Item Effect/Freezing Effect")]
public class Explosion_Effect : ItemEffect
{

    private float detectionRadius;

    public override void ExecuteEffect()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(playerStats.transform.position, detectionRadius);

        foreach (var hit in hitColliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();

                playerStats.DoDamage(_target);
            }
        }
    }
}
