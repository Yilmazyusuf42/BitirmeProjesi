using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 3f;
    private Vector2 direction;

    public EnemyRanged ownerEnemy; // ✅ Shooter of the arrow

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
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
                player.TakeDamage(ownerEnemy); // ✅ Pass full enemy reference
            }
            else
            {
                Debug.LogWarning("[Arrow] Player does not have Player component!");
            }

            Destroy(gameObject);
        }
    }
}
