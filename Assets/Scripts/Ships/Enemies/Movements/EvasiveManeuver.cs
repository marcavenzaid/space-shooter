using UnityEngine;
using System.Collections;

public class EvasiveManeuver : MonoBehaviour {

    [SerializeField] private float maxDodgeStrength = 7;
    [SerializeField] private float dodgeSmoothing = 5;
    [SerializeField] private Vector2 firstManeuverDelay = new Vector2(0.5f, 1);
    [SerializeField] private Vector2 maneuverDuration = new Vector2(1, 2);
    [SerializeField] private Vector2 maneuverWait = new Vector2(1, 4);
    private float velocityZ;
    private float targetManeuver;
    private Rigidbody Rb;
    private Ship ship;

	private void Awake () {
        Rb = GetComponent<Rigidbody>();
        ship = GetComponent<Ship>();        
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
        float newManeuver = Mathf.MoveTowards(Rb.velocity.x, targetManeuver, Time.deltaTime * dodgeSmoothing);

        Rb.velocity = new Vector3(newManeuver, 0, velocityZ);
        Rb.rotation = Quaternion.Euler(0, 0, Rb.velocity.x * -ship.TiltStrength);
    }

    private IEnumerator UpdateTargetManeuver() {
        yield return new WaitForSeconds(Random.Range(firstManeuverDelay.x, firstManeuverDelay.y));
        while (true) {
            targetManeuver = Random.Range(1, maxDodgeStrength) * -Mathf.Sign(transform.position.x);
            yield return new WaitForSeconds(Random.Range(maneuverDuration.x, maneuverDuration.y));
            targetManeuver = 0;
            yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));
        }
    }
}
