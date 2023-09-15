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

    [SerializeField] private Color colDmg1;
    [SerializeField] private Color colDmg2;

    [SerializeField] private Color colHeal1;
    [SerializeField] private Color colHeal2;

    private float tDmg;
    private bool animateDmg;

    private float tHeal;
    private bool animateHeal;

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
        if (animateDmg)
        {
            tDmg += Time.deltaTime * speed;
            float alpha = damageLerp.Evaluate(tDmg);
            vignette.intensity.value = Mathf.Lerp(minMaxIntensity.x, minMaxIntensity.y, alpha);
            vignette.color.value = Color.Lerp(colDmg1, colDmg2, alpha);
            if (tDmg > 1) {
                animateDmg = false;
            }
        }
        
        if (animateHeal) {
            tHeal += Time.deltaTime * speed;
            float alpha = damageLerp.Evaluate(tHeal);
            vignette.intensity.value = Mathf.Lerp(minMaxIntensity.x, minMaxIntensity.y, alpha);
            vignette.color.value = Color.Lerp(colHeal1, colHeal2, alpha);
            if (tHeal > 1) {
                animateHeal = false;
            }
        }
    }

    public void TakeDamage(float dmg) {
        currentHealth -= dmg;
        slider.value = currentHealth / maxHealth;
        if (currentHealth <= 0) {
            Die();
        }
        animateDmg = true;
        tDmg = 0f;
    }

    public void Heal(float heal) {
        currentHealth += heal;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        slider.value = currentHealth / maxHealth;
        animateHeal = true;
        tHeal = 0f;
    }

    void Die() {
        SceneManager.LoadScene("GameOverScene");
    }
}
