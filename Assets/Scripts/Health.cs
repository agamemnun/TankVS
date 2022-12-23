using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"current health: {@currentHealth}");
        if (currentHealth <= 0)
        {
            Debug.Log("You have won!");
            // dead
            // dead animation
            // show gameover screen
        }
    }
}
