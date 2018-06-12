using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DestroyByContact))]
[RequireComponent(typeof(PowerUpDroper))]
public class Enemy : MonoBehaviour {

    [SerializeField] private GameObject explosion;
    [SerializeField] private int health;
    private int maxHealth;
    [SerializeField] private int score;
    private Boss boss;
    private DestructorEnemy destructorEnemy;
    private bool isBoss;
    private bool isDestructorEnemy;
    private PowerUpDroper powerUpDroper;

    private void Awake() {
        maxHealth = health;
    }

    private void OnEnable() {
        health = maxHealth;
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
        return health;
    }

    public int GetMaxHealth() {
        return maxHealth;
    }

    public int GetScore() {
        return score;
    }

    public bool IsAlive() {
        return GetHealth() > 0;
    }

    public void TakeDamage(int damage) {
        if (IsAlive()) {
            health -= damage;
            if (!IsAlive()) {
                Death();                
            }
        }
    }
    
    private void Death() {
        Instantiate(explosion, transform.position, transform.rotation);
        powerUpDroper.DropPowerUps();
        if (isDestructorEnemy) {
            destructorEnemy.DeathBehavior();
        } else if (isBoss) {
            boss.DeathBehavior();            
        } else {
            PoolObject(gameObject);
        }

        if (GetComponent<DisableOnDeath>() != null) {
            GetComponent<DisableOnDeath>().Disable();
        }
    }

    private void PoolObject(GameObject thisGameObject) {
        thisGameObject.SetActive(false);
    }
}
