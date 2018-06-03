using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary {
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {

    [SerializeField] private int health;
    private int maxHealth;
    [SerializeField] private float speed;
    [SerializeField] private float tilt;
    [SerializeField] private Boundary boundary;
    [SerializeField] private float fireRate = 0.3f;
    [SerializeField] private float delay;
    [SerializeField] private Transform[] shotSpawn;
    [SerializeField] private Transform[] shotSpawns1;
    [SerializeField] private Transform[] shotSpawns2;
    [SerializeField] private Transform[] shotSpawns3;
    [SerializeField] private Transform[] currentShotSpawns; // Changed by powerups, using PowerUpsManager.cs
    [SerializeField] private GameObject shot;

    private Rigidbody rb;
    private int fireRateUpgrades;
    private float origFireRate;

    void Start() {
        rb = GetComponent<Rigidbody>();
        origFireRate = fireRate;
        InvokeRepeating("Fire", delay, fireRate);        
        maxHealth = health;
        currentShotSpawns = shotSpawn;
        fireRateUpgrades = 0;
    }

    public int GetHealth() {
        return health;
    }    

    public int GetMaxHealth() {
        return maxHealth;
    }

    public GameObject GetShotGameObject() {
        return shot;
    }

    public void SubtractHealth(int value) {
        health -= value;
    }

    public void InvokeFire(bool fire) {
        if (fire) {
            InvokeRepeating("Fire", fireRate, fireRate);
        } else {
            CancelInvoke("Fire");
        }
    }

    public void ResetFire() {
        InvokeFire(false);
        InvokeFire(true);
    }

    public void UpgradeFireRate() {
        if (fireRateUpgrades < 3) {
            fireRateUpgrades++;
            fireRate -= 0.05f;
            ResetFire();
        }                
    }

    public void UpgradeWeapon() {
        if (currentShotSpawns == shotSpawn) {
            currentShotSpawns = shotSpawns1;
        } else if (currentShotSpawns == shotSpawns1) {
            currentShotSpawns = shotSpawns2;
        } else if (currentShotSpawns == shotSpawns2) {
            currentShotSpawns = shotSpawns3;
        }
    }

    public void ResetUpgrades() {
        fireRate = origFireRate;                // Reset the fireRate to default rate.
        currentShotSpawns = shotSpawn;  // Reset the width of the shots.
        ResetFire();
    }

    void Fire() {
        for (int i = 0; i < currentShotSpawns.Length; i++) {
            GameObject obj = GetComponent<ObjectPoolerScript>().GetPooledObject();

            if (obj == null) {
                return;
            }                
            obj.transform.position = currentShotSpawns[i].position;
            obj.transform.rotation = currentShotSpawns[i].rotation;
            obj.SetActive(true);
        }
        GetComponent<AudioSource>().Play();
    }

    void FixedUpdate() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement * speed;

        // Clamps the Player inside the game area.
        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );

        // Tilt ship on movement.
        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }
}
