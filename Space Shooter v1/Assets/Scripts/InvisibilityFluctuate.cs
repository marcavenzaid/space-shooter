using UnityEngine;
using System.Collections;

public class InvisibilityFluctuate : MonoBehaviour {

    public float duration;
    public byte maxAlpha;

    private Color32 colorA, colorB;    

    void Start() {
        colorA = new Color32(255, 255, 255, 0);
        colorB = new Color32(255, 255, 255, maxAlpha);
    }

    void Update() {
        Color32 textureColor = Color.Lerp(colorA, colorB, Mathf.PingPong(Time.time, duration));
        GetComponent<MeshRenderer>().material.color = textureColor;           
    }
}
