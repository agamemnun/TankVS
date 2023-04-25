using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTerrain
{

    public class Projectile : MonoBehaviour
    {
        public int baseDamage = 10;
        public int criticalDamage = 20;
        public int baseExplosionRadius = 2;
        public int explosionRadius;
        public int destructionRadius;

        protected Shape destroyCircle;

        private BasicPaintableLayer primaryLayer;
        private BasicPaintableLayer secondaryLayer;

        void Start()
        {
            GameObject destructor = GameObject.FindGameObjectWithTag("Destructor");
            if (destructor)
            {
                ClickAndDestroyOptimized destroyScript = destructor.GetComponent<ClickAndDestroyOptimized>();
                if (destroyScript)
                {
                    primaryLayer = destroyScript.primaryLayer;
                    secondaryLayer = destroyScript.secondaryLayer;
                }
            }

            explosionRadius = baseExplosionRadius;
            destructionRadius = explosionRadius * 8;

            destroyCircle = Shape.GenerateShapeCircle(destructionRadius);
        }

        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D other)
        {

            // Yedek - Direkt çarpışmada damage veren kod
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
            DestroyTerrain(gameObject.transform.position, destructionRadius);
            InflictAreaDamage(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), explosionRadius);
            Destroy(gameObject);
        }

        protected virtual void DestroyTerrain(Vector3 position, int r)
        {
            Debug.Log($"projectile position on contact: {position}");
            Vector3 p = position - primaryLayer.transform.position;

            primaryLayer?.Paint(new PaintingParameters()
            {
                Color = Color.clear,
                Position = new Vector2Int((int)(p.x * primaryLayer.PPU) - r, (int)(p.y * primaryLayer.PPU) - r),
                Shape = destroyCircle,
                PaintingMode = PaintingMode.REPLACE_COLOR,
                DestructionMode = DestructionMode.DESTROY
            });

            secondaryLayer?.Paint(new PaintingParameters()
            {
                Color = Color.clear,
                Position = new Vector2Int((int)(p.x * secondaryLayer.PPU) - r, (int)(p.y * secondaryLayer.PPU) - r),
                Shape = destroyCircle,
                PaintingMode = PaintingMode.REPLACE_COLOR,
                DestructionMode = DestructionMode.NONE
            });
        }

        void InflictAreaDamage(Vector2 center, float radius)
        {
            Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(center, radius);

            foreach (Collider2D col in objectsInRange)
            {
                if (col.CompareTag("Player"))
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
}