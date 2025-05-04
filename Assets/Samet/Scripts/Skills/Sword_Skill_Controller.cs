using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd= GetComponent<CircleCollider2D>();
    }

    public void SetSword(Vector2 _dir, float _gravityScale)
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D eksik!");
            return;
        }
        rb.velocity = _dir;
        rb.gravityScale= _gravityScale;
    }
}
