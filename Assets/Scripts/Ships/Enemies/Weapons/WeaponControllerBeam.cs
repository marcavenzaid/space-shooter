using UnityEngine;
using System.Collections;

public class WeaponControllerBeam : MonoBehaviour {
    
    [SerializeField] private GameObject[] beamChargeEffect;
    [SerializeField] private float[] beamChargeMaxScale;
    [SerializeField] private float[] beamChargeScaling;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float fireRateMin = 1, fireRateMax = 4;        
    private Ship ship;
    private Transform beamShotSpawns;
    private Transform[] shotSpawnArr;
    private float[] nextFireTime;

    public Transform BeamShotSpawns {
        set { beamShotSpawns = value; }
    }

    private void Awake() {
        ship = GetComponent<Ship>();
    }

    private void OnEnable() {
        for (int i = 0; i < beamChargeEffect.Length; i++) {
            beamChargeEffect[i].transform.localScale = new Vector3(0, 0, 1);
        }
    }

    private void Start() {
        if (beamShotSpawns.childCount > 0) {
            shotSpawnArr = new Transform[beamShotSpawns.childCount];
            for (int i = 0; i < beamShotSpawns.childCount; i++) {
                shotSpawnArr[i] = beamShotSpawns.GetChild(i);
            }
        } else {
            shotSpawnArr = new Transform[] { beamShotSpawns };
        }

        nextFireTime = new float[shotSpawnArr.Length];
        for (int i = 0; i < nextFireTime.Length; i++) {
            nextFireTime[i] = Time.time + Random.Range(fireRateMin, fireRateMax);
        }        
    }

    private void Update() {        
        for (int i = 0; i < shotSpawnArr.Length; i++) {
            if (Time.time >= nextFireTime[i]) {
                Fire(i);
            }            
        }        
    }

    private void Fire(int i) {
        if (beamChargeEffect[i].transform.localScale.x < beamChargeMaxScale[i]) {
            beamChargeEffect[i].transform.localScale += new Vector3(beamChargeScaling[i], beamChargeScaling[i], 0) * chargeSpeed * Time.deltaTime;
        } else {
            Instantiate(ship.ShotGameObject, shotSpawnArr[i].position, shotSpawnArr[i].rotation);
            ship.WeaponAudioSource.Play();
            nextFireTime[i] = Time.time + Random.Range(fireRateMin, fireRateMax);
            beamChargeEffect[i].transform.localScale = new Vector3(0, 0, 1);
        }
    }
}
