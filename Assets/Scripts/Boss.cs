using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(WeaponControllerPatterns))]
[RequireComponent(typeof(ComplexEvasiveManeuver2))]
[RequireComponent(typeof(ExplodeInDeath))]
[RequireComponent(typeof(Tumble))]
public class Boss : MonoBehaviour {
    
    private Slider healthSlider;
    private Image healthColor;
    private Color lowHealthColor = new Color(1f, 0, 0, 0.5f);
    private Color highHealthColor = new Color(0, 1f, 0, 0.5f);
    private GameController gameController;
    private int maxHealth;

    private void Start() {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        gameController.SetBossHeathBarActive(true);
        healthSlider = GameObject.FindWithTag("BossHealthSlider").GetComponent<Slider>();
        healthColor = GameObject.FindWithTag("BossHealthColor").GetComponent<Image>();                
        maxHealth = GetComponent<Enemy>().GetMaxHealth();
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        gameObject.GetComponent<WeaponControllerPatterns>().enabled = true;
        gameObject.GetComponent<ComplexEvasiveManeuver2>().enabled = true;
        gameObject.GetComponent<ExplodeInDeath>().enabled = false;
        gameObject.GetComponent<Tumble>().enabled = false;
    }    

    public void DecreaseHealthBar(int damage) {
        healthSlider.value -= damage;
        healthColor.color = Color.Lerp(lowHealthColor, highHealthColor, healthSlider.value / maxHealth);
    }

    public void DeathBehavior() {        
        gameObject.GetComponent<WeaponControllerPatterns>().StopFiring();
        gameObject.GetComponent<WeaponControllerPatterns>().enabled = false;
        gameObject.GetComponent<ComplexEvasiveManeuver2>().StopAllCoroutines();
        gameObject.GetComponent<ComplexEvasiveManeuver2>().enabled = false;
        gameObject.GetComponent<ExplodeInDeath>().enabled = true;
        gameObject.GetComponent<Tumble>().enabled = true;
        gameController.SetBossHeathBarActive(false);
    }
}
