using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EvasiveManeuver))]
[RequireComponent(typeof(WeaponControllerBeam))]
public class Trident : Enemy {

    protected override void OnEnable() {
        base.OnEnable();
        MoveForward();
    }

    private void FixedUpdate() {
        ClampXPosition();
    }
}
