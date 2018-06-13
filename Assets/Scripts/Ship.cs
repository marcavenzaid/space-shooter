using UnityEngine;

public abstract class Ship : MonoBehaviour {

    [SerializeField] private Boundary bounds = new Boundary(-8, 8, -7, 12);
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float speed = 10;
    [SerializeField] private float tiltStrength = 4;
    [SerializeField] private GameObject shotGameObject;
    [SerializeField] private Transform shotSpawns;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private GameObject explosion;
    private int health;
    private AudioSource weaponAudioSource;

    public Boundary Bounds {
        get { return bounds; }
        private set { bounds = value; }
    }

    public int MaxHealth {
        get { return maxHealth; }
        private set { maxHealth = value; }
    }

    public int Health {
        get { return health; }
        set { health = value; }
    }

    public float Speed {
        get { return speed; }
        private set { speed = value; }
    }

    public float TiltStrength {
        get { return tiltStrength; }
        set { tiltStrength = value; }
    }

    public GameObject ShotGameObject {
        get { return shotGameObject; }
        set { shotGameObject = value; }
    }

    protected Transform ShotSpawns {
        get { return shotSpawns; }
        set { shotSpawns = value; }
    }

    protected float FireRate {
        get { return fireRate; }
        set { fireRate = value; }
    }

    protected GameObject Explosion {
        get { return explosion; }
        set { explosion = value; }
    }

    protected AudioSource WeaponAudioSource {
        get { return weaponAudioSource; }
    }

    protected virtual void Awake() {
        MaxHealth = Health = maxHealth;
        Speed = speed;
        TiltStrength = tiltStrength;
        ShotGameObject = shotGameObject;
        ShotSpawns = shotSpawns;
        FireRate = fireRate;
        Explosion = explosion;
        Bounds = bounds;
        weaponAudioSource = GetComponent<AudioSource>();
    }

    protected abstract void Fire();

    public virtual void TakeDamage(int damage) {
        Health -= damage;
    }

    protected virtual void Death() {
        Instantiate(Explosion, transform.position, transform.rotation);
    }

    public void SubtractHealth(int value) {
        Health -= value;
    }

    public bool IsAlive() {
        return Health > 0;
    }
}
