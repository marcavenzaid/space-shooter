using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour {

    private Player player;
    private Transform shotSpawns;
    private Rigidbody rb;
    private float boundaryXMin, boundaryXMax, boundaryZMin, boundaryZMax;
    private float speed;
    private float tiltStrength;

    private void Awake() {
        rb = GetComponent<Rigidbody>();        
    }

    private void Start() {
        player = GetComponent<Player>();
        boundaryXMin = player.Bounds.XMin;
        boundaryXMax = player.Bounds.XMax;
        boundaryZMin = player.Bounds.ZMin;
        boundaryZMax = player.Bounds.ZMax;
        speed = player.Speed;
        tiltStrength = player.TiltStrength;
        player.InvokeFire(true, player.FirstAttackDelay);
    }       

    private void FixedUpdate() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement * speed;

        // Clamps the Player inside the game area.
        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, boundaryXMin, boundaryXMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundaryZMin, boundaryZMax));

        // Tilt ship on movement.
        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tiltStrength);
    }
}
