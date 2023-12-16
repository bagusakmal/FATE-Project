using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public float health = 500;

	public GameObject deathEffect;

	public bool isInvulnerable = false;

    

	public void Damage(AttackDetails attackDetails)
	{

		if (isInvulnerable)
			return;

        float damage = attackDetails.damageAmount;
		health -= damage;
        // stunres -= attackDetails.stunDamageAmount;

		if (health <= 200)
		{
			GetComponent<Animator>().SetBool("IsEnraged", true);
		}

		if (health <= 0)
		{
			Die();
		}
	}

	void Die()
	{
		Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}
