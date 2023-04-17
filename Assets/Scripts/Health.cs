using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBarBehaviour healthBar;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.setHealth(currentHealth, maxHealth);
    }

    void Update()
    {

    }

    public void takeDamage(int damageAmount, bool isCriticalHit)
    {
        currentHealth -= damageAmount;
        healthBar.setHealth(currentHealth, maxHealth);

        DamagePopup.Create(gameObject.transform.position, damageAmount, isCriticalHit);

        Debug.Log($"current health: {@currentHealth}");
        if (currentHealth <= 0)
        {
            Debug.Log("You have won!");
            Destroy(gameObject);
            // dead
            // dead animation
            // show gameover screen
        }
    }
}
