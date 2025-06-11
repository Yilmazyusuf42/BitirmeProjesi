using UnityEngine;

public class EnemyLedgeWatcher : MonoBehaviour
{
    private EnemyBase enemy;
    private float ledgeCheckCooldown = 1f; // Time after spawn or transition to avoid early ledge check

    private void Awake()
    {
        enemy = GetComponent<EnemyBase>();
    }

    private void FixedUpdate()
    {
        if (enemy == null || enemy.isDead || GameState.isPlayerDead)
            return;

        var current = enemy.stateMachine.currentState;

        bool isAggressiveState =
            current == enemy.attackState ||
            current == enemy.battleState;

        if (!isAggressiveState)
            return;

        // Avoid early ledge check after spawn or re-entry into aggressive state
        if (Time.time - enemy.lastTimeLedgeAbort < ledgeCheckCooldown)
            return;

        if (!enemy.IsGroundAhead())
        {
            enemy.SetZeroVelocity();
            enemy.lastTimeLedgeAbort = Time.time;
            enemy.stateMachine.ChangeState(enemy.patrolState);
        }
    }
}
