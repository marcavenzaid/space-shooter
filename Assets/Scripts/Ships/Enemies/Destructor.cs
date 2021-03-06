﻿using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DisableOnDeath))]
[RequireComponent(typeof(Tumble))]
[RequireComponent(typeof(WeaponControllerMissile))]
public class Destructor : BasicEnemy {

    [SerializeField] private GameObject missileGameObject;
    [SerializeField] private Transform missileAmmunation;
    [SerializeField] private Transform missileShotSpawns;
    private WeaponControllerMissile weaponControllerMissile;

    protected override void Awake() {
        base.Awake();
        weaponControllerMissile = GetComponent<WeaponControllerMissile>();

        weaponControllerMissile.MissileGameObject = missileGameObject;

        List<GameObject> missileDecorationList = new List<GameObject>();
        for (int i = 0; i < missileAmmunation.childCount; i++) {
            missileDecorationList.Add(missileAmmunation.GetChild(i).gameObject);
        }
        weaponControllerMissile.MissileAmmunation = missileDecorationList;

        List<Transform> missileShotSpawnList = new List<Transform>();
        for (int i = 0; i < missileShotSpawns.childCount; i++) {
            missileShotSpawnList.Add(missileShotSpawns.GetChild(i));
        }
        weaponControllerMissile.ShotSpawns = missileShotSpawnList;
    }

    protected override void OnEnable() {
        base.OnEnable();
        GetComponent<EvasiveManeuver>().enabled = true;
        GetComponent<WeaponControllerMissile>().enabled = true;
        GetComponent<Tumble>().enabled = false;
        GetComponent<DisableOnDeath>().Enable();
    }

    protected override void Death() {
        Instantiate(Explosion, transform.position, transform.rotation);
        PowerUpDroper.DropPowerUps();
        InvokeRepeatingFire(false);
        StopMovement();
        GetComponent<EvasiveManeuver>().enabled = false;
        GetComponent<WeaponControllerMissile>().enabled = false;        
        GetComponent<Tumble>().enabled = true;
        GetComponent<DisableOnDeath>().Disable();
    }
}
