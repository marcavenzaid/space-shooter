using UnityEngine;
using System.Collections;

public class VectorMovement : MonoBehaviour {

    public GameObject boltExplosion;
    public int damage;
    public float speed;
    public float directionInZAxis;

    public float nextMoveTimeMin, nextMoveTimeMax;
    public float degreeMin, degreeMax;    

    private float nextMoveTime;
    private float degree;
    private int sign;

    void Start() {
        sign = RandomSign();
        degree = Random.Range(degreeMin, degreeMax);
    }

    void OnEnable() {
        GetComponent<Rigidbody>().velocity = transform.forward * directionInZAxis * speed;
        nextMoveTime = Time.time + Random.Range(nextMoveTimeMin, nextMoveTimeMax);        
    }
	
	void Update () {
        if (Time.time >= nextMoveTime) {
            RotateByDegree();
            nextMoveTime = Time.time + Random.Range(nextMoveTimeMin, nextMoveTimeMax);
            degree = Random.Range(degreeMin, degreeMax);
            GetComponent<Rigidbody>().velocity = transform.forward * directionInZAxis * speed; // Update velocity.
        }
	}

    private void RotateByDegree() {
        transform.rotation = Quaternion.Euler(0.0f, transform.rotation.y + degree * sign, 0.0f);
        sign *= -1;
    }

    private int RandomSign() {
        return (Random.value < .5) ? 1 : -1;
    }
}
