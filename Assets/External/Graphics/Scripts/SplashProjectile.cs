using UnityEngine;
using GDG;

namespace danielnyan
{
    public class SplashProjectile : MonoBehaviour
    {
        public GameObject instantiatedObject;
        public AllyType damageSource;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                Instantiate(instantiatedObject, transform.position, Quaternion.identity);
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
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Instantiate(instantiatedObject, transform.position, Quaternion.identity);
            Destroy(transform.root.gameObject);
        }
    }
}
