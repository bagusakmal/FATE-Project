using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
     protected D_DeadState stateData;

    public DeadState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData) : base(etity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        GameObject.Instantiate(stateData.deathBloodParticle, entity.aliveGO.transform.position, entity.aliveGO.transform.rotation);
        GameObject.Instantiate(stateData.deathChunkParticle, entity.aliveGO.transform.position, stateData.deathChunkParticle.transform.rotation);

       entity.aliveGO.SetActive(false);
    }

    public override void Exit()
    {
        base.Exit();
        // entity.aliveGO.SetActive(false);
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
