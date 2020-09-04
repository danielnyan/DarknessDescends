using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace danielnyan
{
    public class ContinuousParticleProjectile : ContinuousProjectile
    {
        private ParticleSystem ps;
        private ParticleSystem.MainModule main;
        private Collider2D collider2d;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            ps = GetComponentInChildren<ParticleSystem>();
            collider2d = GetComponentInChildren<Collider2D>();
            collider2d.enabled = true;
            main = ps.main;
            main.loop = true;
        }

        protected override IEnumerator killProjectile()
        {
            if (killTime == 0f)
            {
                main.loop = false;
                collider2d.enabled = false;
                while (ps.particleCount > 0f)
                {
                    yield return new WaitForFixedUpdate();
                }
                Destroy(gameObject);
            }
            else
            {
                base.killProjectile();
            }
        }
    }
}
