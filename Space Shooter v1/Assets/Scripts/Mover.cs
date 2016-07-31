using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

    public float speed;

    void Start() {        
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    // This method will be used for ex. you will execute TumbleWhenDead (script).
    public void StopMoving() {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
