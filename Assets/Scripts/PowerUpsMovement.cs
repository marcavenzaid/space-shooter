using UnityEngine;
using System.Collections;

public class PowerUpsMovement : MonoBehaviour {

    public float speed;
    public float directionInZAxis;
    public float nextMoveTimeMin, nextMoveTimeMax;
    public Boundary boundary;

    private Rigidbody rb;
    private float nextMoveTime;
    private int sign;

    void Start() {
        rb = GetComponent<Rigidbody>();
        sign = RandomSign();
        GetComponent<Rigidbody>().velocity = transform.forward * directionInZAxis * speed;
        nextMoveTime = Time.time + Random.Range(nextMoveTimeMin, nextMoveTimeMax);
    }

    void FixedUpdate() {
        if (Time.time >= nextMoveTime) {
            nextMoveTime = Time.time + Random.Range(nextMoveTimeMin, nextMoveTimeMax);
            if (sign == 1) {
                GetComponent<Rigidbody>().velocity = new Vector3(1, 0, 1) * directionInZAxis * speed; // Update velocity.
            } else {
                GetComponent<Rigidbody>().velocity = new Vector3(-1, 0, 1) * directionInZAxis * speed; // Update velocity.
            }
            sign = -sign;
        }

        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, boundary.XMin, boundary.XMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.ZMin, boundary.ZMax)
        );
    }

    private int RandomSign() {
        return (Random.value < .5) ? 1 : -1;
    }
}
