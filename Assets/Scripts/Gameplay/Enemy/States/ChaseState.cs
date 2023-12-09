using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    private readonly Transform chaseTarget;
    private readonly float chaseSpeed;

    public ChaseState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform chaseTarget, float chaseSpeed)
        : base(entity, stateMachine, animBoolName)
    {
        this.chaseTarget = chaseTarget;
        this.chaseSpeed = chaseSpeed;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        ChaseTarget();
    }

    private void ChaseTarget()
    {
        if (chaseTarget != null)
        {
            float direction = Mathf.Sign(chaseTarget.position.x - entity.transform.position.x);
            entity.SetVelocity(chaseSpeed, Vector2.right, (int)direction);
        }
    }
}
