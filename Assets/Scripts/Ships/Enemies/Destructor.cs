using UnityEngine;

[RequireComponent(typeof(DisableOnDeath))]
[RequireComponent(typeof(Tumble))]
[RequireComponent(typeof(WeaponControllerMissile))]
public class Destructor : BasicEnemy {

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
        InvokeFire(false);
        StopMovement();
        GetComponent<EvasiveManeuver>().enabled = false;
        GetComponent<WeaponControllerMissile>().enabled = false;        
        GetComponent<Tumble>().enabled = true;
        GetComponent<DisableOnDeath>().Disable();
    }
}
