using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary {
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {

    public int health;

    [HideInInspector]
    public int maxHealth;

    public float speed;
    public float tilt;
    public Boundary boundary;
    public float fireRate;
    public float delay;

    public GameObject shot;
    public Transform shotSpawn;

    void Start() {
        InvokeRepeating("Fire", delay, fireRate);
        maxHealth = health;
    }

    // Used when the boss appeares.
    public void InvokeFire() {
        InvokeRepeating("Fire", delay, fireRate);
    }

    // Used when the boss appeares.
    public void CancelInvokeFire() {
        CancelInvoke("Fire");
    }

    void Fire() {
        GameObject obj = GetComponent<ObjectPoolerScript>().GetPooledObject();

        if (obj == null)
            return;

        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.SetActive(true);

        GetComponent<AudioSource>().Play();
    }

    // This FixedUpdate() uses physics to control the Player maneuver/movements.
    void FixedUpdate() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        GetComponent<Rigidbody>().velocity = movement * speed;

        // Clamps the Player inside the game area.
        GetComponent<Rigidbody>().position = new Vector3(
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
        );

        // For the Player ships tilt
        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
    }
}
