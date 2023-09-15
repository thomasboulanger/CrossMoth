using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MothHealth : MonoBehaviour
{
    public float maxHealth = 4;
    public float currentHealth;

    private Slider slider;

    [SerializeField] private Volume volume;
    private Vignette vignette;

    [SerializeField] private AnimationCurve damageLerp;
    [SerializeField] private float speed = 1f;

    [SerializeField] private Vector2 minMaxIntensity;

    [SerializeField] private Color col1;
    [SerializeField] private Color col2;


    private float t;
    private bool animate;
    
    void Start()
    {

        if (volume == null)
        {
            volume = GameObject.Find("Global Volume").GetComponent<Volume>();
        }

        slider = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Slider>();
        currentHealth = maxHealth;
        slider.value = 1;

        Vignette temp;

        if (volume.profile.TryGet<Vignette>(out temp))
        {
            vignette = temp;
        }
    }

    void Update()
    {
        if (animate)
        {
            t += Time.deltaTime * speed;
            float alpha = damageLerp.Evaluate(t);
            vignette.intensity.value = Mathf.Lerp(minMaxIntensity.x, minMaxIntensity.y, alpha);
            vignette.color.value = Color.Lerp(col1, col2, alpha);
        }
    }

    public void TakeDamage(float dmg) {
        currentHealth -= dmg;
        slider.value = currentHealth / maxHealth;
        if (currentHealth <= 0) {
            Die();
        }
        animate = true;
        t = 0f;
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
