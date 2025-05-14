using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;

    public bool isBusy { get; private set; }
    [Header("Move info")]
    public float moveSpeed = 12f;
    public float jumpForce;

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }
    public float crouchSpeed = 0.5f;
    public float swordReturnImpact = 2f;

    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }

    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerDashState dashState { get; private set; }

    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerCounterAttack counterAttack { get; private set; }
    public PlayerCrouchWalkState crouchWalkState { get; private set; }
    public PlayerCrouchAttack crouchAttackState { get; private set; }
    public PlayerCrouchIdle crouchIdle { get; private set; }
    public PlayerRollState rollState { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState playerCatchSwordState { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    public PlayerStunnedState stunnedState { get; private set; }
    #endregion

    public override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");
        crouchWalkState = new PlayerCrouchWalkState(this, stateMachine, "CrouchWalk");
        crouchAttackState = new PlayerCrouchAttack(this, stateMachine, "CrouchAttack");
        crouchIdle = new PlayerCrouchIdle(this, stateMachine, "CrouchIdle");
        rollState = new PlayerRollState(this, stateMachine, "Roll");
        stunnedState = new PlayerStunnedState(this, stateMachine, "Stunned");



        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttack(this, stateMachine, "CounterAttack");

        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        playerCatchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");

        deadState = new PlayerDeadState(this, stateMachine, "Die");
    }

    public override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);

    }


    public override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckForDashInput();
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void ClearSword()
    {
        Destroy(sword);
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return;



        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;


            stateMachine.ChangeState(dashState);
        }
    }

public void TakeDamage(EnemyBase enemy,bool isPhyscial)
{
    if (enemy == null || enemy.stats == null)
    {
        Debug.LogError("[Player] enemy or enemy.stats is null!");
        return;
    }

    if (stats == null)
    {
        Debug.LogError("[Player] stats (PlayerStats) is not assigned!");
        return;
    }

    enemy.stats.DoDamage(stats,true); // ✅ let the enemy damage the player’s stats
    entityFx?.Flash();           // ✅ visual feedback
}

public void StunPlayer(float duration)
{
    stunnedState.SetStunTime(duration);
    stateMachine.ChangeState(stunnedState);
}


public void Die()
{
        if(IsGroundDetected())
    stateMachine.ChangeState(deadState);
        this.tag = "Dead";
        SkillManager.instance.sword.DotsActive(false);
}    
    
}
