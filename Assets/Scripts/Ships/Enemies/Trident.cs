using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EvasiveManeuver))]
[RequireComponent(typeof(WeaponControllerBeam))]
public class Trident : Enemy {

    [SerializeField] private Transform beamShotSpawns;
    private WeaponControllerBeam weaponControllerBeam;

    protected override void Awake() {
        base.Awake();
        weaponControllerBeam = GetComponent<WeaponControllerBeam>();
        weaponControllerBeam.BeamShotSpawns = beamShotSpawns;
    }

    protected override void OnEnable() {
        base.OnEnable();
        MoveForward();
    }

    private void FixedUpdate() {
        ClampXPosition();
    }
}
