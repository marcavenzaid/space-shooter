using UnityEngine;

public class DisableOnDeath : MonoBehaviour {

    public GameObject[] thisGameObject;
    public string[] scriptName;

    public void Disable() {
        for (int i = 0; i < thisGameObject.Length; i++) {
            thisGameObject[i].SetActive(false);
            //(GetComponent(scriptName[i]) as MonoBehaviour).enabled = false;
        }
    }

    public void Enable() {
        for (int i = 0; i < thisGameObject.Length; i++) {
            thisGameObject[i].SetActive(true);
            //(GetComponent(scriptName[i]) as MonoBehaviour).enabled = false;
        }
    }
}
