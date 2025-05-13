public class PlayerStats : CharacterStats
{
    private Player player;
    private EntityFx fx;

    protected override void Start()
    {
        base.Start();
        fx = GetComponent<EntityFx>();
        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        fx?.Flash(); // ✅ fixed: use correct field name
    }

    protected override void Die()
    {
        base.Die();
        player.Die();
    }
}
