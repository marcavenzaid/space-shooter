using UnityEngine;
using System.Collections;

public class BeamChargeEffect : MonoBehaviour {
    
    public GameObject beamChargeEffect;
    public float maxScale;
    public float scaling;
    public float chargeTime;

    void Start() {
        maxScale *= 0.75f;
        scaling *= 0.75f;
    }

	void Update () {
        // chargeSpeed should be the same as the value of fireRate and delay in WeaponController to match the animation.
        if (beamChargeEffect.transform.localScale.x >= maxScale) {
            beamChargeEffect.transform.localScale = new Vector3(scaling, scaling, scaling);
        } else {
            beamChargeEffect.transform.localScale += new Vector3(scaling, scaling, scaling) * chargeTime * Time.deltaTime;
        }
	}
}
