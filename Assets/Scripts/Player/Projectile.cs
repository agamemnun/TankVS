using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int baseDamage = 10;
    public int criticalDamage = 20;
    public float baseExplosionRadius = 2.0f;
    public float explosionRadius;
    // Start is called before the first frame update
    void Start()
    {
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

    public bool getPlayerLuck()
    {
        // TODO ileride playerdan alırız
        return Random.Range(0, 10) < 3;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        InflictAreaDamage(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), explosionRadius);
        Destroy(gameObject);
    }

    void InflictAreaDamage(Vector2 center, float radius)
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
                    bool isCriticalHit = false;

                    int damage = baseDamage;

                    if (effect >= 0.8f)
                    {
                        isCriticalHit = getPlayerLuck();

                        if (isCriticalHit)
                            damage = criticalDamage;
                    }

                    int damageInflicted = (int)(damage * effect);

                    if (damageInflicted <= 0)
                        continue;

                    healthComponent.takeDamage(damageInflicted, isCriticalHit);
                }
            }

        }
    }
}
