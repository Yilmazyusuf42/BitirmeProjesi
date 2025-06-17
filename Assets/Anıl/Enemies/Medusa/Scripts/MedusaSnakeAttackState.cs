using UnityEngine;
using System.Collections;

public class MedusaSnakeAttackState : EnemyState
{
    private Medusa enemy;
    private Coroutine damageCoroutine;

    private float damageInterval = 0.5f;
    private int damagePerTick = 3;

    private float holdDuration = 3f; // ⏳ how long Medusa holds the player
    private float extraStunBuffer = 0.5f; // 🧠 how long the player stays stunned after release
    private bool hasReleased = false;

    public MedusaSnakeAttackState(EnemyStateMachine stateMachine, EnemyBase enemyBase, string animBoolName, Medusa enemy)
        : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        hasReleased = false;

        enemy.SetZeroVelocity();
        enemy.anim.ResetTrigger("SnakeAttack");
        enemy.anim.SetTrigger("SnakeAttack");

        if (enemy.player != null && enemy.player.TryGetComponent(out Player player))
        {
            float totalStun = holdDuration + extraStunBuffer;
            player.stunnedState.SetStunTime(totalStun);
            player.stateMachine.ChangeState(player.stunnedState);

            player.transform.position = enemy.holdPoint.position;
            player.transform.SetParent(enemy.transform);

            EntityFx fx = player.GetComponent<EntityFx>();

            damageCoroutine = enemy.StartCoroutine(DoDamageOverTime(player.stats, fx, totalStun));


        }
    }

// Triggered by animation event at END of the hold animation
public void SnakeHoldFinished()
{
    if (hasReleased) return;
    hasReleased = true;

    if (enemy.player != null)
        enemy.player.transform.SetParent(null);

    // Freeze Medusa's current animation pose
    enemy.anim.speed = 0f;

    if (damageCoroutine != null)
        enemy.StopCoroutine(damageCoroutine);

    // Wait for the player's stun duration before exiting
    enemy.StartCoroutine(WaitForPlayerStunToEnd());
}

private IEnumerator WaitForPlayerStunToEnd()
{
    // Match the player's total stun time
    float waitTime = holdDuration + extraStunBuffer; // must match the stun value set in Enter()
    yield return new WaitForSeconds(waitTime);

    // Resume animation
    enemy.anim.speed = 1f;

    // Resume behavior
    stateMachine.ChangeState(enemy.battleState);
}

private IEnumerator DoDamageOverTime(CharacterStats targetStats, EntityFx fx, float duration)
{
    float timer = 0f;

    while (timer < duration)
    {
        if (targetStats.currentHp > 0)
        {
            targetStats.TakeDamage(damagePerTick);
            if (fx != null)
                fx.Flash();

        }

        yield return new WaitForSeconds(damageInterval);
        timer += damageInterval;
    }
}


}
