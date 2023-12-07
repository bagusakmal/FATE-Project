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
    public int damageAmount = 10;
    private bool isTakingDamage = false; // Flag untuk menandakan apakah pemain sedang mengalami damage
    public float damageInterval = 1.0f; // Interval waktu antara setiap serangan

    private void Start()
    {
        currentHealth = maxHealth;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        anim = GetComponent<Animator>();
    }

     private void Update()
    {
        // // Jika pemain sedang mengalami damage dan game tidak sedang di-pause
        // if (isTakingDamage && !isPaused)
        // {
        //     StartCoroutine(TakeDamageRepeatedly());
        // }
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spike")) // Ganti dengan tag yang sesuai pada tile spike
        {
            DecreaseHealth(damageAmount);
            isTakingDamage = true; // Pemain mengalami damage

        }
    }
     private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Spike")) // Ganti dengan tag yang sesuai pada tile spike
        {
            isTakingDamage = false; // Pemain meninggalkan area spike
        }
    }

    // private IEnumerator TakeDamageRepeatedly()
    // {
    //     while (isTakingDamage)
    //     {
    //         yield return new WaitForSeconds(damageInterval);

    //         DecreaseHealth(damageAmount);
    //     }
    // }

    private void Die()
    {
        die = true;
        anim.SetBool("Die", die);

        // Instantiate(deathChunkParticle, transform.position, deathChunkParticle.transform.rotation);
        // Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);
        GM.gameOver();
    }
}
