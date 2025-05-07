using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState 
{
    protected Rigidbody2D rb;
    protected EnemyStateMachine enemyStateMachine;
    protected Enemy enemyBase;

    private string animBoolName;
    protected bool triggerCalled;

    protected float stateTimer;
    protected EnemyStateMachine stateMachine;

    public EnemyState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName)
    {
        this.stateMachine = stateMachine; // ✅ this is what your states are actually using
        this.enemyBase = enemyBase;
        this.animBoolName = animBoolName;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
    public virtual void Enter()
    {   
        rb = enemyBase.rb;
        triggerCalled = false;
        enemyBase.anim.SetBool(animBoolName, true);
        Debug.Log($"[FSM] Entered state: {animBoolName} on {enemyBase.name}");
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
    }
    
    public void AnimationTrigger()
    {
        triggerCalled = true;
    }
    public virtual void AnimationFinishTrigger() 
    { 

    }

}
