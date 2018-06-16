using UnityEngine;
using System.Collections;

public class WeaponControllerBeam : MonoBehaviour {
    
    [SerializeField] private GameObject[] beamChargeEffect;
    [SerializeField] private float[] beamChargeMaxScale;
    [SerializeField] private float[] beamChargeScaling;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float fireRateMin, fireRateMax;        
    private Ship ship;
    private GameObject shotGameObject;
    private Transform[] shotSpawnArr;
    private int shotSpawnArrLength;
    private AudioSource weaponAudioSource;
    private float[] nextChanceToFire;
    private bool[] isCharging;

    private void Awake() {
        ship = GetComponent<Ship>();
        shotGameObject = ship.ShotGameObject;
        if (ship.ShotSpawns != null && ship.ShotSpawns.childCount > 0) {
            shotSpawnArr = new Transform[ship.ShotSpawns.childCount];
            for (int i = 0; i < ship.ShotSpawns.childCount; i++) {
                shotSpawnArr[i] = ship.ShotSpawns.GetChild(i);
            }
        } else {
            shotSpawnArr = new Transform[] { ship.ShotSpawns };
        }
        shotSpawnArrLength = shotSpawnArr.Length;
        nextChanceToFire = new float[shotSpawnArrLength];
        isCharging = new bool[shotSpawnArrLength];
        weaponAudioSource = ship.WeaponAudioSource;
    }

    private void OnEnable() {
        for (int i = 0; i < shotSpawnArrLength; i++) {
            beamChargeEffect[i].transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            nextChanceToFire[i] = Time.time + Random.Range(fireRateMin, fireRateMax);
            isCharging[i] = false;
        }
    }

	private void Update () {        
        for (int i = 0; i < shotSpawnArrLength; i++) {
            if (Time.time >= nextChanceToFire[i] && !isCharging[i]) {
                isCharging[i] = true;
            }

            if (isCharging[i]) {
                if (beamChargeEffect[i].transform.localScale.x < beamChargeMaxScale[i]) {
                    beamChargeEffect[i].transform.localScale += new Vector3(beamChargeScaling[i], beamChargeScaling[i], 0) * chargeSpeed * Time.deltaTime;                                      
                } else {
                    Instantiate(shotGameObject, shotSpawnArr[i].position, shotSpawnArr[i].rotation);
                    weaponAudioSource.Play();
                    nextChanceToFire[i] = Time.time + Random.Range(fireRateMin, fireRateMax);                    
                    beamChargeEffect[i].transform.localScale = new Vector3(0, 0, 1);
                    isCharging[i] = false;
                }
            }
        }
    }

    // This method will be used when the Ship is destroyed.
    public void StopAllBeamOperation() {
        for (int i = 0; i < shotSpawnArrLength; i++) {
            beamChargeEffect[i].transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);                
        }
    }
}
