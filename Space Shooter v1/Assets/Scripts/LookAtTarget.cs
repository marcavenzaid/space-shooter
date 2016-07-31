using UnityEngine;
using System.Collections;

public class LookAtTarget : MonoBehaviour {

    public float rotationSpeed;

    private Transform target;
    private Vector3 lookDirection;
    private Quaternion _lookRotation;

    void Start () {
        if (GameObject.FindWithTag("Player") != null) {
            target = GameObject.FindWithTag("Player").transform;
        }
    }
	
	void LateUpdate () {
        if (target != null) {
            /* Find the vector pointing from our position to the target.
             * Formula: to get the direction of a vector from another point or (direction of v1 from v2).
             * target.position - transform.position or (v2 - v1)
             * 
             * Since our gameObject is facing the -z-axis then the formula should be
             * transform.position - target.position or (v1 - v2) */
            lookDirection = (transform.position - target.position).normalized;

            // Create the rotation we need to be in to look at the target.
            _lookRotation = Quaternion.LookRotation(lookDirection);

            // Rotate over time according to speed until the required rotation is reached.
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
