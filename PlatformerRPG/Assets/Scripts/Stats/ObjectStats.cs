public class ObjectStats : CharacterStats
{
    WorldObject _worldObject;
    ItemDrop myDropSystem;

    protected override void Start()
    {
        base.Start();

        _worldObject = GetComponent<WorldObject>();
        myDropSystem = GetComponent<ItemDrop>();
    }

    protected override void TakeDamage(int _damage)
    {
    }

    protected override void TakeTrueDamage(int _damage)
    {
        base.TakeTrueDamage(_damage);

    }

    protected override void Die()
    {
        base.Die();
        _worldObject.Die();

        myDropSystem.GenerateDrop();
    }
}