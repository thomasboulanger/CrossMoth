using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 4;
    [SerializeField] private float currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        
    }

    public void TakeDamage(float dmg) {
        currentHealth -= dmg;
        if (currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        Destroy(gameObject);
    }
}
