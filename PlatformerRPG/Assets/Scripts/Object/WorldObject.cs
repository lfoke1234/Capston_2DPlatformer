public class WorldObject : Entity
{
    public override void Die()
    {
        Destroy(gameObject);
    }

    protected override void OnDrawGizmos()
    {
    }
}