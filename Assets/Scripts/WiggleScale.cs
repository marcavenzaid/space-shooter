using UnityEngine;
using System.Collections;

public class WiggleScale : MonoBehaviour {
    
    public float wiggleSpeed;
    public float targetTime;
    public float minScale = 0.5f;

    private Vector3 originalScale;
    private Vector3 targetScale;
    private Vector3 previousScale;    
    private float wiggleSet;

	void Start () {
        originalScale = transform.localScale;
        targetScale = originalScale;
        previousScale = originalScale;
	}
	
	void Update () {
        if (targetTime >= 1.0f) {
            targetTime = 0.0f;
            wiggleSet =  Random.Range(minScale, 1.0f);
            //float w = wiggleSet * -wiggleSpeed * 0.5f + wiggleSpeed;
            targetScale = originalScale * wiggleSet;
            previousScale = originalScale;
        } else {
            targetTime += wiggleSpeed;
            transform.localScale = Vector3.Lerp(previousScale, targetScale, targetTime);
        }
	}
}
