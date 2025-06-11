using UnityEngine;

public class Chargeball : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 direction;
    private bool hasHit = false;

    public LayerMask whatIsGround; // ✅ Add in Inspector
    public EnemyBase owner;

    public void SetDirection(Vector2 dir)
    {
        // ✅ Force direction to horizontal only
        direction = new Vector2(Mathf.Sign(dir.x), 0f);
        transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
    }

    void Update()
    {
        if (hasHit)
            return;

        // ✅ Move
        transform.Translate(direction * speed * Time.deltaTime);

        // ✅ Check for wall hit
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.1f, whatIsGround);
        if (hit.collider != null)
        {
            hasHit = true;
        }
    }

private void OnTriggerEnter2D(Collider2D other)
{
    if (hasHit)
        return;

    // ✅ Only hurt player
    if (other.CompareTag("Player"))
    {
        if (other.TryGetComponent(out Player player))
        {
            owner.stats.DoMagicalDamage(player.stats);
            Debug.Log("🔥 Fireball hit player");
        }

        hasHit = true;
        return;
    }

    // ✅ Ignore enemies entirely
    if (other.CompareTag("Enemy"))
        return;

    // ✅ Ignore other triggers (e.g. hitboxes, attack zones)
    if (other.isTrigger)
        return;

    // ✅ Everything else (e.g. walls, ground) counts as a hit
    hasHit = true;
}


    // Called by animation event
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
