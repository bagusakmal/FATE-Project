using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_ChaseState : ChaseState
{
    public B1_ChaseState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform chaseTarget, float chaseSpeed)
        : base(entity, stateMachine, animBoolName, chaseTarget, chaseSpeed)
    {
        // Additional initialization specific to B1_ChaseState if needed
    }

    // Add any additional methods or overrides specific to B1_ChaseState
    // For example, you might override Enter, Exit, or other methods if needed
     public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
