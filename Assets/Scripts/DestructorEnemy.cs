using UnityEngine;

[RequireComponent(typeof(Tumble))]
public class DestructorEnemy : MonoBehaviour {

    private void OnEnable() {
        gameObject.GetComponent<DisableOnDeath>().Enable();
        gameObject.GetComponent<Mover>().StopMoving();
        gameObject.GetComponent<WeaponController>().enabled = true;
        gameObject.GetComponent<WeaponControllerMissile>().enabled = true;
        gameObject.GetComponent<EvasiveManeuver>().enabled = true;
        gameObject.GetComponent<Tumble>().enabled = false;
    }

    public void DeathBehavior() {        
        gameObject.GetComponent<WeaponController>().StopFiring();
        gameObject.GetComponent<WeaponController>().enabled = false;
        gameObject.GetComponent<WeaponControllerMissile>().enabled = false;
        gameObject.GetComponent<Mover>().StopMoving();
        gameObject.GetComponent<EvasiveManeuver>().StopAllCoroutines();
        gameObject.GetComponent<EvasiveManeuver>().enabled = false;
        gameObject.GetComponent<Tumble>().enabled = true;
    }
}
