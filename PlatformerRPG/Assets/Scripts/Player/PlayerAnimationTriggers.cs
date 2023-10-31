using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>() ;

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadious);

        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null || hit.GetComponent<WorldObject>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                ObjectStats _targetObject = hit.GetComponent<ObjectStats>();

                if (_target != null)
                {
                    player.stats.DoDamage(_target);
                }
                else if (_targetObject != null)
                {
                    player.stats.DoTrueDamage(_targetObject);
                }

                ItemData_Equipment weaponData = Inventory.Instance.GetEquipment(EquipmentType.Weapon);

                if (weaponData != null)
                {
                    weaponData.ExcuteItemEffect();
                }
            }
        }
    }

    private void WeaopnEffect()
    {
        
    }

    private void ThorwSword()
    {
        SkillManager.instance.sword.CreateSwrod();
    }

}
