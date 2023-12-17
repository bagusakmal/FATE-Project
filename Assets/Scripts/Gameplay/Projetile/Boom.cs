using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    public int attackDamage = 20;
    public float damageRadius = 1f;
    public Vector3 attackOffset;
    public LayerMask attackMask;
    AttackDetails attackDetails;

    private void Damage () {
        attackDetails.damageAmount = attackDamage;
        Vector3 pos = transform.position;
		pos += transform.right * attackOffset.x;
		pos += transform.up * attackOffset.y;
        Collider2D damageHit = Physics2D.OverlapCircle(pos, damageRadius, attackMask);
        if (damageHit != null)
		{
			// colInfo.GetComponent<PlayerStats>().DecreaseHealth(attackDamage);
            damageHit.transform.SendMessage("Damage", attackDetails);
		}
    }
    

    private void FinishAnim()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position;
		pos += transform.right * attackOffset.x;
		pos += transform.up * attackOffset.y;

        Gizmos.DrawWireSphere(pos, damageRadius);
    }
}
