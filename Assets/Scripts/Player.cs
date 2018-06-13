using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameController))]
public class Player : Ship {
   
    [SerializeField] private float fireRateIncreaseRate = 0.05f;
    private GameController gameController;
    private HealthBar healthBar;
    private List<Transform> shotSpawnList = new List<Transform>();
    private Transform[] currentShotSpawns; // Changed by powerups, using PowerUpsManager.cs
    private int weaponLevel;        
    private float defaultFireRate;
    private float lowestFireRate = 0.1f;

    protected override void Awake () {        
        base.Awake();

        foreach (Transform t in ShotSpawns) {
            shotSpawnList.Add(t);
        }
        SetWeaponLevel(1);
        defaultFireRate = FireRate;
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        healthBar = GetComponent<HealthBar>();
    }

    public override void TakeDamage(int damage) {
        base.TakeDamage(damage);    
        if (!IsAlive()) {
            Health = 0;
            Death();
        }
        healthBar.SetHealth(Health);
    }

    protected override void Death() {
        base.Death();
        Destroy(gameObject);
        gameController.GameOver();

        if (GetComponent<DisableOnDeath>() != null) {
            GetComponent<DisableOnDeath>().Disable();
        }
    }

    protected override void Fire() {
        for (int i = 0; i < currentShotSpawns.Length; i++) {
            GameObject obj = GetComponent<ObjectPoolerScript>().GetPooledObject();

            if (obj == null) {
                return;
            }
            ShotSpawns.rotation = Quaternion.identity;
            obj.transform.position = currentShotSpawns[i].position;
            obj.transform.rotation = currentShotSpawns[i].rotation;
            obj.SetActive(true);
        }
        WeaponAudioSource.Play();
    }

    public void InvokeFire(bool fire, float time) {
        if (fire) {
            InvokeRepeating("Fire", time, FireRate);
        } else {
            CancelInvoke("Fire");
        }
    }    

    private void ResetFire() {
        InvokeFire(false, FireRate);
        InvokeFire(true, FireRate);
    }

    public void UpgradeFireRate() {
        if (FireRate > lowestFireRate) {
            FireRate -= fireRateIncreaseRate;
            if(FireRate < lowestFireRate) {
                FireRate = lowestFireRate;
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
        FireRate = defaultFireRate; // Reset the fireRate to default rate.
        weaponLevel = 1;
        SetWeaponLevel(weaponLevel);
        ResetFire();
    }
}
