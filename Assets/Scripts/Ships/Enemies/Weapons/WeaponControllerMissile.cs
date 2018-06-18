using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WeaponControllerMissile : MonoBehaviour {
    
    [SerializeField] private float firstAttackDelayMin = 1, firstAttackDelayMax = 4;
    [SerializeField] private float fireRateMin = 0, fireRateMax = 4;
    private GameObject missileGameObject;
    private List<GameObject> missileAmmunation;
    private List<Transform> shotSpawns;
    private float nextFire;
    private int missileIndex;

    public GameObject MissileGameObject {
        private get { return missileGameObject; }
        set { missileGameObject = value; }
    }

    public List<GameObject> MissileAmmunation {
        get { return missileAmmunation; }
        set { missileAmmunation = value; }
    }

    public List<Transform> ShotSpawns {
        get { return shotSpawns; }
        set { shotSpawns = value; }
    }

    private void OnEnable() {
        missileIndex = 0;
    }

    private void Start() {
        FisherYates(missileAmmunation, ShotSpawns);
        nextFire = Time.time + Random.Range(firstAttackDelayMin, firstAttackDelayMax);
    }

    private void Update() {
        if (Time.time > nextFire && missileIndex < missileAmmunation.Count) {
            Fire();
            nextFire = Time.time + Random.Range(fireRateMin, fireRateMax);
        }                
    }

    private void Fire() {
        Instantiate(MissileGameObject, ShotSpawns[missileIndex].position, ShotSpawns[missileIndex].rotation);
        missileAmmunation[missileIndex].SetActive(false);
        missileIndex++;
    }

    public static void FisherYates(List<GameObject> missileDecorations, List<Transform> shotSpawns) {
        for (int i = missileDecorations.Count - 1; i > 0; i--) {
            int index = Random.Range(0, i);
            GameObject tmp = missileDecorations[index];
            missileDecorations[index] = missileDecorations[i];
            missileDecorations[i] = tmp;

            Transform tmp2 = shotSpawns[index];
            shotSpawns[index] = shotSpawns[i];
            shotSpawns[i] = tmp2;
        }
    }
}
