using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;    // Speed for left/right movement
    public float jumpForce = 5f;    // Jump strength
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Horizontal movement (A/D or Left/Right arrows)
        float moveInput = Input.GetAxisRaw("Horizontal"); // Returns -1, 0, or 1
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Jump (Space key)
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    bool IsGrounded()
    {
        // Raycast down to check if on ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, LayerMask.GetMask("Ground"));
        return hit.collider != null; // True if it hits something on the "Ground" layer
    }
}