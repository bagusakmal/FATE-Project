using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{

    public Camera cam; 
    public Transform subject;
    Vector2 startPosition;
    Vector2 travel => (Vector2)cam.transform.position - startPosition ;
  
    float startZ;
    float distanceFromSubject => transform.position.z - subject.position.z;
    float clippingPlane => (cam.transform.position.z + (distanceFromSubject > 0 ? cam.farClipPlane : cam.nearClipPlane));


    float parallaxEffect => Mathf.Abs(distanceFromSubject) / clippingPlane ;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos = startPosition + travel * parallaxEffect;
        Vector2 newYpos = startPosition + travel;
        transform.position = new Vector3(newPos.x, newYpos.y, startZ);
    }
}
