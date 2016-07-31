using UnityEngine;
using System.Collections;

public class TumbleWhenDead : MonoBehaviour {

    public float speed;
    public float[] rangeAngularVelocityX = new float[2];
    public float[] rangeAngularVelocityY = new float[2];
    public float[] rangeAngularVelocityZ = new float[2];

    // This TumbleWhenDead (Script) should be disabled in the gameObject it is attached for the logic to work.

    public void OnEnable () {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;

        GetComponent<Rigidbody>().angularVelocity = new Vector3(
            Random.Range(rangeAngularVelocityX[0], rangeAngularVelocityX[1]), 
            Random.Range(rangeAngularVelocityY[0], rangeAngularVelocityY[1]), 
            Random.Range(rangeAngularVelocityZ[0], rangeAngularVelocityZ[1])
        );
    }
}
