using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameController))]
public class Player : MonoBehaviour {
    
    [SerializeField] private Boundary boundary = new Boundary(-8, 8, -7, 12);
    [SerializeField] private GameObject explosion;
    [SerializeField] private int health = 100;    
    [SerializeField] private float speed = 10;
    [SerializeField] private float tiltStrength = 4;    
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float fireRateIncreaseRate = 0.05f;
    [SerializeField] private GameObject shot;
    [SerializeField] private Transform shotSpawns;
    private List<Transform> shotSpawnList = new List<Transform>();
    private Transform[] currentShotSpawns; // Changed by powerups, using PowerUpsManager.cs
    private int weaponLevel;    
    private int maxHealth;
    private float defaultFireRate;
    private float lowestFireRate = 0.1f;
    private GameController gameController;

    private void Awake () {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();

        foreach (Transform t in shotSpawns) {
            shotSpawnList.Add(t);
        }     
        maxHealth = health;
        SetWeaponLevel(1);
        defaultFireRate = fireRate;
    }

    public Boundary GetBoundary() {
        return boundary;
    }

    public int GetHealth() {
        return health;
    }

    public int GetMaxHealth() {
        return maxHealth;
    }

    public bool IsAlive() {
        return GetHealth() > 0;
    }

    public void TakeDamage(int damage) {
        health -= damage;
        if (!IsAlive()) {
            health = 0;
            Death();            
        }
    }

    private void Death() {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject); // Destroy player gameObject if health is less than zero.
        gameController.GameOver();

        if (GetComponent<DisableOnDeath>() != null) {
            GetComponent<DisableOnDeath>().Disable();
        }
    }

    public float GetSpeed() {
        return speed;
    }

    public float GetTiltStrength() {
        return tiltStrength;
    }

    public float GetFireRate() {
        return fireRate;
    }

    public GameObject GetShotGameObject() {
        return shot;
    }

    public Transform GetShotSpawn() {
        return shotSpawns;
    }    

    public void InvokeFire(bool fire, float time) {
        if (fire) {
            InvokeRepeating("Fire", time, fireRate);
        } else {
            CancelInvoke("Fire");
        }
    }

    private void Fire() {
        for (int i = 0; i < currentShotSpawns.Length; i++) {
            GameObject obj = GetComponent<ObjectPoolerScript>().GetPooledObject();

            if (obj == null) {
                return;
            }
            shotSpawns.rotation = Quaternion.identity;
            obj.transform.position = currentShotSpawns[i].position;
            obj.transform.rotation = currentShotSpawns[i].rotation;
            obj.SetActive(true);
        }
        GetComponent<AudioSource>().Play();
    }

    private void ResetFire() {
        InvokeFire(false, fireRate);
        InvokeFire(true, fireRate);
    }

    public void UpgradeFireRate() {
        if (fireRate > lowestFireRate) {
            fireRate -= fireRateIncreaseRate;
            if(fireRate < lowestFireRate) {
                fireRate = lowestFireRate;
            }
        }
        ResetFire();
    }

    public int GetWeaponlevel() {
        return weaponLevel;
    }

    public void SetWeaponLevel(int level) {
        weaponLevel = level;

        switch (level) {
            case 1:
                currentShotSpawns = shotSpawnList.GetRange(0, 1).ToArray();
                break;
            case 2:
                currentShotSpawns = shotSpawnList.GetRange(1, 2).ToArray();
                break;
            case 3:
                currentShotSpawns = shotSpawnList.GetRange(0, 3).ToArray();
                break;
            case 4:
                currentShotSpawns = shotSpawnList.GetRange(0, 5).ToArray();
                break;
        }
    }

    public void ResetUpgrades() {
        fireRate = defaultFireRate; // Reset the fireRate to default rate.
        weaponLevel = 1;
        SetWeaponLevel(weaponLevel);
        ResetFire();
    }
}
