using System.Collections;
using UnityEngine;

namespace danielnyan
{
    public class ContinuousRigidbodyProjectile : ContinuousProjectile
    {
        [SerializeField]
        private bool freezeRigidbody = true;

        public override void KillProjectile()
        {
            if (freezeRigidbody)
            {
                Debug.Log("Freezing Projectile");
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            }
            base.KillProjectile();
        }
    }
}
