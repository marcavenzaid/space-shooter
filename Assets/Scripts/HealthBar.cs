using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Player))]
public class HealthBar : MonoBehaviour {

    [SerializeField] private Text healthDigits;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image healthColor;
    [SerializeField] private Color lowHealthColor = new Color(1f, 0, 0, 0.5f);
    [SerializeField] private Color highHealthColor = new Color(0, 1f, 0, 0.5f);
    [SerializeField] private Image damageFlashImage;
    [SerializeField] private float damageFlashSpeed = 5f;
    [SerializeField] private Color damageFlashColour = new Color(1f, 0f, 0f, 0.2f);
    private Player player;
    private int maxHealth;
    private bool damaged;

    void Start() {
        player = GetComponent<Player>();
        maxHealth = player.MaxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        healthDigits.text = player.Health + " / " + maxHealth;  
    }

    void Update() {
        FlashScreenOnDamage();
    }

    private void FlashScreenOnDamage() {
        if (damaged) {
            damageFlashImage.color = damageFlashColour;
            player.ResetUpgrades();
        } else {
            damageFlashImage.color = Color.Lerp(damageFlashImage.color, Color.clear, damageFlashSpeed * Time.deltaTime);
        }
        damaged = false;
    }

    public void SetHealth(int health) {
        damaged = true;
        healthSlider.value = health;
        healthColor.color = Color.Lerp(lowHealthColor, highHealthColor, healthSlider.value / maxHealth);
        healthDigits.text = health + " / " + maxHealth;
    }
}
