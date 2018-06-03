using UnityEngine;
using System.Collections;

public class MoveToManeuver : MonoBehaviour {

    public float speed;
    public float waitTimeMin, waitTimeMax;    
    public float minRangeX, maxRangeX, minRangeZ, maxRangeZ;

    private float waitTime;
    private float randomRangeX, randomRangeZ;
    private bool inPosition;

    void Start() {        
        randomRangeX = Random.Range(minRangeX, maxRangeX);
        randomRangeZ = Random.Range(minRangeZ, maxRangeZ);
        inPosition = false;
    }

    void Update() {        
        if (!inPosition) {
            if (transform.position == new Vector3(randomRangeX, 0.0f, randomRangeZ)) {
                inPosition = true;
                waitTime = Time.time + Random.Range(waitTimeMin, waitTimeMax);
            } else {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(randomRangeX, 0.0f, randomRangeZ), speed * Time.deltaTime);
            }
        } else if (inPosition) {
            if (Time.time > waitTime) {
                randomRangeX = Random.Range(minRangeX, maxRangeX);
                randomRangeZ = Random.Range(minRangeZ, maxRangeZ);                
                inPosition = false;
            }
        }
    }
}
