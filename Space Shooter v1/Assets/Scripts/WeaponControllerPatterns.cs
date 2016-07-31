using UnityEngine;
using System.Collections;

public class WeaponControllerPatterns : MonoBehaviour {

    public Transform[] shotSpawns;
    public Transform middleShotSpawn;
    public Transform leftShotSpawn, rightShotSpawn;
    public float delayOfFirstAttack;
    public float delayOfNextAttack;

    public bool randomDirection, bombard, spiral, twinSpiral, greaterSpiral, sensoredSpiral, vectorSpread, circular, flower;

    private AudioSource audioSource;
    private float fireRate = 0.5f;
    private float LifeTimeOfPattern;
    private float LifeTimeOfAttack = 8.0f;
    private int pattern;
    private string[] patterns = new string[9];
    private int lengthOfPattern;
    private int index;

    private string bullet;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        GetPatterns();
        FisherYates_String(patterns);
        LifeTimeOfPattern = Time.time + delayOfFirstAttack; // The time the attackPattern lasts.        

        // Because of the iteration in ChoosePattern() and the way the Update() is written
        // it needs to be initialized at last element to make it the first element to be invoked.
        pattern = lengthOfPattern - 1;
        index = 0;
    }

    void Update() {
        if (Time.time > LifeTimeOfPattern) {
            StopFiring();
            LifeTimeOfPattern = Time.time + delayOfNextAttack + LifeTimeOfAttack;
            index = 0;
            ChoosePattern();
            InvokeRepeating(patterns[pattern], delayOfNextAttack, fireRate);
        }
    }

    public void StopFiring() {
        CancelInvoke(patterns[pattern]);
    }

    private void ChoosePattern() {
        // If the pattern length is reached then reshuffle the array.
        if (pattern + 1 == lengthOfPattern) {
            FisherYates_String(patterns);
        }

        pattern = (pattern + 1) % lengthOfPattern; // Iterates the pattern in a loop.

        switch (patterns[pattern]) {
            case "RandomDirection": fireRate = 0.25f; break;
            case "Spiral": fireRate = 0.05f; break;
            case "TwinSpiral": fireRate = 0.1f; break;
            case "GreaterSpiral": fireRate = 0.09f; break;
            case "SensoredSpiral": fireRate = 0.25f; break;
            case "VectorSpread": fireRate = 2.5f; break;
            case "Circular": fireRate = 0.5f; break;
            case "Flower": fireRate = 0.1f; break;
            case "Bombard": fireRate = 2.3f; break;
        }
    }

    private void FisherYates_String(string[] array) {
        for (int i = lengthOfPattern - 1; i > 0; i--) {
            int index = Random.Range(0, i);
            // swap
            string tmp = array[index];
            array[index] = array[i];
            array[i] = tmp;
        }
    }

    private void Fire(Vector3 shotSpawnPosition, Quaternion shotSpawnRotation, string bullet) {
        GameObject obj = GetComponent<MultiObjectPooler>().GetPooledObject(bullet);

        if (obj == null) return;

        obj.transform.position = new Vector3(shotSpawnPosition.x, 0.0f, shotSpawnPosition.z);
        obj.transform.rotation = Quaternion.Euler(0.0f, shotSpawnRotation.eulerAngles.y, 0.0f);
        obj.SetActive(true);
        audioSource.Play();
    }

    private void RandomDirection() {
        for (int i = 0; i < shotSpawns.Length; i++) {
            if (index % 2 == 0) {
                shotSpawns[i].rotation = Quaternion.Euler(0.0f, Random.Range(-30, 30), 0.0f);
                Fire(shotSpawns[i].position, shotSpawns[i].rotation, "BoltBlue(Clone)");
            }                   
        }

        index++;

        float rotationSpeed = 70.0f;
        middleShotSpawn.rotation = Quaternion.Euler(0.0f, Time.time * -rotationSpeed, 0.0f);
        Fire(middleShotSpawn.position, middleShotSpawn.rotation, "OrbPink(Clone)");
        middleShotSpawn.rotation = Quaternion.Euler(0.0f, Time.time * rotationSpeed, 0.0f);
        Fire(middleShotSpawn.position, middleShotSpawn.rotation, "OrbPink(Clone)");

        leftShotSpawn.rotation = Quaternion.Euler(0.0f, Mathf.Lerp(50.0f, 80.0f, Mathf.PingPong(Time.time * 1.0f, 1)), 0.0f);
        Fire(leftShotSpawn.position, leftShotSpawn.rotation, "OrbOrange(Clone)");
        leftShotSpawn.rotation = Quaternion.Euler(0.0f, Mathf.Lerp(20.0f, 50.0f, Mathf.PingPong(Time.time * 1.0f, 1)), 0.0f);
        Fire(leftShotSpawn.position, leftShotSpawn.rotation, "OrbOrange(Clone)");

        rightShotSpawn.rotation = Quaternion.Euler(0.0f, Mathf.Lerp(-20.0f, -50.0f, Mathf.PingPong(Time.time * 1.0f, 1)), 0.0f);
        Fire(rightShotSpawn.position, rightShotSpawn.rotation, "OrbOrange(Clone)");
        rightShotSpawn.rotation = Quaternion.Euler(0.0f, Mathf.Lerp(-50.0f, -80.0f, Mathf.PingPong(Time.time * 1.0f, 1)), 0.0f);
        Fire(rightShotSpawn.position, rightShotSpawn.rotation, "OrbOrange(Clone)");
    }

    private void Bombard() {
        for (int i = 0; i < 360; i += 30) {
            middleShotSpawn.rotation = Quaternion.Euler(0.0f, i, 0.0f); ;
            Fire(middleShotSpawn.position, middleShotSpawn.rotation, "BombConstant(Clone)");
        }
    }

    private void Spiral() {
        float rotationSpeed = 200.0f;
        middleShotSpawn.rotation = Quaternion.Euler(0.0f, Time.time * rotationSpeed, 0.0f);
        Fire(middleShotSpawn.position, middleShotSpawn.rotation, "OrbPink(Clone)");

        if (index % 20 == 0) {
            float rotationSpeed2 = 40.0f;
            leftShotSpawn.rotation = Quaternion.Euler(0.0f, Time.time * rotationSpeed2, 0.0f);
            Fire(leftShotSpawn.position, leftShotSpawn.rotation, "OrbHuge(Clone)");

            rightShotSpawn.rotation = Quaternion.Euler(0.0f, Time.time * -rotationSpeed2, 0.0f);
            Fire(rightShotSpawn.position, rightShotSpawn.rotation, "OrbHuge(Clone)");
        }
        index++;
    }

    private void TwinSpiral() {
        float rotationSpeed = 200.0f;
        leftShotSpawn.rotation = Quaternion.Euler(0.0f, Time.time * rotationSpeed, 0.0f);
        Fire(leftShotSpawn.position, leftShotSpawn.rotation, "OrbBlue(Clone)");
        Fire(leftShotSpawn.position, leftShotSpawn.rotation * Quaternion.Euler(0.0f, 90.0f, 0.0f), "OrbBlue(Clone)");
        Fire(leftShotSpawn.position, leftShotSpawn.rotation * Quaternion.Euler(0.0f, 180.0f, 0.0f), "OrbBlue(Clone)");
        Fire(leftShotSpawn.position, leftShotSpawn.rotation * Quaternion.Euler(0.0f, 270.0f, 0.0f), "OrbBlue(Clone)");

        rightShotSpawn.rotation = Quaternion.Euler(0.0f, Time.time * -rotationSpeed, 0.0f);
        Fire(rightShotSpawn.position, rightShotSpawn.rotation, "OrbBlue(Clone)");
        Fire(rightShotSpawn.position, rightShotSpawn.rotation * Quaternion.Euler(0.0f, 90.0f, 0.0f), "OrbBlue(Clone)");
        Fire(rightShotSpawn.position, rightShotSpawn.rotation * Quaternion.Euler(0.0f, 180.0f, 0.0f), "OrbBlue(Clone)");
        Fire(rightShotSpawn.position, rightShotSpawn.rotation * Quaternion.Euler(0.0f, 270.0f, 0.0f), "OrbBlue(Clone)");
    }

    private void GreaterSpiral() {
        float rotationSpeed = 100.0f;
        middleShotSpawn.rotation = Quaternion.Euler(0.0f, Time.time * rotationSpeed, 0.0f);

        Fire(middleShotSpawn.position, middleShotSpawn.rotation, "OrbBlue(Clone)");
        Fire(middleShotSpawn.position, middleShotSpawn.rotation * Quaternion.Euler(0.0f, 90.0f, 0.0f), "OrbBlue(Clone)");
        Fire(middleShotSpawn.position, middleShotSpawn.rotation * Quaternion.Euler(0.0f, 270.0f, 0.0f), "OrbBlue(Clone)");
        Fire(middleShotSpawn.position, middleShotSpawn.rotation * Quaternion.Euler(0.0f, 180.0f, 0.0f), "OrbBlue(Clone)");

        Fire(middleShotSpawn.position, middleShotSpawn.rotation * Quaternion.Euler(0.0f, 45.0f, 0.0f), "OrbOrange(Clone)");
        Fire(middleShotSpawn.position, middleShotSpawn.rotation * Quaternion.Euler(0.0f, 135.0f, 0.0f), "OrbOrange(Clone)");
        Fire(middleShotSpawn.position, middleShotSpawn.rotation * Quaternion.Euler(0.0f, 225.0f, 0.0f), "OrbOrange(Clone)");
        Fire(middleShotSpawn.position, middleShotSpawn.rotation * Quaternion.Euler(0.0f, 315.0f, 0.0f), "OrbOrange(Clone)");
    }

    private void SensoredSpiral() {
        float rotationSpeed = -100.0f;
        middleShotSpawn.rotation = Quaternion.Euler(0.0f, Time.time * rotationSpeed, 0.0f);
        Fire(middleShotSpawn.position, middleShotSpawn.rotation, "SensoredOrb(Clone)");
    }

    private void VectorSpread() {
        for (int i = 0; i < 16; i++) {
            middleShotSpawn.rotation = Quaternion.Euler(0.0f, i * 12.0f - 90.0f, 0.0f); // 37.5 is offset, 5 is degree.
            Fire(middleShotSpawn.position, middleShotSpawn.rotation, "VectorCircle(Clone)");
        }
    }

    private void Circular() {
        for (int i = 0; i <= 360 - 6; i += 6) {
            middleShotSpawn.rotation = Quaternion.Euler(0.0f, i + index, 0.0f);
            Fire(middleShotSpawn.position, middleShotSpawn.rotation, "OrbBlue(Clone)");
        }
        index++;
    }

    private void Flower() {
        float rotationSpeed = 70.0f;
        middleShotSpawn.rotation = Quaternion.Euler(0.0f, Time.time * rotationSpeed, 0.0f);
        Fire(middleShotSpawn.position, middleShotSpawn.rotation, "OrbBlue(Clone)");
        Fire(middleShotSpawn.position, middleShotSpawn.rotation * Quaternion.Euler(0.0f, 90.0f, 0.0f), "OrbBlue(Clone)");
        Fire(middleShotSpawn.position, middleShotSpawn.rotation * Quaternion.Euler(0.0f, 180.0f, 0.0f), "OrbBlue(Clone)");
        Fire(middleShotSpawn.position, middleShotSpawn.rotation * Quaternion.Euler(0.0f, 270.0f, 0.0f), "OrbBlue(Clone)");

        middleShotSpawn.rotation = Quaternion.Euler(0.0f, Time.time * -rotationSpeed, 0.0f);
        Fire(middleShotSpawn.position, middleShotSpawn.rotation * Quaternion.Euler(0.0f, 45.0f, 0.0f), "OrbBlue(Clone)");
        Fire(middleShotSpawn.position, middleShotSpawn.rotation * Quaternion.Euler(0.0f, 135.0f, 0.0f), "OrbBlue(Clone)");
        Fire(middleShotSpawn.position, middleShotSpawn.rotation * Quaternion.Euler(0.0f, 225.0f, 0.0f), "OrbBlue(Clone)");
        Fire(middleShotSpawn.position, middleShotSpawn.rotation * Quaternion.Euler(0.0f, 315.0f, 0.0f), "OrbBlue(Clone)");
        
        if (index % 10 == 0) {
            Quaternion reverseRotation = Quaternion.Euler(0.0f, Time.time * rotationSpeed, 0.0f);
            Fire(middleShotSpawn.position, middleShotSpawn.rotation, "OrbPink(Clone)");
            Fire(middleShotSpawn.position, middleShotSpawn.rotation * Quaternion.Euler(0.0f, 90.0f, 0.0f), "OrbPink(Clone)");
            Fire(middleShotSpawn.position, middleShotSpawn.rotation * Quaternion.Euler(0.0f, 180.0f, 0.0f), "OrbPink(Clone)");
            Fire(middleShotSpawn.position, middleShotSpawn.rotation * Quaternion.Euler(0.0f, 270.0f, 0.0f), "OrbPink(Clone)");
            Fire(middleShotSpawn.position, reverseRotation * Quaternion.Euler(0.0f, 45.0f, 0.0f), "OrbPink(Clone)");
            Fire(middleShotSpawn.position, reverseRotation * Quaternion.Euler(0.0f, 135.0f, 0.0f), "OrbPink(Clone)");
            Fire(middleShotSpawn.position, reverseRotation * Quaternion.Euler(0.0f, 225.0f, 0.0f), "OrbPink(Clone)");
            Fire(middleShotSpawn.position, reverseRotation * Quaternion.Euler(0.0f, 315.0f, 0.0f), "OrbPink(Clone)");
        }
        index++;
    }

    private void GetPatterns() {
        int i = 0;

        if (randomDirection) {
            patterns[i] = "RandomDirection";
            i++;
        }
        if (bombard) {
            patterns[i] = "Bombard";
            i++;
        }
        if (spiral) {
            patterns[i] = "Spiral";
            i++;
        }
        if (twinSpiral) {
            patterns[i] = "TwinSpiral";
            i++;
        }
        if (greaterSpiral) {
            patterns[i] = "GreaterSpiral";
            i++;
        }
        if (sensoredSpiral) {
            patterns[i] = "SensoredSpiral";
            i++;
        }
        if (vectorSpread) {
            patterns[i] = "VectorSpread";
            i++;
        }
        if (circular) {
            patterns[i] = "Circular";
            i++;
        }
        if (flower) {
            patterns[i] = "Flower";
            i++;
        }

        lengthOfPattern = i;
    }
}
