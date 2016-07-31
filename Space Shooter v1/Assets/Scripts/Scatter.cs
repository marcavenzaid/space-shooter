using UnityEngine;
using System.Collections;

public class Scatter : MonoBehaviour {

    public GameObject effect;
    public float detonationTimeMin, detonationTimeMax;
    public int quantity;

    private float detonationTime;

    void OnEnable() {
        detonationTime = Time.time + Random.Range(detonationTimeMin, detonationTimeMax);
    }

    void Update() {
        if (gameObject.activeInHierarchy && Time.time >= detonationTime) {
            Explode();            
        }
    } 

    public void Explode() {        
        Instantiate(effect, transform.position, transform.rotation); // Explosion effect.

        for (int i = 0; i < 18; i++) {
            GameObject obj = GetComponent<ObjectPoolerScript>().GetPooledObject();

            obj.transform.position = transform.position;
            obj.transform.rotation = Quaternion.Euler(0.0f, i * 20, 0.0f);
            obj.SetActive(true);
        }

        RePool(gameObject);
    }

    void RePool(GameObject thisGameObject) {
        thisGameObject.SetActive(false);
    }
}
