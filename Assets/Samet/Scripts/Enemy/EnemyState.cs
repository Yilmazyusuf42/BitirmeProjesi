using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyState 
{
    protected Rigidbody2D rb;
    protected EnemyStateMachine enemyStateMachine;
    protected Enemy enemyBase;

    private string animBoolName;
    protected bool triggerCalled;

    protected float stateTimer;

    public EnemyState(EnemyStateMachine enemyStateMachine, Enemy enemyBase, string animBoolName)
    {
        this.enemyStateMachine = enemyStateMachine;
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
    }
    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
    }
    
    public void AnimationTrigger()
    {
        triggerCalled = true;
    }
}
