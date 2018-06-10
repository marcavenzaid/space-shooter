using UnityEngine;
using System.Collections;

public class BeamCollision : MonoBehaviour {

    public int damage;
    public GameObject explosion;
    public GameObject explosionPlayer;

    public bool Reflect = false;
	private BeamLine BL;

    public float hitAndShotEffectScale;
	public GameObject HitEffect = null;
    public GameObject ShotEffect = null;

    private bool bHit = false;

	private BeamParam BP;

    private GameController gameController; //===

    public LayerMask layerMask;

    // Use this for initialization
    void Start () {
		BL = (BeamLine)this.gameObject.transform.Find("BeamLine").GetComponent<BeamLine>();
		BP = this.transform.root.gameObject.GetComponent<BeamParam>();

        //=== Edited, for destroy player which is in gameController.GameOver()
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null) {
            gameController = gameControllerObject.GetComponent<GameController>();
        } else {
            Debug.Log("Cannot find 'Game Controller' script");
        }
        //===

        //=== Edited, for additional effects "SimpleFlash" in transform.position
        GameObject obj2 = (GameObject)Instantiate(ShotEffect, this.transform.position, Quaternion.AngleAxis(180.0f, transform.up) * this.transform.rotation);
        obj2.GetComponent<BeamParam>().SetBeamParam(BP);
        obj2.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f) * hitAndShotEffectScale;
        //===
    }

    // Update is called once per frame
    void Update () {
		//RayCollision
		RaycastHit hit;
        //int layerMask = ~(1 << LayerMask.NameToLayer("NoBeamHit") | 1 << 2); ,this is changed as a public LayerMask//===

        if (HitEffect != null && !bHit && Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layerMask))
        {            
			if(hit.distance < BL.GetNowLength())
		    {
				BL.StopLength(hit.distance);
				bHit = true;

                Quaternion Angle;
                //Reflect to Normal
                if (Reflect)
                {
                    Angle = Quaternion.LookRotation(Vector3.Reflect(transform.forward, hit.normal));
                }
                else
                {
                    //=== Edited, Assets > BasicBeamShot > Parts > S_BeamHit > SimpleFlash, transform.rotation.x = 90
                    Angle = Quaternion.AngleAxis(180.0f, transform.up) * this.transform.rotation;
                }
                GameObject obj = (GameObject)Instantiate(HitEffect,this.transform.position+this.transform.forward*hit.distance,Angle);                
                obj.GetComponent<BeamParam>().SetBeamParam(BP);
				obj.transform.localScale = this.transform.localScale * hitAndShotEffectScale;

                //g edited (DestroyByContact for this beam)===
                if (hit.collider.gameObject.CompareTag("Player")) {
                    int playerHealth = hit.collider.gameObject.GetComponent<Player>().GetHealth();

                    hit.collider.gameObject.GetComponent<HealthBar>().TakeDamage(damage);
                    hit.collider.gameObject.GetComponent<Player>().SubtractHealth(damage);
                    playerHealth -= damage;

                    if (playerHealth <= 0) {
                        gameController.GameOver();
                        Destroy(hit.rigidbody.gameObject);
                        Instantiate(explosionPlayer, hit.transform.position, hit.transform.rotation);
                    }               
                }
                //g===
			}
			//print("find" + hit.collider.gameObject.name);
		}
		/*
		if(bHit && BL != null)
		{
			BL.gameObject.renderer.material.SetFloat("_BeamLength",HitTimeLength / BL.GetNextLength() + 0.05f);
		}
		*/
	}
}
