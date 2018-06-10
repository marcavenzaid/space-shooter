using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private Boundary boundary = new Boundary(-8, 8, -7, 12);
    [SerializeField] private int health = 100;    
    [SerializeField] private float speed = 10;
    [SerializeField] private float tiltStrength = 4;    
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float fireRateIncreaseRate = 0.05f;
    [SerializeField] private GameObject shot;
    [SerializeField] private Transform shotSpawns;
    private List<Transform> shotSpawnsList = new List<Transform>();
    private Transform[] currentShotSpawns; // Changed by powerups, using PowerUpsManager.cs
    private int weaponLevel;    
    private int maxHealth;
    private float defaultFireRate;

    private void Awake () {
        foreach (Transform t in shotSpawns) {
            shotSpawnsList.Add(t);
        };        
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

    public void SubtractHealth(int value) {
        health -= value;
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

    public Transform[] GetCurrentShotSpawns() {
        return currentShotSpawns;
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
            obj.transform.position = currentShotSpawns[i].position;
            obj.transform.rotation = currentShotSpawns[i].rotation;
            obj.SetActive(true);
        }
        GetComponent<AudioSource>().Play();
    }

    public void ResetFire() {
        InvokeFire(false, fireRate);
        InvokeFire(true, fireRate);
    }

    public void UpgradeFireRate() {
        if (fireRate < 0.1f) {
            fireRate -= fireRateIncreaseRate;
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
                currentShotSpawns = shotSpawnsList.GetRange(0, 1).ToArray();
                break;
            case 2:
                currentShotSpawns = shotSpawnsList.GetRange(1, 2).ToArray();
                break;
            case 3:
                currentShotSpawns = shotSpawnsList.GetRange(0, 3).ToArray();
                break;
            case 4:
                currentShotSpawns = shotSpawnsList.GetRange(0, 5).ToArray();
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
