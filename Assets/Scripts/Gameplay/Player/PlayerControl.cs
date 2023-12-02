using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private float movementInputDirection;
    private float jumpTimer;
    private float turnTimer;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100f;
    private float knockbackStartTime;
    [SerializeField]
    private float knockbackDuration;

    
    private int amountOfJumpsLeft;
    private int facingDirection = 1;

    private bool isFacingRight = true;
    private bool isWalking;
    private bool isGrounded;
    // private bool isTouchingWall;
    // private bool isWallSliding;
    private bool canNormalJump;
    // private bool canWallJump;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    private bool canMove;
    private bool canFlip;
    // private bool hasWallJumped;
    // private bool isTouchingLedge;
    // private bool canClimbLedge = false;
    // private bool ledgeDetected;
    private bool isDashing;
    private bool isCrouch;
    private bool isCrouching = false;
    private bool hasObstacleAbove = false;
    private bool knockback;
    private bool crouchPressed;


    [SerializeField]
    private Vector2 knockbackSpeed;


    private Rigidbody2D rb;
    private Animator anim;

    public int amountOfJumps = 1;


    public float movementSpeed = 10.0f;
    public float crouchSpeed = 0.36f;
    public float jumpForce = 16.0f;
    public float groundCheckRadius;
    public float overheadCheckRadius;
    // public float wallCheckDistance;
    // public float wallSlideSpeed;
    // public float movementForceInAir;
    // public float airDragMultiplier = 0.95f;
    // public float variableJumpHeightMultiplier = 0.5f;
    // public float wallHopForce;
    // public float wallJumpForce;
    public float jumpTimerSet = 0.15f;
    // public float turnTimerSet = 0.1f;
    // public float wallJumpTimerSet = 0.5f;
    public float dashTime;
    public float dashSpeed;
    public float distanceBetweenImages;
    public float dashCoolDown;

     public Transform groundCheck;
     public Transform overheadCheck;

    public LayerMask whatIsGround;
    public Collider2D standingCollider,crouchingCollider;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        // CheckIfWallSliding();
        CheckJump();
        // CheckLedgeClimb();
        CheckDash();
        CheckKnockback();
        
    }
    
    private void FixedUpdate() 
    {
        ApplyMovement();
        CheckSurroundings();
    }

     public bool GetDashStatus()
    {
        return isDashing;
    }

    public void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }

    private void CheckKnockback()
    {
        if(Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }

     private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isCrouch = Physics2D.OverlapCircle(overheadCheck.position,overheadCheckRadius,whatIsGround);

        if (isCrouch)
        {
            hasObstacleAbove = true; // Jika ada obstacle, atur hasObstacleAbove ke true
        }
        else
        {
            hasObstacleAbove = false;
        }

    }

     private void CheckIfCanJump()
    {
        if(isGrounded && rb.velocity.y <= 0)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if(amountOfJumpsLeft <= 0)
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }
      
    }

    private void CheckMovementDirection()
    {
        if(isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if(!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        if(Mathf.Abs(rb.velocity.x) >= 0.01f)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isCrouching", isCrouching);
        // anim.SetBool("isWallSliding", isWallSliding);
    }

     private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && !isCrouching)
        {
            if(isGrounded || (amountOfJumpsLeft > 0))
            {
                NormalJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }

        if (turnTimer >= 0)
        {
            turnTimer -= Time.deltaTime;

            if(turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }

        // if (checkJumpMultiplier && !Input.GetButton("Jump"))
        // {
        //     checkJumpMultiplier = false;
        //     rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        // }

        if (Input.GetButtonDown("Dash") && isGrounded && !isCrouching)
        {
            // Tambahkan kondisi untuk memastikan pemain tidak dapat dash saat melompat
            if (Time.time >= (lastDash + dashCoolDown))
            {
                AttemptToDash();
            }
        }

        //If we press Crouch button enable crouch 
        if (Input.GetButtonDown("Crouch")){
            CrouchDown();
            
        }
           
        //Otherwise disable it
        else if (Input.GetButtonUp("Crouch")){
            // Tambahkan kondisi untuk memastikan pemain tidak dapat dash saat crouching
            if (!hasObstacleAbove && !isAttemptingToJump)
            {
                CrouchUp();
            }
            
        }

    }

    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }

    public int GetFacingDirection()
    {
        return facingDirection;
    }

    private void CheckDash()
    {
        if (isDashing)
        {
            if(dashTimeLeft > 0)
            {
                canMove = false;
                canFlip = false;
                rb.velocity = new Vector2(dashSpeed * facingDirection, 0.0f);
                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }

            if(dashTimeLeft <= 0 )
            {
                isDashing = false;
                canMove = true;
                canFlip = true;
                isAttemptingToJump = false; // Tambahkan baris ini
            }
            
        }
    }

private void CheckJump()
    {
        if(isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }
    }

    private void NormalJump()
    {
        if (canNormalJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }

     private void ApplyMovement()
    {

        if(canMove && !knockback)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }
    
    }

    public void DisableFlip()
    {
        canFlip = false;
    }

    public void EnableFlip()
    {
        canFlip = true;
    }

    private void Flip()
    {
        if (canFlip && !knockback)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    private void CrouchDown()
    {
        if (!isCrouching) // Cek apakah karakter belum dalam kondisi crouching
    {
        isCrouching = true; // Atur karakter dalam kondisi crouching
        crouchingCollider.enabled = true;
        standingCollider.enabled = false;
        movementSpeed *= crouchSpeed;
    }
        
        
    }

    private void CrouchUp(){

       if (isCrouching)
    {
            isCrouching = false; // Atur karakter kembali ke posisi berdiri
            crouchingCollider.enabled = false;
            standingCollider.enabled = true;
            movementSpeed /= crouchSpeed;
    }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireSphere(overheadCheck.position, overheadCheckRadius);

        // Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}
