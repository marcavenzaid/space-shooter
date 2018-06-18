using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EvasiveManeuver))]
public class BasicEnemy : Enemy {

    [SerializeField] private Transform shotSpawns;
    private float targetManeuver;
    private Transform[] shotSpawnArr;    

    protected override void OnEnable() {
        base.OnEnable();
        MoveForward();
        InvokeRepeatingFire(true);
    }

    private void Start() {
        if (shotSpawns != null && shotSpawns.childCount > 0) {
            shotSpawnArr = new Transform[shotSpawns.childCount];
            for (int i = 0; i < shotSpawns.childCount; i++) {
                shotSpawnArr[i] = shotSpawns.GetChild(i);
            }
        } else {
            shotSpawnArr = new Transform[] { shotSpawns };
        }
    }

    private void OnDisable() {
        InvokeRepeatingFire(false);
    }

    private void FixedUpdate() {
        ClampXPosition();
    }    

    private void Fire() {
        for (int i = 0; i < shotSpawnArr.Length; i++) {
            GameObject obj = GetComponent<ObjectPoolerScript>().GetPooledObject();

            if (obj == null) {
                return;
            }
            shotSpawnArr[i].rotation = Quaternion.identity;
            obj.transform.position = shotSpawnArr[i].position;
            obj.transform.eulerAngles = shotSpawnArr[i].eulerAngles;
            obj.SetActive(true);
        }
        WeaponAudioSource.Play();
    }

    protected void InvokeRepeatingFire(bool invoke) {
        if (invoke) {
            InvokeRepeating("Fire", FirstAttackDelay, FireRate);
        } else {
            CancelInvoke("Fire");
        }
    }
}
