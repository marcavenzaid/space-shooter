using UnityEngine;
using System.Collections;

public class WeaponControllerMissile : MonoBehaviour {

    public GameObject shot;
    public Transform[] shotSpawns;
    public GameObject[] missiles;
    public float firstAttackDelayMin, firstAttackDelayMax;
    public float fireRateMin, fireRateMax;

    private float nextFire;
    private int missileIndex;

    void OnEnable() {
        FisherYates2(shotSpawns, missiles); // randomize the array of shotSpawns.
        missileIndex = 0;
        nextFire = Time.time + Random.Range(firstAttackDelayMin, firstAttackDelayMax);
        for (int i = 0; i < missiles.Length; i++) {
            missiles[i].SetActive(true);
        }
    }

    void Update() {
        if (missileIndex < shotSpawns.Length && missiles[missileIndex] != null) {
            if (Time.time > nextFire) {
                Fire();
                nextFire = Time.time + Random.Range(fireRateMin, fireRateMax);
            }
        }                
    }

    void Fire() {
        Instantiate(shot, shotSpawns[missileIndex].position, shotSpawns[missileIndex].rotation);
        missiles[missileIndex].SetActive(false);
        missileIndex++;
    }

    private void FisherYates2(Transform[] array1, GameObject[] array2) {
        for (int i = array1.Length - 1; i > 0; i--) {
            int index = Random.Range(0, i);
            // swap array1
            Transform tmp1 = array1[index];
            array1[index] = array1[i];
            array1[i] = tmp1;
            //swap array2
            GameObject tmp2 = array2[index];
            array2[index] = array2[i];
            array2[i] = tmp2;
        }
    }
}
