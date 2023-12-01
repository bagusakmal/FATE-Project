using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private GameObject deathChunkParticle, deathBloodParticle;
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
    }

    private void Die()
    {
        die = true;
        anim.SetBool("Die", die);

        // Instantiate(deathChunkParticle, transform.position, deathChunkParticle.transform.rotation);
        // Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);
        PauseGame();
    }

    private void Update()
    {
        // Move this part to Update to listen for Escape key
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            GM.Respawn();
            ResumeGame(); // Add this line to resume the game after respawn
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1; // Set timeScale back to 1 to resume the game
        isPaused = false;
        pauseMenu.SetActive(false);
    }
}
