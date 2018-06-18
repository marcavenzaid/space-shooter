using UnityEngine;

public abstract class Ship : MonoBehaviour {

    [SerializeField] private Boundary bounds = new Boundary(-8, 8, -15, 25);
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private float horizontalSpeed = 2.5f;
    [SerializeField] private float verticalSpeed = 2.5f;
    [SerializeField] private float tiltStrength = 4;
    [SerializeField] private GameObject shotGameObject;    
    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private Vector2 firstAttackDelay = new Vector2(1, 1);
    [SerializeField] private GameObject explosion;
    private int health;
    private AudioSource weaponAudioSource;
    private Rigidbody rb;

    public Boundary Bounds {
        get { return bounds; }
        set { bounds = value; }
    }

    public int MaxHealth {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    public int Health {
        get { return health; }
        set { health = value; }
    }

    public float HorizontalSpeed {
        get { return horizontalSpeed; }
        private set { horizontalSpeed = value; }
    }

    public float VerticalSpeed {
        get { return verticalSpeed; }
        private set { verticalSpeed = value; }
    }    

    public float TiltStrength {
        get { return tiltStrength; }
    }

    public GameObject ShotGameObject {
        get { return shotGameObject; }
        set { shotGameObject = value; }
    }

    protected float FireRate {
        get { return fireRate; }
        set { fireRate = value; }
    }

    public float FirstAttackDelay {
        get { return Random.Range(firstAttackDelay.x, firstAttackDelay.y); }
    }

    protected GameObject Explosion {
        get { return explosion; }
        set { explosion = value; }
    }

    public AudioSource WeaponAudioSource {
        get { return weaponAudioSource; }
        private set { weaponAudioSource = value; }
    }

    protected Rigidbody Rb {
        get { return rb; }
        private set { rb = value; }
    }

    protected virtual void Awake() {   
        if(MaxHealth <= 0) {
            MaxHealth = 1;
        }
        Health = MaxHealth;
        WeaponAudioSource = GetComponent<AudioSource>();
        Rb = GetComponent<Rigidbody>();
    }    

    public bool IsAlive() {
        return Health > 0;
    }

    public virtual void TakeDamage(int damage) {
        Health -= damage;
    }

    protected virtual void Death() {
        Instantiate(Explosion, transform.position, transform.rotation);
        PoolObject();
    }    

    protected void PoolObject() {
        gameObject.SetActive(false);
    }
}
