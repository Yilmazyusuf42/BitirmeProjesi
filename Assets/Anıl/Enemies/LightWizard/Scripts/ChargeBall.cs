using UnityEngine;

public class Chargeball : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 direction;
    private bool hasHit = false;

    public EnemyBase owner;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
    }

    void Update()
    {
        if (!hasHit)
            transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit)
            return;

        Debug.Log("🔥 Fireball hit: " + other.name);

        // Damage player only
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out Player player))
            {
                Debug.Log("💥 Fireball is calling TakeDamage()");
                owner.stats.DoMagicalDamage(player.stats); // ✅ Ensure Player.cs has public TakeDamage(EnemyBase enemy)
            }

            hasHit = true;
        }

        // Stop on any solid object
        if (!other.isTrigger)
        {
            hasHit = true;
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
