using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Enemy))]
public class ExplodeInDeath : MonoBehaviour {

    public GameObject[] explosions;
    public float rangeX;
    public float rangeZ;
    public float explosionsRate; // This is used in the DestroyByContact (script) when MultipleExplosions() is called.
    public float explosionsDuration;

    private Enemy enemy;
    private float startTimeOfExplosion;
    private bool exploding;
    private bool doneExploding;

    // Make sure this script is disabled.

    void Start() {
        enemy = GetComponent<Enemy>();
        exploding = false;
        doneExploding = false;
    }

    void Update() {
        if (!enemy.IsAlive() && !exploding && !doneExploding) {
            StartMultipleExplosions();
        }

        if (exploding) {
            if (Time.time > startTimeOfExplosion) {
                CancelInvoke("MultipleExplosions");
                exploding = false;
                doneExploding = true;
            }            
        }
    }

    public void StartMultipleExplosions() {
        InvokeRepeating("MultipleExplosions", 0.0f, explosionsRate);
        exploding = true;
        startTimeOfExplosion = Time.time + explosionsDuration;
    }

    public void MultipleExplosions() {                
        Instantiate(explosions[Random.Range(0, explosions.Length)], new Vector3(
            transform.position.x + Random.Range(-rangeX, rangeX),
            0.0f,
            transform.position.z + Random.Range(-rangeZ, rangeZ)),
        transform.rotation);
    }
}
