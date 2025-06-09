using UnityEngine;

public class EnemyLedgeWatcher : MonoBehaviour
{
    private EnemyBase enemy;

    private void Awake()
    {
        enemy = GetComponent<EnemyBase>();
    }

    private void FixedUpdate()
    {
        if (enemy == null || enemy.isDead || GameState.isPlayerDead)
            return;

        // Only check during combat-related states
        var current = enemy.stateMachine.currentState;
        bool isAggressiveState =
            current == enemy.attackState ||
            current == enemy.battleState;

        if (!isAggressiveState)
            return;

        // 🛑 Abort movement or attack if no ground ahead
        if (!enemy.IsGroundAhead())
        {
            enemy.SetZeroVelocity();
            enemy.lastTimeLedgeAbort = Time.time; 
            enemy.stateMachine.ChangeState(enemy.patrolState); // or patrolState if safer
        }
    }
}
