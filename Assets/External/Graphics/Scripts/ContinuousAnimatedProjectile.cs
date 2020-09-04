using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace danielnyan
{
    public class ContinuousAnimatedProjectile : ContinuousProjectile
    {
        private Animator animator;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Finished"))
            {
                Destroy(gameObject);
            }
        }

        protected override IEnumerator killProjectile()
        {
            if (killTime == 0f)
            {
                animator.SetTrigger("Interrupt");
                yield break;
            }
            else
            {
                base.killProjectile();
            }
        }
    }
}
