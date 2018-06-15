using UnityEngine;
using System.Collections;

public class ComplexEvasiveManeuver2 : MonoBehaviour {

    public float forwardSpeed;
    public float positionToStopZ;
    public float dodgeX, dodgeZ;
    public float smoothing;
    public float tilt;
    public Vector2 startWait;
    public Vector2 maneuverTime;
    public Vector2 maneuverWait;
    public Boundary boundary;
    public float zKeepLine;

    private float targetManeuverX, targetManeuverZ;
    private Rigidbody rb;
    private bool preparationComplete;

    // ComplexEvasiveManeuver2 (script) is target independent unlike ComplexEvasiveManeuver (script).

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * forwardSpeed;
        StartCoroutine(Evade());
        preparationComplete = false;
    }

    void Update() {
        if (!preparationComplete & transform.position.z <= positionToStopZ) {
            rb.velocity = Vector3.zero;
            preparationComplete = true;            
        }
    }

    void FixedUpdate() {
        if (!preparationComplete) {
            return;
        }

        float newManeuverX = Mathf.MoveTowards(rb.velocity.x, targetManeuverX, Time.deltaTime * smoothing);
        float newManeuverZ = Mathf.MoveTowards(rb.velocity.z, targetManeuverZ, Time.deltaTime * smoothing);

        rb.velocity = new Vector3(newManeuverX, 0.0f, newManeuverZ);
        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, boundary.XMin, boundary.XMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.ZMin, boundary.ZMax)
        );

        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }

    IEnumerator Evade() {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));

        while (true) {
            targetManeuverX = Random.Range(1, dodgeX) * -Mathf.Sign(transform.position.x);
            targetManeuverZ = Random.Range(1, dodgeZ) * -Mathf.Sign(transform.position.z - zKeepLine);
            yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
            targetManeuverX = 0;
            targetManeuverZ = 0;
            yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));
        }
    }
}
