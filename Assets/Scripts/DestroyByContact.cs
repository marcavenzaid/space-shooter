using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Enemy))]
public class DestroyByContact : MonoBehaviour {

    public GameObject playerExplosion; //d, put this on player controller, or new script
    private GameController gameController;
    private GameObject playerObject;
    private GameObject playerBoltExplosion;
    private GameObject shotExplosion;
    private int damage;    // ??
    private Enemy enemy;

    private void Start () {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");

        if (gameControllerObject != null) {
            gameController = gameControllerObject.GetComponent<GameController>();
        } else {
            Debug.Log("Cannot find 'Game Controller' script");
        }

        playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null) {
            playerBoltExplosion = playerObject.GetComponent<Player>().GetShotGameObject().GetComponent<BoltMover>().boltExplosion;
        }

        enemy = GetComponent<Enemy>();
    }    

    private void OnTriggerEnter(Collider other) {
        if(IsToIgnore(other)) {
            return;
        }

        GetShotProperties();

        if (gameObject.CompareTag("EnemyBolt") && other.CompareTag("Player")) {
            if (GetComponent<Scatter>() != null) {
                GetComponent<Scatter>().Explode();
            }

            if (shotExplosion != null) {
                Instantiate(shotExplosion, transform.position, transform.rotation);
            }                        

            int playerHealth = other.GetComponent<Player>().GetHealth();
            other.GetComponent<HealthBar>().TakeDamage(damage);
            other.GetComponent<Player>().SubtractHealth(damage);
            playerHealth -= damage;                       
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

        if (gameObject.CompareTag("EnemyBolt") && shotExplosion != null) {
            RePool(gameObject);
        } else if (other.CompareTag("PlayerBolt")) {
            RePool(other.gameObject);
        }

        //if ((gameObject.CompareTag("Enemy") || gameObject.CompareTag("Boss")) 
        //    && (other.CompareTag("PlayerBolt") || other.CompareTag("Player"))) {
        //    if (enemy.IsAlive()) {                
        //        gameController.AddScore(enemy.GetScore());
        //    }
        //}        
    }

    private bool IsToIgnore(Collider other) {
        if (other.CompareTag("Boundary")) {
            return true;
        } else if (gameObject.CompareTag("Enemy") && other.CompareTag("EnemyBolt") ||
                   gameObject.CompareTag("EnemyBolt") && other.CompareTag("Enemy") ||
                   gameObject.CompareTag("EnemyBolt") && other.CompareTag("Boss") ||
                   gameObject.CompareTag("Boss") && other.CompareTag("EnemyBolt")) {
            // This else if needs to be in this order because both gameObjects have DestroyByContact class component
            return true;
        } else if (gameObject.CompareTag("EnemyBolt") && other.CompareTag("EnemyBolt")) {
            return true;
        } else if (gameObject.CompareTag("EnemyBolt") && other.CompareTag("PlayerBolt")) {
            return true;
        }
        return false;
    }

    private void GetShotProperties() {        
        if (gameObject.name == "BoltBlue(Clone)" || gameObject.name == "BoltRed(Clone)" || gameObject.name == "OrbBlue(Clone)"
                || gameObject.name == "OrbOrange(Clone)" || gameObject.name == "OrbPink(Clone)" || gameObject.name == "Bomd(Clone)"
                || gameObject.name == "BombConstant(Clone)" || gameObject.name == "Fragments(Clone)") {
            damage = gameObject.GetComponent<BoltMover>().damage;
            shotExplosion = GetComponent<BoltMover>().boltExplosion;
        } else if (gameObject.name == "Missile(Clone)") {
            damage = gameObject.GetComponent<MissileMover>().damage;
            shotExplosion = gameObject.GetComponent<MissileMover>().boltExplosion;
        } else if (gameObject.name == "VectorOrb(Clone)") {
            damage = gameObject.GetComponent<VectorMovement>().damage;
            shotExplosion = GetComponent<VectorMovement>().boltExplosion;
        } else if (gameObject.name == "SensoredOrb(Clone)") {
            damage = gameObject.GetComponent<FollowMovement>().damage;
            shotExplosion = GetComponent<FollowMovement>().boltExplosion;
        }
    }
    
    private void RePool(GameObject thisGameObject) {
        thisGameObject.SetActive(false);        
    }    
}
