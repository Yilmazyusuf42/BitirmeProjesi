using UnityEngine;

public class MedusaAnimationEvents : MonoBehaviour
{
    private Medusa medusa;

    private void Awake()
    {
        medusa = GetComponent<Medusa>();
    }

    // Called from AnimationEvent
    public void SnakeHoldFinished()
    {
        if (medusa.stateMachine.currentState is MedusaSnakeAttackState snakeState)
        {
            snakeState.SnakeHoldFinished(); // ✅ forward to the state
        }
    }
}
