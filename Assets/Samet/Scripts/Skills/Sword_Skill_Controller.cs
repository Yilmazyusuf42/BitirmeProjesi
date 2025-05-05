using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    [SerializeField] private float returnSpeed=12;
    private Animator anim;
    private Rigidbody2D rb;
    private CapsuleCollider2D cd;
    private Player player;

    private bool canRotate=true;
    private bool isReturning;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd= GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity.normalized;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.ClearSword();
        }
    }
    public void SetSword(Vector2 _dir, float _gravityScale,Player _player)
    {
       player = _player;

        rb.velocity = _dir;
        rb.gravityScale= _gravityScale;

        anim.SetBool("Rotation", true);
    }

    public void ReturnSword()
    {
        rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;

    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent =collision.transform;
    }
}
