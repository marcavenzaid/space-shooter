using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DestroyByContact))]
[RequireComponent(typeof(PowerUpDroper))]
public class Enemy : Ship {

    [SerializeField] private int score;
    private Boss boss;
    private DestructorEnemy destructorEnemy;
    private bool isBoss;
    private bool isDestructorEnemy;
    private PowerUpDroper powerUpDroper;

    protected override void Awake() {
        base.Awake();
    }

    private void OnEnable() {
        Health = MaxHealth;
    }

    private void Start() {
        if (gameObject.CompareTag("Boss")) {
            boss = GetComponent<Boss>();
            isBoss = true;
        } else if (gameObject.name == "Destructor") {
            destructorEnemy = GetComponent<DestructorEnemy>();
            isDestructorEnemy = true;
        }
        powerUpDroper = GetComponent<PowerUpDroper>();
    }

    protected override void Fire() {

    }

    public override void TakeDamage(int damage) {
        if (IsAlive()) {
            base.TakeDamage(damage);
            if (!IsAlive()) {
                Death();                
            }
        }
    }
    
    protected override void Death() {
        base.Death();
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

    public int GetScore() {
        return score;
    }

    private void PoolObject(GameObject thisGameObject) {
        thisGameObject.SetActive(false);
    }
}
