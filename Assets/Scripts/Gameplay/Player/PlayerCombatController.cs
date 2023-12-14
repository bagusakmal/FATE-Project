using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField]
    public bool combatEnabled;
    [SerializeField]
    private float inputTimer, attack1Radius, attack1Damage;
    [SerializeField]
    private float stunDamageAmount = 1f;
    [SerializeField]
    private Transform attack1HitBoxPos;
    [SerializeField]
    private LayerMask whatIsDamageable;
    private bool gotInput, isFirstAttack;
    public bool isAttacking;
    private float lastInputTime = Mathf.NegativeInfinity;
    private float lastAttackTime = Mathf.NegativeInfinity;
    private AttackDetails attackDetails;
    private Animator anim;
    private PlayerControl PC;
    private PlayerStats PS;
    private int numberOfAttacks; // Track the number of attacks
    private bool isCooldown; // Track whether cooldown is active
    [SerializeField]
    private float attackCooldown; // Cooldown time after two attacks

    [SerializeField]
    private GameObject projectilePrefab1; // Projectile for Skill 1
    [SerializeField]
    private GameObject projectilePrefab2; // Projectile for Skill 2
    [SerializeField]
    private Transform projectileSpawnPoint;
    [SerializeField]
    private float projectileForce = 10f;
    [SerializeField]
    private float cooldownBetweenSkills = 1f;
    [SerializeField]
    private float skill2Cooldown; // Cooldown time for Skill 2

    public float projectileSpeed = 10f;
    public float projectileMaxTravelDistance;

    private bool canUseSkill1 = true;
    private bool canUseSkill2 = true;
    private bool isUsingSkill = false;

    private const string Attack1DamageKey = "Attack1Damage";

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnabled);
        PC = GetComponent<PlayerControl>();
        PS = GetComponent<PlayerStats>();
        LoadAttack1Damage();
    }

    private void Update()
    {
        CheckCombatInput();
        CheckAttacks();
        CheckSkills();
    }

    public void IncreaseAttack1Damage(float amount)
    {
        attack1Damage += amount;
        SaveAttack1Damage();
    }

    private void SaveAttack1Damage()
    {
        // PlayerPrefs.SetFloat(Attack1DamageKey, attack1Damage);
        // PlayerPrefs.Save();
    }

    private void LoadAttack1Damage()
    {
        if (PlayerPrefs.HasKey(Attack1DamageKey))
        {
            attack1Damage = PlayerPrefs.GetFloat(Attack1DamageKey);
        }
        else
        {
            attack1Damage = 10f; // Default value
            SaveAttack1Damage();
        }
    }

    public float GetAttack1Damage()
    {
        return attack1Damage;
    }

    private void CheckCombatInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (combatEnabled && !isCooldown) // Check if cooldown is not active
            {
                // Attempt combat
                gotInput = true;
                lastInputTime = Time.time;
            }
        }
    }

    private void CheckAttacks()
    {
        if (!isUsingSkill)
        {
            if (gotInput)
            {
                // Perform Attack1
                if (!isAttacking)
                {
                    gotInput = false;
                    isAttacking = true;
                    canUseSkill1 = false;
                    isFirstAttack = !isFirstAttack;
                    anim.SetBool("attack1", true);
                    anim.SetBool("firstAttack", isFirstAttack);
                    anim.SetBool("isAttacking", isAttacking);
                    lastAttackTime = Time.time;
                    numberOfAttacks++;

                    if (numberOfAttacks >= 2)
                    {
                        StartCoroutine(StartAttackCooldown());
                    }
                }
            }
        }

        if (Time.time >= lastInputTime + inputTimer)
        {
            // Wait for new input
            gotInput = false;
        }

        if (Time.time >= lastAttackTime + attackCooldown && isAttacking)
        {
            FinishAttack1();
        }
    }

    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius, whatIsDamageable);

        attackDetails.damageAmount = attack1Damage;
        attackDetails.position = transform.position;
        attackDetails.stunDamageAmount = stunDamageAmount;

        foreach (Collider2D collider in detectedObjects)
        {
            collider.transform.parent.SendMessage("Damage", attackDetails);
            // Instantiate hit particle
        }
    }

    private void FinishAttack1()
    {
        isAttacking = false;
        canUseSkill1 = true;
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);
    }

    private void Damage(AttackDetails attackDetails)
    {
        if (!PC.GetDashStatus())
        {
            int direction;

            PS.DecreaseHealth(attackDetails.damageAmount);

            if (attackDetails.position.x < transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            PC.Knockback(direction);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }

    private IEnumerator StartAttackCooldown()
    {
        isCooldown = true; // Set cooldown to true
        yield return new WaitForSeconds(attackCooldown);
        isCooldown = false; // Reset cooldown
        numberOfAttacks = 0; // Reset the number of attacks
    }

    private void CheckSkills()
    {
        if(PS.currentMana >= projectileForce && !isAttacking){
            if(!PC.isWalking && PC.isGrounded){
            if (Input.GetKeyDown(KeyCode.Q) && canUseSkill1 && !isUsingSkill)
            {
                PC.isUsingSkill = true;
                isUsingSkill = true;
                anim.SetTrigger("UseSkill");
                StartCoroutine(StartSkillCooldown(skill2Cooldown));
            }
            if (Input.GetKeyDown(KeyCode.E) && canUseSkill2 && !isUsingSkill)
            {
                PC.isUsingSkill = true;
                isUsingSkill=true;
                anim.SetTrigger("UseSkill2");
                StartCoroutine(StartSkillCooldown(skill2Cooldown));
            }
            }
        }
        
    }

    private void triggerAttack(){
        SpawnProjectile1();
        PS.DecreaseMana(projectileForce);
        // PC.isUsingSkill = false;
    }

    private void triggerAttack2(){
        SpawnProjectile2();
        PS.DecreaseMana(projectileForce);
        // PC.isUsingSkill = false;
    }

    private void enablemove () {
        PC.isUsingSkill = false;
        
    }

    private void SpawnProjectile1()
{
    Debug.Log("Spawning Projectile1");
    projectileMaxTravelDistance = 1000;
    // Spawn projectile for Skill 1
    GameObject projectile = Instantiate(projectilePrefab1, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
    attackDetails.damageAmount = attack1Damage;

    // Add force to the projectile
    PrijectilePlayer projectileRb = projectile.GetComponent<PrijectilePlayer>();
    
    if (projectileRb != null)
    {
        Debug.Log("Projectile Active");
        projectileRb.SetProjectileParameters(projectileSpeed, projectileMaxTravelDistance, attack1Damage);
    }
    else
    {
        Debug.LogError("PrijectilePlayer script not found on the projectile");
    }
}


    public void SpawnProjectile2()
    {
        Debug.Log("Spawning Projectile1");
        projectileMaxTravelDistance = 1000;
        // Spawn projectile for Skill 2
        GameObject projectile = Instantiate(projectilePrefab2, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        attackDetails.damageAmount = attack1Damage;

        // Add force to the projectile
        PrijectilePlayer projectileRb = projectile.GetComponent<PrijectilePlayer>();
    
        if (projectileRb != null)
        {
            Debug.Log("Projectile Active");
            projectileRb.SetProjectileParameters(projectileSpeed, projectileMaxTravelDistance, attack1Damage);
        }
        else
        {
            Debug.LogError("PrijectilePlayer script not found on the projectile");
        }
    }

    private IEnumerator StartSkillCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        PC.isUsingSkill = false;
        isUsingSkill = false;
    }
}
