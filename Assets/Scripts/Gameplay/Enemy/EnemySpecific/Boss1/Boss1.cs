using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : Entity
{
    public B1_IdleState idleState { get; private set; }
    public B1_MoveState moveState { get; private set; }
    public B1_PlayerDetectedState playerDetectedState { get; private set; }
    public B1_ChargeState chargeState { get; private set; }
    public B1_LookForPlayerState lookForPlayerState { get; private set; }
    public B1_MeleeAttackState meleeAttackState { get; private set; }
    public B1_StunState stunState { get; private set; }
    public B1_DeadState deadState { get; private set; }

    public B1_ChaseState chaseState { get; private set; }

    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private float chaseSpeed;
    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_PlayerDetected playerDetectedData;
    [SerializeField]
    private D_ChargeState chargeStateData;
    [SerializeField]
    private D_LookForPlayer lookForPlayerStateData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    [SerializeField]
    private D_StunState stunStateData;
    [SerializeField]
    private D_DeadState deadStateData;


    [SerializeField]
    private Transform meleeAttackPosition;

    public override void Start()
    {
        base.Start();

        moveState = new B1_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new B1_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new B1_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedData, this);
        chargeState = new B1_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        lookForPlayerState = new B1_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        meleeAttackState = new B1_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        stunState = new B1_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new B1_DeadState(this, stateMachine, "dead", deadStateData, this);
        // chaseState = new B1_ChaseState(this, stateMachine, "chase", playerTransform, chaseSpeed);

        stateMachine.Initialize(moveState);
       
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);

        // Draw a wire sphere around the OverlapCircle area for visual representation
        Gizmos.DrawWireSphere(playerCheck.position, entityData.minAgroDistance);

        // Perform OverlapCircle to check for player detection
        Collider2D[] detectedPlayers = Physics2D.OverlapCircleAll(playerCheck.position, entityData.minAgroDistance, entityData.whatIsPlayer);

        // Check if any player is detected
        if (detectedPlayers.Length > 0)
        {
        // Do something when player is detected (e.g., change Gizmos color)
        Gizmos.color = Color.red;
        }
        else
        {
        // Do something when no player is detected (e.g., change Gizmos color)
        Gizmos.color = Color.green;
        }

        // Draw the actual OverlapCircle area
        Gizmos.DrawWireSphere(playerCheck.position, 0.1f);

        // Reset Gizmos color for other potential Gizmos draws
        Gizmos.color = Color.white;
    }

    //     public override void Update()
    //     {
    //     base.Update();

    //     // Check if player is in aggro range to initiate the chase
    //     if (CheckPlayerInMinAgroRange())
    //     {
    //         stateMachine.ChangeState(chaseState);
    //     }
    // }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);

        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }
        else if (isStunned && stateMachine.currentState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }        
    }
}
