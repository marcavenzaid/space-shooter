using UnityEngine;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour {

    public Transform shotSpawns;
    private Transform[] shotSpawnArr;
    public float fireRate;
    public float firstAttackDelay;

    public bool reloadable;
    public int ammunation;
    public float reloadTime;

    private AudioSource audioSource;
    private float ammunationsFired;

    private void Awake() {
        if(shotSpawns.childCount > 0) {
            shotSpawnArr = new Transform[shotSpawns.childCount];
            for (int i = 0; i < shotSpawns.childCount; i++) {
                shotSpawnArr[i] = shotSpawns.GetChild(i);
            }
        } else {
            shotSpawnArr = new Transform[] { shotSpawns };
        }
        
    }

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable() {
        InvokeRepeating("Fire", firstAttackDelay, fireRate);
    }

    // Cancel firing or reloading when disabled by RePool().
    void OnDisable() {
        StopFiring();
        ammunationsFired = 0;
    }

    void Fire() {
        for (int i = 0; i < shotSpawnArr.Length; i++) {
            GameObject obj = GetComponent<ObjectPoolerScript>().GetPooledObject();

            if (obj == null) {
                return;
            }

            shotSpawnArr[i].rotation = Quaternion.identity;
            obj.transform.position = new Vector3(shotSpawnArr[i].position.x, 0.0f, shotSpawnArr[i].position.z);
            obj.transform.rotation = shotSpawnArr[i].rotation;
            obj.SetActive(true);

            //---

            if (reloadable) {
                ammunationsFired++;
                if (ammunationsFired >= ammunation) {
                    CancelInvoke("Fire");
                    Invoke("Reload", reloadTime);
                }
            }
        }
        audioSource.Play();
    }    

    void Reload() {
        ammunationsFired = 0;
        InvokeRepeating("Fire", 0, fireRate);
    }

    public void StopFiring() {
        CancelInvoke();
    }
}
