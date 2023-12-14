using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrijectilePlayer : MonoBehaviour
{
    private float speed;
    private float maxTravelDistance;
    private float damage;

    [SerializeField]
    private LayerMask whatIsDamageable;
    
    [SerializeField]
    private LayerMask whatIsGround;
    private AttackDetails attackDetails;
    [SerializeField]
    private float damageRadius;
     [SerializeField]
    private Transform damagePosition;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    private void Update()
    {
        // Hancurkan proyektil jika melebihi jarak maksimum
        if (Mathf.Abs(transform.position.x) >= maxTravelDistance)
        {
             Debug.Log("Proyektil melebihi jarak maksimum. Menghancurkan proyektil.");
            Destroy(gameObject);
        }
    }


    private void FixedUpdate()
    {
            Collider2D[] damageHit = Physics2D.OverlapCircleAll(damagePosition.position, damageRadius, whatIsDamageable);
            Collider2D[] groundHit = Physics2D.OverlapCircleAll(damagePosition.position, damageRadius, whatIsGround);

            foreach (Collider2D collider in damageHit)
            {
                collider.transform.parent.SendMessage("Damage", attackDetails);
                Destroy(gameObject);
            }

            foreach (Collider2D collider in groundHit)
            {
                Destroy(gameObject);
            }
    }
    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     // Cek apakah proyektil menyentuh objek dengan tag 'Enemy'
    //     if (other.CompareTag("Enemy"))
    //     {
    //         // Dapatkan komponen dari skrip EnemyHealth untuk memberikan damage
    //         EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            
    //         // Berikan damage ke musuh
    //         if (enemyHealth != null)
    //         {
    //             enemyHealth.TakeDamage(damage);
    //         }

    //         // Hancurkan proyektil setelah menyentuh musuh
    //         Destroy(gameObject);
    //     }
    // }

    public void SetProjectileParameters(float speed, float maxTravelDistance, float damage)
    {
        this.speed = speed;
        this.maxTravelDistance = maxTravelDistance;
        this.damage = damage;

        attackDetails.damageAmount = damage;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(damagePosition.position, damageRadius);
    }
}
