using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDrak : MonoBehaviour
{
    private Transform player;
	private GameObject Player;
    private Transform playerTransform;
    public float projectileSpeed = 5f;
    public float duration = 3f;  // Durasi dalam detik
    public int attackDamage = 20;
    public GameObject deathEffect;
    public bool isFlipped = false;

    private Animator anim;

    private Rigidbody2D rb;
    private float startTime;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private LayerMask whatIsPlayer;
    [SerializeField]
    private Transform damagePosition;
     [SerializeField]
    private float damageRadius;
     private AttackDetails attackDetails;

    void Start()
    {
        Player = GameObject.FindWithTag("Player");
    	
         if (Player != null) {
        // Dapatkan komponen Transform dari objek pemain dan simpan referensinya
        player = Player.GetComponent<PlayerControl>().transform;
        } else {
        Debug.LogError("Objek Player tidak ditemukan!");
        }

        rb = GetComponent<Rigidbody2D>();
        startTime = Time.time;
    }

    void Update()
    {
        LookAtPlayer();
        // Cek apakah durasi sudah habis
        if (Time.time - startTime < duration)
        {
            // Hitung arah ke pemain
            Vector2 direction = (player.position - transform.position).normalized;

            // Terapkan gaya ke rigidbody untuk menggerakkan proyektil
            rb.velocity = direction * projectileSpeed;
        }
        else
        {
            // Setel kecepatan menjadi nol setelah durasi berakhir
            rb.velocity = Vector2.zero;
            Instantiate(deathEffect, transform.position,transform.rotation);
            FinishAnim();
        }
    }

    private void FixedUpdate()
    {
            attackDetails.damageAmount = attackDamage;
            Collider2D damageHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsPlayer);
            Collider2D groundHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsGround);
            
            if (damageHit)
            {
                damageHit.transform.SendMessage("Damage", attackDetails);
                Instantiate(deathEffect, transform.position,transform.rotation);
                FinishAnim();
            }

            if (groundHit)
            {
                Instantiate(deathEffect, transform.position,transform.rotation);
                FinishAnim();
            }

    }

   public void LookAtPlayer()
	{
		Vector3 flipped = transform.localScale;
		flipped.z *= -1f;

		if (transform.position.x > player.position.x && isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = false;
		}
		else if (transform.position.x < player.position.x && !isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = true;
		}
	}

    private void FinishAnim()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(damagePosition.position, damageRadius);
    }
}
