using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class CloneSkillController : MonoBehaviour
{

    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] public float colorLoosingSpeed;

    private float cloneTimer;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackRadius=.8f;
    private Transform closestEnemy;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            sr.color= new Color(1,1,1,sr.color.a-(Time.deltaTime*colorLoosingSpeed));

            if (sr.color.a <= 0)
                Destroy(gameObject);
        }
    }
    public void SetUpClone(Transform _newTransform,float _cloneDuration,bool canAttack)
    {
        if (canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 3));

        transform.position = _newTransform.position;
        cloneTimer = _cloneDuration;


        FaceClosestTarget();
    }
    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<EnemyBase>()!=null)
            {
                hit.GetComponent<EnemyBase>().TakeDamage();
            }
        }
    }
    private void FaceClosestTarget()
    {
        Collider2D[] colliders= Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<EnemyBase>()!=null)
            {
                float distanceToEnemy= Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestEnemy = hit.transform;
                }
            }
        }
        if(closestEnemy != null)
        {
            if(transform.position.x>closestEnemy.position.x)
            {
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
