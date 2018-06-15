using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiObjectPooler : MonoBehaviour {

    public GameObject[] pooledObject;
    public int[] pooledAmount;
    public bool willGrow = false;
    public string[] pooledObjectsString;
    private List<GameObject> pooledObjects;
    private GameObject lastGivenObject;

    private void Awake() {
        pooledObjects = new List<GameObject>();
        for (int j = 0; j < pooledObject.Length; j++) {
            for (int i = 0; i < pooledAmount[j]; i++) {
                GameObject obj = Instantiate(pooledObject[j]); 
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }    

    public GameObject GetPooledObject(string thisObject) {
        for (int i = 0; i < pooledObjects.Count; i++) {
            if(pooledObjects[i].name == thisObject && !pooledObjects[i].activeInHierarchy) {
                lastGivenObject = pooledObjects[i];
                return pooledObjects[i];
            }
        }

        if (willGrow) {
            GameObject obj = Instantiate(pooledObject[FindIndex(thisObject)]);
            pooledObjects.Add(obj);
            lastGivenObject = obj;
            return obj;
        }
        return null;
    }

    public GameObject GetLastGivenObject() {
        return lastGivenObject;
    }

    private int FindIndex(string thisString) {
        for (int i = 0; i < pooledObjectsString.Length; i++) {
            if (string.Equals(thisString, pooledObjectsString[i])) {
                return i;
            }
        }
        return 0;
    }
}
