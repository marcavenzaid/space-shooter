using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemySpawnerScript : MonoBehaviour {

    public Transform middleLeftSpawner;
    public Transform middleRightSpawner;
    public GameObject bossHealthBar; // used only when boss appeared.
    public Slider healthSlider;

    public float startWait;

    private GameObject thisObject;

    void Start () {        
        StartCoroutine(SpawnWave());        
    } 

    IEnumerator SpawnWave() {
        // 1
        yield return new WaitForSeconds(startWait);
        Spawn("Frigate(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);
        //
        yield return new WaitForSeconds(3);
        Spawn("FrigateStill(Clone)", new Vector3(0, 0, 21), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Spawn("FrigateStill(Clone)", new Vector3(-2, 0, 21), Quaternion.identity);        
        Spawn("FrigateStill(Clone)", new Vector3(2, 0, 21), Quaternion.identity);
        // Red First Appearance
        yield return new WaitForSeconds(5);
        Spawn("Red(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);
        //
        yield return new WaitForSeconds(4);
        for (int i = 0; i < 2; i++) {
            Spawn("Red(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);
            yield return new WaitForSeconds(0.7f);
        }
        // Destructor First Appearance
        yield return new WaitForSeconds(6);
        Spawn("Destructor(Clone)", new Vector3(0, 0, 21), Quaternion.identity);
        //
        yield return new WaitForSeconds(10);
        Spawn("FrigateStill(Clone)", new Vector3(-5, 0, 21), Quaternion.identity);
        Spawn("FrigateStill(Clone)", new Vector3(0, 0, 21), Quaternion.identity);        
        Spawn("FrigateStill(Clone)", new Vector3(5, 0, 21), Quaternion.identity);
        // Trident First Appearance
        yield return new WaitForSeconds(8);
        Spawn("Trident(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);
        //
        yield return new WaitForSeconds(10);        
        Spawn("Fighter(Clone)", new Vector3(-6, 0, 21), Quaternion.identity);
        Spawn("Droid(Clone)", new Vector3(0, 0, 21), Quaternion.identity);
        Spawn("Fighter(Clone)", new Vector3(6, 0, 21), Quaternion.identity);
        //
        // Green First Appearance
        yield return new WaitForSeconds(8);
        for (int i = 0; i < 3; i++) {
            Spawn("Green(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);
            yield return new WaitForSeconds(0.7f);
        }
        //
        yield return new WaitForSeconds(8);
        for (int i = 0; i < 3; i++) {
            Spawn("Red(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);
            yield return new WaitForSeconds(1);
        }
        // Large wave
        yield return new WaitForSeconds(2);
        for (int i = 0; i < 3; i++) {
            Spawn("Droid(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);
            yield return new WaitForSeconds(1);
        }
        yield return new WaitForSeconds(6);
        for (int i = 0; i < 3; i++) {  
            Spawn("Fighter(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);                     
            yield return new WaitForSeconds(2);
        }
        yield return new WaitForSeconds(7);
        for (int i = 0; i < 2; i++) {
            Spawn("Green(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);
            yield return new WaitForSeconds(2);
        }
        yield return new WaitForSeconds(5);
        Spawn("Trident(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);
        yield return new WaitForSeconds(3);
        Spawn("Trident(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);
        yield return new WaitForSeconds(5);
        for (int i = 0; i < 10; i++) {
            Spawn("Frigate(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(0.8f, 1.5f));
        }

        // DarkFighter Appearance
        yield return new WaitForSeconds(6);
        Spawn("DarkFighter(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);

        // Wait until DarkFighter(Clone) is not active in hierarchy
        while (thisObject.activeInHierarchy) {
            yield return null;
        }
        //

        // Boss Appearance        
        yield return new WaitForSeconds(3);
        // Stop the firing of the player.
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().CancelInvokeFire();

        yield return new WaitForSeconds(3);
        Spawn("AK5(Clone)", new Vector3(0.0f, 0.0f, 24), Quaternion.identity);

        // Invoke the firing of the player.
        yield return new WaitForSeconds(15);
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().InvokeFire();
        //        
    }

    private void Spawn(string thisEnemy, Vector3 thisPosition, Quaternion thisRotation) {
        GameObject obj = GetComponent<MultiObjectPooler>().GetPooledObject(thisEnemy);        

        if (obj == null)
            return;

        obj.transform.position = thisPosition;
        obj.transform.rotation = thisRotation;
        obj.SetActive(true);

        if (obj.name == "DarkFighter(Clone)") {
            thisObject = obj;
        }

        if (obj.CompareTag("Boss")) {
            thisObject = obj;

            // Remember to make the Boss SetActive(false).
            // This will set the healthSlider of the boss to active.
            if (obj.CompareTag("Boss")) {
                healthSlider.value = healthSlider.maxValue;
                bossHealthBar.SetActive(true);
            }            
        }
    }

    private Vector3 RandomPositionXBetween(Transform transform1, Transform transform2) {
        Vector3 transformPosition = new Vector3(
            Random.Range(transform1.position.x, transform2.position.x),
            0.0f,
            transform1.position.z
        );
        return transformPosition;
    }
}
