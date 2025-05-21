using UnityEngine;

public class Fireball : MonoBehaviour
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

        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out Player player))
            {
                owner.stats.DoMagicalDamage(player.stats);
                Debug.Log("🔥 Fireball hit player");
            }

            hasHit = true;
        }

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
