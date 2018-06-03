using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour {

    // The collider of the Boss is always a child so it is destroyed by DestroyByContact.
    void OnTriggerExit(Collider other) {
        RePool(other);      
    }

    void RePool(Collider thisGameObject) {
        thisGameObject.gameObject.SetActive(false);
    }
}
