using UnityEngine;
using System.Collections;

public class MissileMover : MonoBehaviour {

    public GameObject boltExplosion;

    public int damage;
    public float speed;
    public float rotationSpeed;
    public float directionInZAxis; // direction should be either 1 or -1 (forward or backwards in z-axis)

    private Transform target;
    private Vector3 lookDirection;
    private Quaternion _lookRotation;

    void Start() {
        if (GameObject.FindWithTag("Player") != null) {
            target = GameObject.FindWithTag("Player").transform;
        }        
        GetComponent<Rigidbody>().velocity = transform.forward * directionInZAxis * speed;       
    }

    void LateUpdate() {
        if (target != null) {
            /* Find the vector pointing from our position to the target.
             * Formula: to get the direction of a vector from another point or (direction of v1 from v2).
             * target.position - transform.position or (v2 - v1)
             * 
             * Since our gameObject is facing the -z-axis then the formula should be
             * transform.position - target.position or (v1 - v2) */
            lookDirection = (transform.position - target.position).normalized;

            // Create the rotation we need to face the target.
            _lookRotation = Quaternion.LookRotation(lookDirection);

            // Rotate over time according to speed until the required rotation is reached.
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
        }

        // Update the Rigidbody.velocity to make it move towards where it is facing.
        GetComponent<Rigidbody>().velocity = transform.forward * directionInZAxis * speed;
    }
    
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("PlayerBolt")) {
            Instantiate(boltExplosion, transform.position, transform.rotation); // Instantiate a bolt Explosion for effects.
            Destroy(gameObject); // The missile
            RePool(other.gameObject); // The PlayerBolt
        }
    }

    void RePool(GameObject thisGameObject) {
        thisGameObject.SetActive(false);
    }
}
