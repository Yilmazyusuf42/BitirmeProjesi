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
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit)
            return;

        Debug.Log("⚡ Chargeball hit: " + other.name);

        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out Player player))
            {
                Debug.Log("💥 Chargeball is applying magic damage!");
                owner.stats.DoMagicalDamage(player.stats);
            }

            hasHit = true;
        }

        // Stop on any solid object
        if (!other.isTrigger)
        {
            hasHit = true;
        }
    }

    // Called by animation event
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
