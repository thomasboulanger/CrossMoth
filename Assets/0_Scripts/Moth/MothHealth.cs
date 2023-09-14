using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MothHealth : MonoBehaviour
{
    public float maxHealth = 4;
    public float currentHealth;

    private Slider slider;
    
    void Start()
    {
        slider = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Slider>();
        currentHealth = maxHealth;
        slider.value = 1;
    }

    void Update()
    {
        
    }

    public void TakeDamage(float dmg) {
        currentHealth -= dmg;
        slider.value = currentHealth / maxHealth;
        if (currentHealth <= 0) {
            Die();
        }
    }

    public void Heal(float heal) {
        currentHealth += heal;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        slider.value = currentHealth / maxHealth;
    }

    void Die() {
        Destroy(gameObject);
        SceneManager.LoadScene("MainMenu");
    }
}
