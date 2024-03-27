using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Explodable : MonoBehaviour {
    [SerializeField] private float triggerForce = 0.5f;
    [SerializeField] private float explosionRadius = 5;
    [SerializeField] private float explosionForce = 500;

//    [SerializeField] public GameObject Target;
    void OnCollisionEnter(Collision collision) 
    {
        if (collision.relativeVelocity.magnitude >= triggerForce && collision.gameObject.CompareTag("Player")) 
        {
            var surroundingObjects = Physics.OverlapSphere(transform.position, explosionRadius);
 
            foreach (var obj in surroundingObjects) 
            {
                var rb = obj.GetComponent<Rigidbody>();
                if (rb == null) continue;
 
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius,1);
            }
 
        //    Instantiate(Target, transform.position, Quaternion.identity);
 
        //    Destroy(gameObject);
        }
    }
}