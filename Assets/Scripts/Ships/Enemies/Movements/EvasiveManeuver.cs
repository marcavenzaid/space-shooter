using UnityEngine;
using System.Collections;

public class EvasiveManeuver : MonoBehaviour {

    [SerializeField] private Vector2 dodgeStrength = new Vector2(1, 7);
    [SerializeField] private Vector2 firstManeuverDelay = new Vector2(0.5f, 1);
    [SerializeField] private Vector2 maneuverDuration = new Vector2(1, 2);
    [SerializeField] private Vector2 maneuverWait = new Vector2(1, 4);
    private float velocityZ;
    private float targetManeuver;
    private Rigidbody Rb;
    private Ship ship;
    private float tiltStrength;
    private float horizontalSpeed;

	private void Awake () {
        Rb = GetComponent<Rigidbody>();
        ship = GetComponent<Ship>();
        tiltStrength = ship.TiltStrength;
        horizontalSpeed = ship.HorizontalSpeed;
    }

    private void OnEnable() {
        StartCoroutine(UpdateTargetManeuver());
    }

    private void Start() {
        velocityZ = Rb.velocity.z;
    }

    private void FixedUpdate() {
        Maneuver();
    }

    private void Maneuver() {
        float newManeuver = Mathf.MoveTowards(Rb.velocity.x, targetManeuver, Time.deltaTime * horizontalSpeed);

        Rb.velocity = new Vector3(newManeuver, 0, velocityZ);
        Rb.rotation = Quaternion.Euler(0, 0, Rb.velocity.x * -tiltStrength);
    }

    private IEnumerator UpdateTargetManeuver() {
        yield return new WaitForSeconds(Random.Range(firstManeuverDelay.x, firstManeuverDelay.y));
        while (true) {
            targetManeuver = Random.Range(dodgeStrength.x, dodgeStrength.y) * -Mathf.Sign(transform.position.x);
            yield return new WaitForSeconds(Random.Range(maneuverDuration.x, maneuverDuration.y));
            targetManeuver = 0;
            yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));
        }
    }
}
