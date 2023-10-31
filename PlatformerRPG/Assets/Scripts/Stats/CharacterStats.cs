using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum StatType
{
    health,
    stamina,
    staminaRecoverSpeed,
    damage,
    armor,
    TrueDamage,
}


public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major Stat")]
    public Stat strength; // 피해량, 치명타 피해 증가
    public Stat agility;  // 회피, 치명타 확률 증가
    public Stat intelligence; // 마법뎀, 저항 증가
    public Stat vitality; // 체력 증가

    [Header("Offensive Stats")]
    public Stat damage;
    public Stat trueDamage;
    public Stat recoveryStaminaSpeed;
    public Stat critChacne;
    public Stat critPower; // default 150

    [Header("Defencive stats")]
    public Stat maxHealth;
    public Stat maxHiddenHealth;
    public Stat maxStamina;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic stats")]
    public Stat posionDamage;
    public Stat iceDamage;
    public Stat lightingDamage;

    public bool isPoisoned; // 틱뎀
    public bool isChilled; // 적 빙결, 방감
    public bool isShocked; // 명중 감소

    public float poisonedTimer;
    public float chilledTimer;
    public float shockedTimer;

    private float poisonDamageCoolDown = 0.3f;
    private float posionDamageTimer;
    private int poisonDamage;

    public int currentHealth;
    public int currentHiddenHealth;
    public float currentStamina;

    public System.Action onHealthChanged;
    public System.Action onStaminaChanged;
    protected bool isDaed;

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();
        currentHiddenHealth = GetHiddenHealthValue();
        currentStamina = GetMaxStaminaValue();

        fx = GetComponent<EntityFX>();
    }
    
    protected virtual void Update()
    {
        poisonedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        posionDamageTimer -= Time.deltaTime;

        if (poisonedTimer < 0)
            isPoisoned = false;

        if (chilledTimer < 0)
            isChilled = false;

        if (shockedTimer < 0)
            isShocked = false;

        if (isPoisoned)
            ApplyPoisonDamage();

        RecoveryStamina();
    }

    public void RecoveryStamina()
    {
        if(currentStamina < GetMaxStaminaValue())
            currentStamina += (Time.deltaTime * recoveryStaminaSpeed.GetValue());

        if(currentStamina > GetMaxStaminaValue())
        {
            currentStamina = GetMaxStaminaValue();
        }

        if (onStaminaChanged != null)
            onStaminaChanged();
    }

    public void DecreaseStamianBy(float _stamina)
    {
        currentStamina -= _stamina;

        if(onStaminaChanged != null)
        {
            onStaminaChanged();
        }
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        //if (TargetCanAvoidAttack(_targetStats)) return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);

        DoMagicalDamage(_targetStats);
    }

    public virtual void DoTrueDamage(CharacterStats _targetStats)
    {
        int totalDamage = trueDamage.GetValue();

        _targetStats.TakeTrueDamage(totalDamage);
    }


    #region MagicalDamage
    // 마뎀 to ApplyAilments
    public virtual void DoMagicalDamage(CharacterStats _targetStatas)
    {
        int _poisonDamage = posionDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicalDamage = _poisonDamage + _iceDamage + _lightingDamage + intelligence.GetValue();

        totalMagicalDamage = CheckTargetResustance(_targetStatas, totalMagicalDamage);
        _targetStatas.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(_poisonDamage, _iceDamage, _lightingDamage) <= 0)
        {
            return;
        }

        bool canApplyPoison = _poisonDamage > _iceDamage && _poisonDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _poisonDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _poisonDamage && _lightingDamage > _iceDamage;

        AttemptyToApplyAilments(_targetStatas, _poisonDamage, _iceDamage, _lightingDamage, ref canApplyPoison, ref canApplyChill, ref canApplyShock);

    }

    private void AttemptyToApplyAilments(CharacterStats _targetStatas, int _poisonDamage, int _iceDamage, int _lightingDamage, ref bool canApplyPoison, ref bool canApplyChill, ref bool canApplyShock)
    {
        while (!canApplyPoison && !canApplyChill && !canApplyShock)
        {
            if (Random.value < 0.5f && _poisonDamage > 0)
            {
                canApplyPoison = true;
                _targetStatas.ApplyAilments(canApplyPoison, canApplyChill, canApplyShock);

                return;
            }

            if (Random.value < 0.5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStatas.ApplyAilments(canApplyPoison, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < 0.5f && _lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStatas.ApplyAilments(canApplyPoison, canApplyChill, canApplyShock);
                return;
            }
        }

        if (canApplyPoison)
            _targetStatas.SetupPoisonDamage(Mathf.RoundToInt(_poisonDamage * 0.2f));

        _targetStatas.ApplyAilments(canApplyPoison, canApplyChill, canApplyShock);
    }

    public void ApplyAilments(bool _poison, bool _chill, bool _shock)
    {
        if (isPoisoned || isChilled || isShocked)
            return;

        if(_poison)
        {
            poisonedTimer = 2f;
            isPoisoned = _poison;

            fx.poisonFxFor(poisonedTimer);
        }

        if (_chill)
        {
            chilledTimer = 2f;
            isChilled = _chill;

            //float slowPercentage = 0.5f;

            //GetComponent<Entity>().EntitySpeedChangeBy(slowPercentage, chilledTimer);
            Enemy enemy = GetComponent<Enemy>();
            enemy.StartCoroutine("FreezeTimeFor", chilledTimer);
            fx.ChillFxFor(chilledTimer);
        }

        if(_shock)
        {
            shockedTimer = 2f;
            isShocked = _shock;

            fx.ShockFxFor(shockedTimer);
        }
    }

    private void ApplyPoisonDamage()
    {
        if (posionDamageTimer < 0)
        {
            DecreaseHealthBy(poisonDamage);

            if (currentHealth < 0 && !isDaed)
                Die();

            posionDamageTimer = poisonDamageCoolDown;
        }
    }

    public void SetupPoisonDamage(int _damage) => poisonDamage = _damage;
    #endregion


    protected virtual void TakeDamage(int _damage)
    {

        DecreaseHealthBy(_damage);
        
        Entity target = GetComponent<Entity>();

        if (target.stats.isDaed == false)
        {
            target.DamageEffect();
            target.fx.StartCoroutine("FlashFX");
        }

        if (currentHealth < 0 && !isDaed)
            Die();

    }


    protected virtual void TakeTrueDamage(int _damage)
    {
        DecreaseHiddenHealthBy(_damage);

        Entity target = GetComponent<Entity>();

        if (currentHiddenHealth <= 0 && currentHealth <= 0 && !isDaed)
            Die();
    }

    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;

        if (currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();

        if(onHealthChanged != null)
            onHealthChanged();
    }

    public virtual void DecreaseHiddenHealthBy(int _damage)
    {
        currentHiddenHealth -= _damage;

        if (currentHiddenHealth > GetHiddenHealthValue())
            currentHiddenHealth = GetHiddenHealthValue();
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;

        if (onHealthChanged != null)
            onHealthChanged();
    }

    #region Defend Check
    // 총 체력 반환
    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

    public float GetMaxStaminaValue()
    {
        return maxStamina.GetValue();
    }

    public int GetHiddenHealthValue()
    {
        return maxHiddenHealth.GetValue();
    }

    // 회피기능
    private bool TargetCanAvoidAttack(CharacterStats _targetStats) 
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }

    // 총데미지 - 방어력, 최솟값 0
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if(_targetStats == null)
        {
            return 0;
        }

        if (_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f);
        else
            totalDamage -= _targetStats.armor.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    // 마뎀감
    private int CheckTargetResustance(CharacterStats _targetStatas, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStatas.magicResistance.GetValue() + (_targetStatas.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    
    #endregion


    #region Check Critical
    // 치확
    private bool CanCrit()
    {
        int totalCriticalChance = critChacne.GetValue() + agility.GetValue();

        if(Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    // 치피 계산
    private int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;

        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }
    #endregion


    protected virtual void Die()
    {
        isDaed = true;
    }

    public Stat GetStat(StatType statType)
    {
        if (statType == StatType.health) return maxHealth;
        else if (statType == StatType.stamina) return maxStamina;
        else if (statType == StatType.staminaRecoverSpeed) return recoveryStaminaSpeed;
        else if (statType == StatType.damage) return damage;
        else if (statType == StatType.armor) return armor;
        else if (statType == StatType.TrueDamage) return trueDamage;

        return null;
    }
}
