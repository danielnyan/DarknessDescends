using UnityEngine;
using GDG;

namespace danielnyan
{
    public class SplashProjectile : MonoBehaviour
    {
        public GameObject instantiatedObject;
        public AllyType damageSource;
        public bool killParent;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                Instantiate(instantiatedObject, transform.position, Quaternion.identity);
                if (killParent)
                {
                    ContinuousRigidbodyProjectile k = transform.root.GetComponent<ContinuousRigidbodyProjectile>();
                    if (k != null)
                    {
                        k.KillProjectile();
                    }
                }
                Destroy(gameObject);
                return;
            }

            IDamagee damagee = collision.GetComponent<IDamagee>();
            if (damagee == null ||
                damagee.GetAllyType() == damageSource || // friendly
                !damagee.IsVulnerableToDamage())         // immune to damage
            {
                return;
            }
            Instantiate(instantiatedObject, transform.position, Quaternion.identity);
            if (killParent)
            {
                ContinuousRigidbodyProjectile k = transform.root.GetComponent<ContinuousRigidbodyProjectile>();
                if (k != null)
                {
                    k.KillProjectile();
                }
            }
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Instantiate(instantiatedObject, transform.position, Quaternion.identity);
            if (killParent)
            {
                ContinuousRigidbodyProjectile k = transform.root.GetComponent<ContinuousRigidbodyProjectile>();
                if (k != null)
                {
                    k.KillProjectile();
                }
            }
            Destroy(gameObject);
        }
    }
}
