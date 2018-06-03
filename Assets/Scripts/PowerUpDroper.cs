using UnityEngine;
using System.Collections;

public class PowerUpDroper : MonoBehaviour {

    public GameObject[] powerUps;
    public float powerUpDropChance; // Power ups  

    public void DropPowerUps() {
        if (Random.value < powerUpDropChance) {
            Instantiate(powerUps[Random.Range(0, 2)], transform.position, Quaternion.Euler(0, 0, 0));
        }
    }
}
