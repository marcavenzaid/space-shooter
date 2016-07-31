using UnityEngine;
using System.Collections;

public class WeaponControllerBeam : MonoBehaviour {

    public GameObject[] shot;
    public Transform[] shotSpawn;
    public GameObject[] beamChargeEffect;
    public float[] maxScale;
    public float[] scaling;
    public float fireRateMin, fireRateMax;
    public float chargeSpeed;

    private AudioSource[] audioSource;
    private float[] nextChanceToFire = new float[5]; // I decided that the maximum amount of arrays for this script is 5    
    private bool[] charging = new bool[5]; // I decided that the maximum amount of arrays for this script is 5

    // The EnemyDroid Boss Ship chronological order of the elements is from 
    // left secondary weapon, right secondary weapon, left canon, then right canon

    void Start () {        
        for (int i = 0; i < shot.Length; i++) {
            maxScale[i] *= 0.75f;
            scaling[i] *= 0.75f;
        }
        audioSource = GetComponents<AudioSource>();
    }

    void OnEnable() {
        for (int i = 0; i < shot.Length; i++) {
            beamChargeEffect[i].transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            charging[i] = false;
            nextChanceToFire[i] = Time.time + Random.Range(fireRateMin, fireRateMax);
        }
    }
	
	void Update () {        
        for (int i = 0; i < shot.Length; i++) {
            if (Time.time >= nextChanceToFire[i] && !charging[i]) {
                charging[i] = true;
            }

            if (charging[i]) {
                if (beamChargeEffect[i].transform.localScale.x >= maxScale[i]) {
                    beamChargeEffect[i].transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    charging[i] = false;
                    
                    Instantiate(
                        shot[i], 
                        new Vector3(shotSpawn[i].position.x, 0.0f, shotSpawn[i].position.z), 
                        Quaternion.Euler(0.0f, shotSpawn[i].eulerAngles.y, 0.0f)
                    );

                    nextChanceToFire[i] = Time.time + Random.Range(fireRateMin, fireRateMax);
                    if (audioSource.Length == 1) {
                        audioSource[0].Play();
                    } else {
                        audioSource[1].Play();
                    }                    
                } else {
                    beamChargeEffect[i].transform.localScale += new Vector3(scaling[i], scaling[i], scaling[i]) * chargeSpeed * Time.deltaTime;
                }                
            }            
        }               
    }

    // This method will be used when the Ship is destroyed.
    public void StopAllBeamOperation() {
        for (int i = 0; i < shot.Length; i++) {
            beamChargeEffect[i].transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);                
        }
    }

    bool willIFire() {
        return (Random.value > 0.75f);
    }
}
