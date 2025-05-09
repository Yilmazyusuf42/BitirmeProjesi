using UnityEngine;

public class EnemyAnimationTriggers : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    public void AnimationTrigger()
    {
        enemy?.stateMachine?.currentState?.AnimationTrigger();
    }

    public void AnimationFinishTrigger()
    {
        enemy?.stateMachine?.currentState?.AnimationFinishTrigger();
    }
}
