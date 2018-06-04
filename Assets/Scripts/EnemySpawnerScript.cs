using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(MultiObjectPooler))]
public class EnemySpawnerScript : MonoBehaviour {

    [SerializeField] private Transform middleLeftSpawner;
    [SerializeField] private Transform middleRightSpawner;
    [SerializeField] private float startWait;
    private MultiObjectPooler multiObjectPooler;

    void Start () {        
        StartCoroutine(SpawnWave());
        multiObjectPooler = GetComponent<MultiObjectPooler>();
    }

    private void SpawnPooled(string thisEnemy, Vector3 thisPosition, Quaternion thisRotation) {
        GameObject obj = multiObjectPooler.GetPooledObject(thisEnemy);

        if (obj == null) {
            return;
        }
        obj.transform.position = thisPosition;
        obj.transform.rotation = thisRotation;
        obj.SetActive(true);
    }

    private GameObject GetLastSpawned() {
        return multiObjectPooler.GetLastGivenObject();
    }

    private Vector3 RandomPositionXBetween(Transform transform1, Transform transform2) {
        Vector3 transformPosition = new Vector3(
            Random.Range(transform1.position.x, transform2.position.x),
            0.0f,
            transform1.position.z
        );
        return transformPosition;
    }

    IEnumerator SpawnWave() {
        // 1
        yield return new WaitForSeconds(startWait);
        SpawnPooled("Frigate(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);
        
        yield return new WaitForSeconds(3);
        SpawnPooled("FrigateStill(Clone)", new Vector3(0, 0, 21), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        SpawnPooled("FrigateStill(Clone)", new Vector3(-2, 0, 21), Quaternion.identity);
        SpawnPooled("FrigateStill(Clone)", new Vector3(2, 0, 21), Quaternion.identity);
        // Red First Appearance
        yield return new WaitForSeconds(5);
        SpawnPooled("Red(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);
        
        yield return new WaitForSeconds(4);
        for (int i = 0; i < 2; i++) {
            SpawnPooled("Red(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);
            yield return new WaitForSeconds(0.7f);
        }
        // Destructor First Appearance
        yield return new WaitForSeconds(6);
        SpawnPooled("Destructor(Clone)", new Vector3(0, 0, 21), Quaternion.identity);
        
        yield return new WaitForSeconds(10);
        SpawnPooled("FrigateStill(Clone)", new Vector3(-5, 0, 21), Quaternion.identity);
        SpawnPooled("FrigateStill(Clone)", new Vector3(0, 0, 21), Quaternion.identity);
        SpawnPooled("FrigateStill(Clone)", new Vector3(5, 0, 21), Quaternion.identity);
        // Trident First Appearance
        yield return new WaitForSeconds(8);
        SpawnPooled("Trident(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);
        
        yield return new WaitForSeconds(10);
        SpawnPooled("Fighter(Clone)", new Vector3(-6, 0, 21), Quaternion.identity);
        SpawnPooled("Droid(Clone)", new Vector3(0, 0, 21), Quaternion.identity);
        SpawnPooled("Fighter(Clone)", new Vector3(6, 0, 21), Quaternion.identity);
        
        // Green First Appearance
        yield return new WaitForSeconds(8);
        for (int i = 0; i < 3; i++) {
            SpawnPooled("Green(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);
            yield return new WaitForSeconds(0.7f);
        }
        
        yield return new WaitForSeconds(8);
        for (int i = 0; i < 3; i++) {
            SpawnPooled("Red(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);
            yield return new WaitForSeconds(1);
        }
        // Large wave
        yield return new WaitForSeconds(2);
        for (int i = 0; i < 3; i++) {
            SpawnPooled("Droid(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);
            yield return new WaitForSeconds(1);
        }
        yield return new WaitForSeconds(6);
        for (int i = 0; i < 3; i++) {
            SpawnPooled("Fighter(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);
            yield return new WaitForSeconds(2);
        }
        yield return new WaitForSeconds(7);
        for (int i = 0; i < 2; i++) {
            SpawnPooled("Green(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);
            yield return new WaitForSeconds(2);
        }
        yield return new WaitForSeconds(5);
        SpawnPooled("Trident(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);
        yield return new WaitForSeconds(3);
        SpawnPooled("Trident(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);
        yield return new WaitForSeconds(5);
        for (int i = 0; i < 10; i++) {
            SpawnPooled("Frigate(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(0.8f, 1.5f));
        }

        // DarkFighter Appearance
        yield return new WaitForSeconds(6);
        SpawnPooled("DarkFighter(Clone)", RandomPositionXBetween(middleLeftSpawner, middleRightSpawner), Quaternion.identity);

        // Wait until DarkFighter(Clone) is not active in hierarchy
        Enemy enemy = GetLastSpawned().GetComponent<Enemy>();
        while (enemy.IsAlive()) {
            yield return null;
        }
        

        // Boss Appearance        
        yield return new WaitForSeconds(3);
        // Stop the firing of the player.
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().InvokeFire(false);

        yield return new WaitForSeconds(3);
        SpawnPooled("AK5(Clone)", new Vector3(0.0f, 0.0f, 24), Quaternion.identity);

        // Invoke the firing of the player.
        yield return new WaitForSeconds(15);
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().InvokeFire(true);
    }    
}
