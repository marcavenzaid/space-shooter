using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

    public Transform[] shotSpawn;
    public float fireRate;
    public float delay;

    public bool reloadable;
    public int ammunation;
    public float reloadTime;

    private AudioSource audioSource;
    private float ammunationsFired;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    // InvokeRepeating Fire() if the enemy ship is spawned in the game.
    void OnEnable() {
        InvokeRepeating("Fire", delay, fireRate);
    }

    // Cancel firing or reloading when disabled by RePool().
    void OnDisable() {
        CancelInvoke();
        ammunationsFired = 0;
    }

    void Fire() {
        for (int i = 0; i < shotSpawn.Length; i++) {
            GameObject obj = GetComponent<ObjectPoolerScript>().GetPooledObject();

            if (obj == null) return;

            obj.transform.position = new Vector3(shotSpawn[i].position.x, 0.0f, shotSpawn[i].position.z);
            obj.transform.rotation = Quaternion.Euler(0.0f, shotSpawn[i].eulerAngles.y, 0.0f);
            obj.SetActive(true);

            //---

            if (reloadable) {
                ammunationsFired++;
                if (ammunationsFired >= ammunation) {
                    CancelInvoke("Fire");
                    Invoke("Reload", reloadTime); // This will stand as a wait time.
                }
            }
        }
        audioSource.Play();
    }    

    void Reload() {
        ammunationsFired = 0;
        InvokeRepeating("Fire", 0.0f, fireRate);
    }

    // Cancels firing and reloading. e.x stop firing if dead.
    public void StopFiring() {
        CancelInvoke();
    }
}
