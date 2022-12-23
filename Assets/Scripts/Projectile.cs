using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int baseDamage = 10;
    public int damage;
    public float baseExplosionRadius = 2.0f;
    public float explosionRadius;
    // Start is called before the first frame update
    void Start()
    {
        damage = baseDamage;
        explosionRadius = baseExplosionRadius;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        // Direkt çarpışmada damage veren kod
        /* if (other.tag == "Player")
        {
            var healthComponent = other.GetComponent<Health>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damage);
            }
        } */


    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        AreaDamageEnemies(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), explosionRadius, damage);
        Destroy(gameObject);
    }

    void AreaDamageEnemies(Vector2 center, float radius, float damage)
    {
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(center, radius);

        foreach (Collider2D col in objectsInRange)
        {
            if (col.tag == "Player")
            {
                var healthComponent = col.GetComponent<Health>();

                if (healthComponent != null)
                {
                    float proximity = (center - new Vector2(healthComponent.transform.position.x, healthComponent.transform.position.y)).magnitude;
                    float effect = 1 - (proximity / radius);

                    healthComponent.TakeDamage((int)(damage * effect));
                }
            }

        }
    }
}
