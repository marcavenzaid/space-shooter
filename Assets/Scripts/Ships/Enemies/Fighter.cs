using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponControllerMissile))]
public class Fighter : BasicEnemy {

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
        foreach (GameObject missile in weaponControllerMissile.MissileAmmunation) {
            missile.SetActive(true);
        }
    }
}
