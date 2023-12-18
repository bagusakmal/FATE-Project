using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public float health = 500;

	public GameObject deathEffect;
	public GameObject hitParticle;
	public GameObject Key;

	public bool isInvulnerable = false;

    public GameObject Alive;
	public GameObject Dialogue;



	public void Damage(AttackDetails attackDetails)
	{
        
		if (isInvulnerable)
			return;

        float damage = attackDetails.damageAmount;
		health -= damage;
        // stunres -= attackDetails.stunDamageAmount;
		Instantiate(hitParticle, Alive.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

		if (health <= 200)
		{
			Alive.GetComponent<Animator>().SetBool("IsEnraged", true);
		}

		if (health <= 0)
		{
			Die();
		}
	}

	void Die()
	{	
		Alive.GetComponent<Transform>();
		Instantiate(deathEffect, Alive.transform.position,Alive.transform.rotation);
		Instantiate(Key, Alive.transform.position,Alive.transform.rotation);
		Destroy(gameObject);
		Dialogue.SetActive(true);
	}
}
