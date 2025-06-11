using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 3f;
    private Vector2 direction;

    public LayerMask whatIsGround; // ✅ For wall detection
    public EnemyRanged ownerEnemy;

    public void SetDirection(Vector2 dir)
    {
        // ✅ Force direction to be strictly horizontal (left or right)
        direction = new Vector2(Mathf.Sign(dir.x), 0f);

        transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);

        if (TryGetComponent(out SpriteRenderer sr))
            sr.enabled = true;

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // ✅ Move
        transform.Translate(direction * speed * Time.deltaTime);

        // ✅ Raycast forward to check for wall collision
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.1f, whatIsGround);
        if (hit.collider != null)
        {
            Destroy(gameObject); // Hit wall or environment
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (ownerEnemy == null)
            {
                Debug.LogError("[Arrow] ownerEnemy is not set! Cannot apply damage.");
                Destroy(gameObject);
                return;
            }

            if (other.TryGetComponent(out Player player))
            {
                player.TakeDamage(ownerEnemy, true);
            }
            else
            {
                Debug.LogWarning("[Arrow] Player does not have Player component!");
            }

            Destroy(gameObject);
        }
    }
}
