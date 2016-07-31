using UnityEngine;
using System.Collections;

public class FollowMovement : MonoBehaviour {

    public GameObject boltExplosion;
    public int damage;
    public float speed;
    public float boostSpeed;
    public float sensorDistance; 
    public float directionInZAxis;    

    private Transform target;
    private Vector3 lookDirection;
    private Quaternion _lookRotation;

    void Start () {
        if (GameObject.FindWithTag("Player") != null) {
            target = GameObject.FindWithTag("Player").transform;
        } else {
            target = null;
        }
    }

    void OnEnable() {
        GetComponent<Rigidbody>().velocity = transform.forward * directionInZAxis * speed;
    }

	void LateUpdate () {
        if (target == null) {
            return;
        }

        if (Vector3.Distance(gameObject.transform.position, target.position) <= sensorDistance) {
            MoveTowardsTarget();
        }
    }

    void MoveTowardsTarget() {
        if (target != null) {
            /* Find the vector pointing from our position to the target.
             * Formula: to get the direction of a vector from another point or (direction of v1 from v2).
             * target.position - transform.position or (v2 - v1)
             * 
             * Since our gameObject is facing the -z-axis then the formula should be
             * transform.position - target.position or (v1 - v2) */
            lookDirection = (transform.position - target.position).normalized;

            // The rotation we need face the target.
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }

        // Update the Rigidbody.velocity to make it move towards where it is facing.
        GetComponent<Rigidbody>().velocity = transform.forward * directionInZAxis * boostSpeed;
    }
}
