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
    private Transform sword;

    [Header("Pierce Info")]
    [SerializeField] private int amountOfPierce;


    [Header("Bounce info")]
    [SerializeField] private float bounceSpeed;
    private bool isBouncing;
    private int amountOfBounce;
    private List<Transform> enemyTargets;
    private int targetIndex;
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
            {
                player.ClearSword();
                sword = player.sword.transform;

                if (player.transform.position.x > sword.transform.position.x && player.facingDir == 1)
                    player.Flip();
                else if (player.transform.position.x < sword.transform.position.x && player.facingDir == -1)
                    player.Flip();
            }
        }
        BounceLogic();
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTargets.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTargets[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTargets[targetIndex].position) < .1f)
            {
                targetIndex++;
                amountOfBounce--;
                if (amountOfBounce <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }
                if (targetIndex >= enemyTargets.Count)
                    targetIndex = 0;
            }
        }
    }

    public void SetSword(Vector2 _dir, float _gravityScale,Player _player)
    {
       player = _player;

        rb.velocity = _dir;
        rb.gravityScale= _gravityScale;

        anim.SetBool("Rotate", true);
    }

    public void SetupBounce(bool _isBouncing,int _amountOfBounces)
    {
        isBouncing= _isBouncing;
        amountOfBounce= _amountOfBounces;

        enemyTargets = new List<Transform>();
    }

    public void SetupPierce(int _amountOfPierce)
    {
        amountOfPierce=_amountOfPierce;
    }
    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        anim.SetBool("Rotate", true);
        transform.parent = null;
        isReturning = true;

    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;

      //  collision.GetComponent<Enemy>()?.Damage(); //eneemy damage

        if (collision.tag == "Enemy")
        {
            if (isBouncing && enemyTargets.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.tag == "Enemy")
                        enemyTargets.Add(hit.transform);
                }
            }
        }
        StukInto(collision);
    }

    private void StukInto(Collider2D collision)
    {
        if (amountOfPierce > 0 && collision.tag == "Enemy")
        {
            amountOfPierce--;
            return;
        }
          
 
        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing &&enemyTargets.Count>0)
            return;

        anim.SetBool("Rotate", false);
        transform.parent = collision.transform;
    }
}
