using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private GameObject deathChunkParticle, hitParticle, deathBloodParticle;
    public GameObject pauseMenu;

    private float currentHealth;

    private Animator anim;

    private GameManager GM;
    private bool die = false;
    private bool isPaused = false;

    private void Start()
    {
        currentHealth = maxHealth;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        anim = GetComponent<Animator>();
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0.0f && !die)
        {
            Die();
        }
        else
        {
            Instantiate(hitParticle, anim.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
            anim.SetTrigger("damage");
        }
    }

    private void Die()
    {
        die = true;
        anim.SetBool("Die", die);

        // Instantiate(deathChunkParticle, transform.position, deathChunkParticle.transform.rotation);
        // Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);
        GM.gameOver();
    }
}
