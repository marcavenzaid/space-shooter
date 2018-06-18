using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DestroyByContact))]
[RequireComponent(typeof(PowerUpDroper))]
public class Enemy : Ship {

    [SerializeField] private int score = 7;
    private Boss boss;
    private bool isBoss;
    private PowerUpDroper powerUpDroper;

    protected override void Awake() {
        base.Awake();
        if (gameObject.CompareTag("Boss")) {
            boss = GetComponent<Boss>();
            isBoss = true;
        }
        powerUpDroper = GetComponent<PowerUpDroper>();
    }    

    protected virtual void OnEnable() {
        Health = MaxHealth;
    }

    protected PowerUpDroper PowerUpDroper {
        get { return powerUpDroper; }
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
        if (isBoss) {
            boss.DeathBehavior();
        }
    }

    public int GetScore() {
        return score;
    }    

    protected void MoveForward() {
        Rb.velocity = -transform.forward * VerticalSpeed;
    }

    protected void StopMovement() {
        Rb.velocity = Vector3.zero;
    }

    protected void ClampXPosition() {
        Rb.position = new Vector3(Mathf.Clamp(Rb.position.x, Bounds.XMin, Bounds.XMax), 0, Rb.position.z);
    }    
}
