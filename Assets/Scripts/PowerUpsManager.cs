using UnityEngine;
using System.Collections;

public class PowerUpsManager : MonoBehaviour {

    public int powerup_type; // powerup_type: 1 = fireRate, 2 = widen;
    
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (powerup_type == 1) {
                other.GetComponent<Player>().UpgradeFireRate();             
            }

            if (powerup_type == 2) {
                int weaponLevel = other.GetComponent<Player>().GetWeaponlevel();
                other.GetComponent<Player>().SetWeaponLevel(weaponLevel + 1);
            }

            Destroy(gameObject);
        }        
    }    
}
