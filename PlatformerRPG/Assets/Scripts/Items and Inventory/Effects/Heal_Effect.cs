using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal effect", menuName = "Data/Item Effect/Heal effect")]
public class Heal_Effect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercent;
    [SerializeField] private float duration = 5f; // 힐 효과 지속 시간
    [SerializeField] private float interval = 1f; // 힐 효과 간격

    public override void ExecuteEffect()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        playerStats.StartCoroutine(HealOverTime(playerStats));
    }

    private IEnumerator HealOverTime(PlayerStats playerStats)
    {
        float timePassed = 0f;

        while (timePassed < duration)
        {
            int healAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercent);
            playerStats.IncreaseHealthBy(healAmount);

            yield return new WaitForSeconds(interval);
            timePassed += interval;
        }
    }
}
