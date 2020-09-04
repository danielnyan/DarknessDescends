using System.Collections;
using UnityEngine;

namespace danielnyan
{
    public class ProjectileLifetime : MonoBehaviour
    {
        [SerializeField]
        private float killTime;

        private void Start()
        {
            StartCoroutine(KillProjectile());
        }

        private IEnumerator KillProjectile()
        {
            yield return new WaitForSeconds(killTime);
            Destroy(gameObject);
            yield break;
        }
    }
}
