using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBOSS : MonoBehaviour
{
     public int attackDamage = 20;
    public LayerMask attackMask;
    public GameObject bulletTrans;
    public GameObject bullet;

    // public GameObject pilarTrans;
    public GameObject bulletTrans2;
    public GameObject Player;
    public GameObject bulletTrans3;
    // public GameObject awakeTrans3;
    // public GameObject awakeTrans4;
    public GameObject pilar;
    // public GameObject SK;
    public Vector3 attackOffset;
    public Vector3 attackOffset2;
    AttackDetails attackDetails;    
     public void  Start()
     {
        Player.GetComponent<Transform>();
     } 
        
    public void attack () {
        Instantiate(bullet, bulletTrans.transform.position,bulletTrans.transform.rotation);
        Instantiate(bullet, bulletTrans2.transform.position,bulletTrans2.transform.rotation);
        Instantiate(bullet, bulletTrans3.transform.position,bulletTrans3.transform.rotation);
    }

    public void attack2(){
        Instantiate(pilar, Player.transform.position,Player.transform.rotation);
    }

    // public void inattack2(){
    //     Instantiate(pilar, pilarTrans.transform.position,pilarTrans.transform.rotation);
    //     Instantiate(pilar, pilarTrans2.transform.position,pilarTrans2.transform.rotation);
    // }

    // public void inattack(){
    //     Instantiate(SK, awakeTrans.transform.position,awakeTrans.transform.rotation);
    //     Instantiate(SK, awakeTrans2.transform.position,awakeTrans2.transform.rotation);
    //     Instantiate(SK, awakeTrans3.transform.position,awakeTrans3.transform.rotation);
    //     Instantiate(SK, awakeTrans4.transform.position,awakeTrans4.transform.rotation);
    // }

    void OnDrawGizmosSelected()
	{	
        bulletTrans.GetComponent<Transform>();
        Vector3 pos = bulletTrans.transform.position;
		pos += transform.right * attackOffset.x;
		pos += transform.up * attackOffset.y;
		Gizmos.DrawWireSphere(pos, 1);

        // pilarTrans.GetComponent<Transform>();
        // Vector3 poss = bulletTrans.transform.position;
		// poss += transform.right * attackOffset2.x;
		// poss += transform.up * attackOffset2.y;
		// Gizmos.DrawWireSphere(poss, 1);


	}
}
