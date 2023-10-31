using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    Enemy enemy;

    [Header("Level details")]
    [SerializeField] private int level;
    [SerializeField] private int damageStatsModifireWithLevel;
    [SerializeField] private int maxHealthStatsModifireWithLevel;
    [SerializeField] private int armorStatsModifireWithLevel;

    private ItemDrop myDropSystem;

    protected override void Start()
    {
        ApplyLevelModifires();

        base.Start();

        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();

    }


    #region Add modifire with level

    private void DamageModify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            int modifire = damageStatsModifireWithLevel;

            _stat.AddModifiers(modifire);
        }
    }

    private void HealthModify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            int modifire = maxHealthStatsModifireWithLevel;

            _stat.AddModifiers(modifire);
        }
    }

    private void ArmorModify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            int modifire = armorStatsModifireWithLevel;

            _stat.AddModifiers(modifire);
        }
    }
    private void ApplyLevelModifires()
    {
        DamageModify(damage);
        HealthModify(maxHealth);
        ArmorModify(armor);
    }

    #endregion

    protected override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();

        myDropSystem.GenerateDrop();
    }
}
