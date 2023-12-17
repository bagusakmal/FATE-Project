using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Idle : StateMachineBehaviour
{

    public float attackRange = 3f;
    public float closeRange = 3f;
    public float attackCooldown = 5f;
    public Vector3 attackOffset;
    private float cooldownTimer;
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
        cooldownTimer = 5f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();
        
        if (cooldownTimer <= 0f && Vector2.Distance(player.position, rb.position) <= closeRange && Vector2.Distance(player.position, rb.position) <= attackRange)
		{
			animator.SetTrigger("Attack2");
            cooldownTimer = attackCooldown;
		}
        else if (cooldownTimer <= 0f && Vector2.Distance(player.position, rb.position) <= attackRange && Vector2.Distance(player.position, rb.position) >= closeRange)
		{
			animator.SetTrigger("Attack");
            cooldownTimer = attackCooldown;
		}
        else
        {
            cooldownTimer -= Time.deltaTime; // Decrease cooldown timer over time
        }

       
       
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.ResetTrigger("Attack");
       animator.ResetTrigger("Attack2");
    }


    void OnDrawGizmosSelected()
	{	
        Vector3 pos = transform.position;
		Gizmos.DrawWireSphere(pos, attackRange);
	}

   
}
