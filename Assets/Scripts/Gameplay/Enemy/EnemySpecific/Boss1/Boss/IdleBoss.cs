using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBoss : StateMachineBehaviour
{    
    public float attackRange = 3f;
    public float closeRange = 3f;
    public float attackCooldown = 5f;
    public float boomCooldown = 5f;
    public Vector3 attackOffset;
    private float cooldownTimer;
    private float boomTimer;
	Transform player;
	Rigidbody2D rb;
    Transform transform;
	Boss boss;
      // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
		rb = animator.GetComponent<Rigidbody2D>();
		boss = animator.GetComponent<Boss>();
        cooldownTimer = 10f;
        boomTimer = 15f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       boss.LookAtPlayer();
        
        if (boomTimer <= 0f && Vector2.Distance(player.position, rb.position) <= attackRange)
		{
			animator.SetTrigger("Attack2");
            boomTimer = boomCooldown;
		}
        else if (cooldownTimer <= 0f && Vector2.Distance(player.position, rb.position) <= attackRange)
		{
			animator.SetTrigger("Attack");
            cooldownTimer = attackCooldown;
		}
        else
        {
            cooldownTimer -= Time.deltaTime;
            boomTimer -= Time.deltaTime; // Decrease cooldown timer over time
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.ResetTrigger("Attack");
       animator.ResetTrigger("Attack2");
    }
}
