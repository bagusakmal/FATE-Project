using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private float jum = 5;
    [SerializeField] private int totalJump;
    [Range(0, 1)] [SerializeField] private float CrouchSpeed = .36f;
    [SerializeField] private Collider2D m_CrouchDisableCollider;
    [SerializeField] private Collider2D m_CrouchEnableCollider;
    [SerializeField] private TrailRenderer tr;
    [Header("Events")]
	[Space]
    public UnityEvent OnLandEvent;

    [System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }
    public BoolEvent OnCrouchEvent;

    private Rigidbody2D body;
    private Transform playerTransform; // Added to store the Transform component
    private Animator anim;
    private bool isGround;
    private int airCount;
    private bool crouch = false;
    public HealthBar healthBar;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;



    private void  Awake() {
        body = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        anim = GetComponent<Animator>();
    }

    private void  Update() {
    
       Move();
       Jump();
       Crouch();
       Dashse();
       if (isDashing)
        {
            return;
        }

       
    }
    private void Move (){
        float horizontalinput = Input.GetAxis("Horizontal");
        Vector2 direc = new Vector2(horizontalinput,0);
        transform.Translate(direc * Time.deltaTime * speed);

        // flip right or left
        if(horizontalinput > 0.01f)
            playerTransform.localScale = Vector3.one;
        else if(horizontalinput < -0.01f)
            playerTransform.localScale = new Vector3(-1,1,1);
        
        //set anim
        anim.SetBool("Run", horizontalinput != 0);
        anim.SetBool("ground", isGround);
    }
    

    private void Jump () {
        // singel jump
        /*
         if(Input.GetKey(KeyCode.Space) && isGround){
            body.velocity = new Vector2(body.velocity.x, jum );
            anim.SetTrigger("jump");
            isGround = false;
         }
         */

         
         // double jump
         if(Input.GetKeyDown(KeyCode.Space) && airCount < totalJump){
            Vector2 direction = new Vector2(0,1 );
            body.velocity = direction * jum;
            anim.SetTrigger("jump");
            airCount += 1;
         }
         
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "OneWayPlatform")
        {
            airCount = 0;
            isGround = true;
            Debug.Log("isGrounded");
        }
            
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground" || collision.gameObject.tag == "OneWayPlatform")
        {
            isGround = false;
            Debug.Log("isNotGrounded");
        }
    }

    private void Crouch () {
        
        if(Input.GetKeyDown(KeyCode.C)){

                if (!crouch)
				{
					crouch = true;
                    // OnCrouchEvent.Invoke(true);
				}
                speed *= CrouchSpeed;
                

                if (m_CrouchDisableCollider != null){
					m_CrouchDisableCollider.enabled = false;
                    m_CrouchEnableCollider.enabled = true;
                    totalJump = 0;
                }
        }
        else if(Input.GetKeyUp(KeyCode.C)){
            // Enable the collider when not crouching
				if (m_CrouchDisableCollider != null){
					m_CrouchDisableCollider.enabled = true;
                    m_CrouchEnableCollider.enabled = false;
                    totalJump = 2;
                }
            speed /= CrouchSpeed;
            

            
				if (crouch)
				{
					crouch = false;
					// OnCrouchEvent.Invoke(false);
				}
        }
    }

    private void Dashse(){
        if (isDashing)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

          if (isDashing)
        {
            return;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = body.gravityScale;
        body.gravityScale = 0f;
        body.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        body.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
     private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Spike")
        {
            // healthBar.Damage(0.002f);
        }
    }

}

