using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {

    public GameObject explosion;
    public GameObject playerExplosion;
    public int enemyHealth;    
    public int scoreValue;
    [HideInInspector]
    public bool isDead;
    
    private GameController gameController;
    private GameObject playerObject;
    private GameObject playerBoltExplosion;
    private GameObject boltExplosion;
    private int damage;

    public int currentHealth;

    void Start () {
        // Find GameController Object.
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        // This is for reference in GameOver().
        if (gameControllerObject != null) {
            gameController = gameControllerObject.GetComponent<GameController>();
        } else {
            Debug.Log("Cannot find 'Game Controller' script");
        }

        // Find playerObject.
        playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null) {
            // Get the type of boltExplosion depending to the type of shot.
            playerBoltExplosion = playerObject.GetComponent<PlayerController>().shot.GetComponent<BoltMover>().boltExplosion;
        }
    }

    void OnEnable() {
        // Reset health and isDead bool.
        currentHealth = enemyHealth;
        isDead = false;

        if (gameObject.name == "Destructor(Clone)") {
            gameObject.GetComponent<DisableOnDeath>().Enable();
            gameObject.GetComponent<Mover>().StopMoving();
            gameObject.GetComponent<WeaponController>().enabled = true;
            gameObject.GetComponent<WeaponControllerMissile>().enabled = true;
            gameObject.GetComponent<EvasiveManeuver>().enabled = true;
            gameObject.GetComponent<TumbleWhenDead>().enabled = false;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Boundary")) {
            return;
        }
        // This else if needs to be in this order because both gameObjects have DestroyByContact class component
        else if (gameObject.CompareTag("Enemy") && other.CompareTag("EnemyBolt") ||
                 gameObject.CompareTag("EnemyBolt") && other.CompareTag("Enemy") ||
                 gameObject.CompareTag("EnemyBolt") && other.CompareTag("Boss") ||
                 gameObject.CompareTag("Boss") && other.CompareTag("EnemyBolt")) {
            return;
        } else if (gameObject.CompareTag("Enemy") && other.CompareTag("Enemy") ||
                   gameObject.CompareTag("Enemy") && other.CompareTag("Boss") ||
                   gameObject.CompareTag("Boss") && other.CompareTag("Enemy")) {
            return;
        } else if (gameObject.CompareTag("EnemyBolt") && other.CompareTag("EnemyBolt")) {
            return;
        } else if (gameObject.CompareTag("EnemyBolt") && other.CompareTag("PlayerBolt")) {
            return;
        } else if (gameObject.CompareTag("EnemyBolt") && other.CompareTag("Player")) {
            // If not null then call Explode() for effects.
            if (GetComponent<Scatter>() != null) {
                GetComponent<Scatter>().Explode();
            }
            
            if (GetComponent<BoltMover>() != null) {
                boltExplosion = GetComponent<BoltMover>().boltExplosion;
            } else if (GetComponent<FollowMovement>() != null) {
                boltExplosion = GetComponent<FollowMovement>().boltExplosion;
            } else if (GetComponent<VectorMovement>() != null) {
                boltExplosion = GetComponent<VectorMovement>().boltExplosion;
            } else if (GetComponent<MissileMover>() != null) {
                boltExplosion = GetComponent<MissileMover>().boltExplosion;
            } else {
                boltExplosion = null;
            }

            if (boltExplosion != null) {
                Instantiate(boltExplosion, transform.position, transform.rotation);
            }            

            // Get the int damage value by getting the reference from the script used.
            GetDamageValue();

            int playerHealth = other.GetComponent<PlayerController>().health;

            other.GetComponent<HealthBar>().TakeDamage(damage);
            other.GetComponent<PlayerController>().health -= damage;
            playerHealth -= damage;
                       
            // Destroy player if health is less than zero.
            if (playerHealth <= 0) {
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                Destroy(other.gameObject); // Destroy player gameObject if health is less than zero.
                gameController.GameOver();
            }
        } else if (gameObject.CompareTag("Enemy") && other.CompareTag("PlayerBolt")) {
            damage = other.GetComponent<BoltMover>().damage;

            currentHealth -= damage;

            // Instantiate a bolt Explosion for effects.
            Instantiate(playerBoltExplosion, other.transform.position, other.transform.rotation); 

            if (currentHealth <= 0) {
                if (gameObject.name == "Destructor(Clone)") {
                    if (!isDead) {                        
                        Instantiate(explosion, transform.position, transform.rotation);
                        // Destroy not needed because the ship will tumble until it falls outside the boundary and will automatically be deleted.
                        gameObject.GetComponent<WeaponController>().StopFiring(); // Stop all InvokeRepeating in the WeaponController (script).
                        gameObject.GetComponent<WeaponController>().enabled = false; 
                        gameObject.GetComponent<WeaponControllerMissile>().enabled = false;
                        gameObject.GetComponent<Mover>().StopMoving(); // Make the rigidbody velocity to Vector3.zero
                        gameObject.GetComponent<EvasiveManeuver>().StopAllCoroutines(); 
                        gameObject.GetComponent<EvasiveManeuver>().enabled = false;
                        gameObject.GetComponent<TumbleWhenDead>().enabled = true; 
                    }
                } else {
                    Instantiate(explosion, transform.position, transform.rotation);
                    RePool(gameObject); // Repool enemy gameObject if health is less than zero.                    
                }                
            }
        } else if (gameObject.CompareTag("Boss") && other.CompareTag("PlayerBolt")) {
            damage = other.GetComponent<BoltMover>().damage;

            gameObject.GetComponent<HealthBar>().TakeDamage(damage);
            currentHealth -= damage;

            Instantiate(playerBoltExplosion, other.transform.position, other.transform.rotation); // Instantiate a bolt Explosion for effects.

            if (currentHealth <= 0 && !isDead) {
                // The Boss explosion in death is handled by ExplodeInDeath (script).
                // Destroy not needed because the boss will tumble until it falls outside the boundary and will automatically be deleted.

                gameController.bossHealthBar.SetActive(false); // Deactivates the bossHealthBar.                
                gameObject.GetComponent<WeaponControllerPatterns>().StopFiring(); // Stops the current firing of bolts.
                gameObject.GetComponent<WeaponControllerPatterns>().enabled = false; 
                gameObject.GetComponent<ComplexEvasiveManeuver2>().StopMoving(); // Make the rigidbody velocity to Vector3.zero
                gameObject.GetComponent<ComplexEvasiveManeuver2>().StopAllCoroutines();
                gameObject.GetComponent<ComplexEvasiveManeuver2>().enabled = false;
                gameObject.GetComponent<ExplodeInDeath>().enabled = true;
                gameObject.GetComponent<TumbleWhenDead>().enabled = true;

                gameController.YouWin();
            }
        }

        if (gameObject.CompareTag("EnemyBolt") && boltExplosion != null) {
            RePool(gameObject);
        } else if (other.CompareTag("PlayerBolt")) {
            RePool(other.gameObject);
        }

        // Disables scripts that is needed to be disabled on death.
        // Make isDead = true, so other statements won't be read for efficiency.
        // Then adds score value.
        if (gameObject.CompareTag("Enemy") || gameObject.CompareTag("Boss") &&
            other.CompareTag("PlayerBolt") || other.CompareTag("Player")) {

            if (currentHealth <= 0 && !isDead) {
                if (GetComponent<DisableOnDeath>() != null) {
                    GetComponent<DisableOnDeath>().Disable();
                }
                isDead = true;
                gameController.AddScore(scoreValue); // Adds score according on enemy score value.
            }
        }        
    }

    void GetDamageValue() {
        if (gameObject.GetComponent<BoltMover>() != null) {
            damage = gameObject.GetComponent<BoltMover>().damage;
        } else if (gameObject.GetComponent<MissileMover>() != null) {
            damage = gameObject.GetComponent<MissileMover>().damage;
        } else if (gameObject.GetComponent<VectorMovement>() != null) {
            damage = gameObject.GetComponent<VectorMovement>().damage;
        } else if (gameObject.GetComponent<FollowMovement>() != null) {
            damage = gameObject.GetComponent<FollowMovement>().damage;
        }        
    }

    void RePool(GameObject thisGameObject) {
        thisGameObject.SetActive(false);        
    }
}
