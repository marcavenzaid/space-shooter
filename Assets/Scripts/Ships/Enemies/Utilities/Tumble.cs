using UnityEngine;

public class Tumble : MonoBehaviour {
    
    public float[] rangeAngularVelocityX = new float[2];
    public float[] rangeAngularVelocityY = new float[2];
    public float[] rangeAngularVelocityZ = new float[2];
    private float speed;

    private void Awake() {
        speed = GetComponent<Ship>().VerticalSpeed;
    }

    public void OnEnable () {
        GetComponent<Rigidbody>().velocity = -transform.forward * speed;

        GetComponent<Rigidbody>().angularVelocity = new Vector3(
            Random.Range(rangeAngularVelocityX[0], rangeAngularVelocityX[1]), 
            Random.Range(rangeAngularVelocityY[0], rangeAngularVelocityY[1]), 
            Random.Range(rangeAngularVelocityZ[0], rangeAngularVelocityZ[1])
        );
    }
}
