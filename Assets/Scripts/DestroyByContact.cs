using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Enemy))]
public class DestroyByContact : MonoBehaviour {

    public GameObject explosion; //d
    public GameObject playerExplosion; //d, put this on player controller, or new script
    private GameController gameController;
    private GameObject playerObject;
    private GameObject playerBoltExplosion;
    private GameObject boltExplosion;
    private int damage;    // ??
    private Enemy enemy;

    void Start () {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");

        if (gameControllerObject != null) {
            gameController = gameControllerObject.GetComponent<GameController>();
        } else {
            Debug.Log("Cannot find 'Game Controller' script");
        }

        playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null) {
            playerBoltExplosion = playerObject.GetComponent<PlayerController>().GetShotGameObject().GetComponent<BoltMover>().boltExplosion;
        }

        enemy = GetComponent<Enemy>();
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

            int playerHealth = other.GetComponent<PlayerController>().GetHealth();

            other.GetComponent<HealthBar>().TakeDamage(damage);
            other.GetComponent<PlayerController>().SubtractHealth(damage);
            playerHealth -= damage;
                       
            // Destroy player if health is less than zero.
            if (playerHealth <= 0) {
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                Destroy(other.gameObject); // Destroy player gameObject if health is less than zero.
                gameController.GameOver();
            }
        } else if (gameObject.CompareTag("Enemy") && other.CompareTag("PlayerBolt")) {
            damage = other.GetComponent<BoltMover>().damage;
            enemy.TakeDamage(damage);
            Instantiate(playerBoltExplosion, other.transform.position, other.transform.rotation);             
        } else if (gameObject.CompareTag("Boss") && other.CompareTag("PlayerBolt")) {
            damage = other.GetComponent<BoltMover>().damage;
            enemy.TakeDamage(damage);
            gameObject.GetComponent<Boss>().DecreaseHealthBar(damage);
            Instantiate(playerBoltExplosion, other.transform.position, other.transform.rotation);
        }

        if (gameObject.CompareTag("EnemyBolt") && boltExplosion != null) {
            RePool(gameObject);
        } else if (other.CompareTag("PlayerBolt")) {
            RePool(other.gameObject);
        }

        if ((gameObject.CompareTag("Enemy") || gameObject.CompareTag("Boss")) 
            && (other.CompareTag("PlayerBolt") || other.CompareTag("Player"))) {
            if (enemy.IsAlive()) {                
                gameController.AddScore(enemy.GetScore());
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
