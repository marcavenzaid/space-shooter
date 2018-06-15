using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EvasiveManeuver))]
public class BasicEnemy : Enemy {

    private float targetManeuver;
    private Transform[] shotSpawnArr;    

    protected override void Awake() {
        base.Awake();
        if (ShotSpawns != null && ShotSpawns.childCount > 0) {
            shotSpawnArr = new Transform[ShotSpawns.childCount];
            for (int i = 0; i < ShotSpawns.childCount; i++) {
                shotSpawnArr[i] = ShotSpawns.GetChild(i);
            }
        } else {
            shotSpawnArr = new Transform[] { ShotSpawns };
        }
    }

    protected override void OnEnable() {
        base.OnEnable();
        MoveForward();
        InvokeFire(true);
    }

    private void OnDisable() {
        InvokeFire(false);
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

    protected void InvokeFire(bool invoke) {
        if (invoke) {
            InvokeRepeating("Fire", FirstAttackDelay, FireRate);
        } else {
            CancelInvoke("Fire");
        }
    }
}
