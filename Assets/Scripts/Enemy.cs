using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DestroyByContact))]
[RequireComponent(typeof(PowerUpDroper))]
public class Enemy : MonoBehaviour {

    [SerializeField] private GameObject explosion;
    [SerializeField] private int maxHealth;
    private int currentHealth;
    [SerializeField] private int score;
    private Boss boss;
    private DestructorEnemy destructorEnemy;
    private bool isBoss;
    private bool isDestructorEnemy;
    private PowerUpDroper powerUpDroper;
    private bool isAlive;

    private void OnEnable() {
        currentHealth = maxHealth;
        isAlive = true;
    }

    private void Start () {
        if (gameObject.CompareTag("Boss")) {
            boss = GetComponent<Boss>();
            isBoss = true;
        } else if (gameObject.name == "Destructor") {
            destructorEnemy = GetComponent<DestructorEnemy>();
            isDestructorEnemy = true;
        }
        powerUpDroper = GetComponent<PowerUpDroper>();
    }    

    public int GetHealth() {
        return currentHealth;
    }

    public int GetMaxHealth() {
        return maxHealth;
    }

    public int GetScore() {
        return score;
    }

    public bool IsAlive() {
        return isAlive;
    }

    public void TakeDamage(int damage) {
        if(!IsAlive()) {
            return;
        }
        currentHealth -= damage;
        if (currentHealth <= 0) {
            DestroyEnemy();
            if (GetComponent<DisableOnDeath>() != null) {
                GetComponent<DisableOnDeath>().Disable();
            }
            isAlive = false;
        }
    }

    private void DestroyEnemy() {
        Instantiate(explosion, transform.position, transform.rotation);
        powerUpDroper.DropPowerUps();
        if (isDestructorEnemy) {
            destructorEnemy.DeathBehavior();
        } else if (isBoss) {            
            boss.DeathBehavior();            
        } else {
            PoolObject(gameObject);
        }        
    }

    private void PoolObject(GameObject thisGameObject) {
        thisGameObject.SetActive(false);
    }
}
