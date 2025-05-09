using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 1;
    public float lifetime = 3f;
    private Vector2 direction;

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
            if (other.TryGetComponent(out Player player))
            {
                player.TakeDamage(damage); // This assumes your Player script has a TakeDamage(int) method
            }

            Destroy(gameObject);
        }
    }
}
