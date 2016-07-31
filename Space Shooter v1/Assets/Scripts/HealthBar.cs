using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Text healthDigits;
    public Slider healthSlider;
    public Image healthColor;

    public Color lowColor;
    public Color highColor;

    private float currentHealth;

    void Start() {
        if (gameObject.CompareTag("Player")) {
            healthDigits.text = GetComponent<PlayerController>().health + " / " + GetComponent<PlayerController>().maxHealth;
        }
    }

    public void TakeDamage(int damage) {
        // if else statement is for getting the reference the health of the Player or the Boss.
        if (gameObject.CompareTag("Player")) {
            currentHealth = GetComponent<PlayerController>().health; // The script where the player health is located.
        }
        else {
            GameObject bossHealthSlider = GameObject.FindWithTag("BossHealthSlider"); // Find the Boss Slider in the Hierarchy.
            if (bossHealthSlider != null) {
                healthSlider = bossHealthSlider.GetComponent<Slider>();
            }
            currentHealth = GetComponent<DestroyByContact>().currentHealth; // The script where the enemy health is located.

            GameObject bossHealthColor = GameObject.FindWithTag("BossHealthColor"); // Find the Boss Fill Color in the Hierarchy.
            if (bossHealthColor != null) {
                healthColor = bossHealthColor.GetComponent<Image>();
            }
        }        

        float damagedHealth = currentHealth - damage;
        healthSlider.value *= damagedHealth / currentHealth; // Divide it to get the ratio.
        healthColor.color = Color.Lerp(lowColor, highColor, healthSlider.value); // Lerps the color of health depending on value.

        // Update Health Digits.
        if (gameObject.CompareTag("Player")) {
            healthDigits.text = damagedHealth + " / " + GetComponent<PlayerController>().maxHealth;
        }        
    }
}
