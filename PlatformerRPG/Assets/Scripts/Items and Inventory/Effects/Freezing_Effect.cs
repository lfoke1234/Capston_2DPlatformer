using UnityEngine;

[CreateAssetMenu(fileName = "Freezing effect", menuName = "Data/Item Effect/Freezing Effect")]
public class Freezing_Effect : ItemEffect
{
    [SerializeField] private float freezingTime;
    [SerializeField] private float detectionRadius = 5f; // 감지 반경

    public override void ExecuteEffect()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(playerStats.transform.position, detectionRadius);

        foreach (var hit in hitColliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();

                playerStats.iceDamage.AddModifiers(1);
                playerStats.DoMagicalDamage(_target);
                playerStats.iceDamage.RemoveModifiers(1);
            }
        }

    }
}
