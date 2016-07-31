using UnityEngine;
using System.Collections;

public class SpaceDroneRotator : MonoBehaviour {

    public Transform childTransform;    
    public float rotationSpeed;    

    void Update() {
        childTransform.Rotate(Vector3.forward, Time.deltaTime * rotationSpeed);
    }
}