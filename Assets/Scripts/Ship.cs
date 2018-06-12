using UnityEngine;

public abstract class Ship {

    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public float Speed { get; private set; }
    public float TiltStrength { get; private set; }
    public GameObject ShotGameObject { get; private set; }
    public Transform ShotSpawns { get; private set; }
    public float FireRate { get; set; }
    public GameObject Explosion { get; private set; }

    public Ship(int health, float speed, float tiltStrength, GameObject shotGameObject, 
                Transform shotSpawns, float fireRate, GameObject explosion) {
        Health = health;
        MaxHealth = Health;
        Speed = speed;
        TiltStrength = tiltStrength;
        ShotGameObject = shotGameObject;
        ShotSpawns = shotSpawns;
        FireRate = fireRate;
        Explosion = explosion;
    }

    public abstract void TakeDamage(int damage);
    public abstract void Death();

    public void SubtractHealth(int value) {
        Health -= value;
    }

    public bool IsAlive() {
        return Health > 0;
    }
}
