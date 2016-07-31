using UnityEngine;
using System.Collections;

public class BoltMover : MonoBehaviour {

    public GameObject boltExplosion;
    public int damage;
    public float speed;
    public float directionInZAxis; // direction should be either 1 or -1 (forward or backwards in z-axis)    

    void FixedUpdate() {
        if (enabled) {
            GetComponent<Rigidbody>().velocity = transform.forward * directionInZAxis * speed;
        }
    }
}
