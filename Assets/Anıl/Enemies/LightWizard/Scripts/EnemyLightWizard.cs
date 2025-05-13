using UnityEngine;

public class EnemyLightWizard : EnemyHybrid
{
    public EnemyIdleState idleState { get; private set; }
    public EnemyMoveState moveState { get; private set; }
    public EnemyLightWizardBattleState battleStateInternal { get; private set; }
    public EnemyLightWizardAttackState attackStateInternal { get; private set; }
    public EnemyLightWizardCastState castState { get; private set; }
    public EnemyStunnedState stunnedStateInternal { get; private set; }
    public EnemyPatrolState patrolStateInternal { get; private set; }

    public override EnemyState battleState => battleStateInternal;
    public override EnemyState patrolState => patrolStateInternal;
    public override EnemyState attackState => attackStateInternal;
    public override EnemyState stunnedState => stunnedStateInternal;
    public override EnemyState idle => idleState;

    public override float stunDuration => 1f;
    private float magicCooldown = 3f;
    private float lastMagicTime = Mathf.NegativeInfinity;
    public Transform chargeballSpawnPoint;
    public GameObject chargeballPrefab;
    public float chargeballSpeed = 10f;
    
    [HideInInspector]
    public int lastAttackIndex = -1;



    public override void Awake()
    {
        base.Awake();

        idleState = new EnemyIdleState(stateMachine, this, "PlayIdle");
        moveState = new EnemyMoveState(stateMachine, this, "PlayRun");
        battleStateInternal = new EnemyLightWizardBattleState(stateMachine, this, "PlayIdle", this);
        attackStateInternal = new EnemyLightWizardAttackState(stateMachine, this, "Attack", this);
        castState = new EnemyLightWizardCastState(stateMachine, this, "", this);
        stunnedStateInternal = new EnemyStunnedState(stateMachine, this, "Stunned");
        patrolStateInternal = new EnemyPatrolState(stateMachine, this, "PlayWalk");
    }

    public override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        spawnPosition = transform.position;
    }

    public override bool CanBeStunned()
    {
        if (canBeStunned)
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }

    public bool CanUseMagic()
    {
        return Time.time >= lastMagicTime + magicCooldown;
    }

    public void SetMagicCooldown()
    {
        lastMagicTime = Time.time;
    }

        public void SpawnChargeball()
    {
        if (chargeballPrefab == null || chargeballSpawnPoint == null)
        {
            Debug.LogWarning("[LightWizard] Missing fireball prefab or spawn point.");
            return;
        }

        GameObject obj = Instantiate(chargeballPrefab, chargeballSpawnPoint.position, Quaternion.identity);
        Chargeball fb = obj.GetComponent<Chargeball>();

        Vector2 direction = PlayerManager.instance.player.transform.position - chargeballSpawnPoint.position;
        fb.SetDirection(direction);
        fb.owner = this;
    }

}
