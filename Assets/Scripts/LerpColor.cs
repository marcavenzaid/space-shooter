using UnityEngine;
using System.Collections;

public class LerpColor : MonoBehaviour {

    public byte[] colorA_4 = new byte[4];
    public byte[] colorB_4 = new byte[4];
    public float duration;    

    private Color32 colorA;
    private Color32 colorB;

    void Start () {
        colorA = new Color32(colorA_4[0], colorA_4[1], colorA_4[2], colorA_4[3]);
        colorB = new Color32(colorB_4[0], colorB_4[1], colorB_4[2], colorB_4[3]);
    }
	
	void Update () {
        Color32 lerpedColor = Color32.Lerp(colorA, colorB, Mathf.PingPong(Time.time, duration) / duration);
        gameObject.GetComponent<MeshRenderer>().material.color = lerpedColor;
    }
}
