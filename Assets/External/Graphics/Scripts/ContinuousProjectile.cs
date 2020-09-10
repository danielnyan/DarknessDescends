using System.Collections;
using UnityEngine;

namespace danielnyan
{
    public class ContinuousProjectile : MonoBehaviour
    {
        [SerializeField]
        protected float killTime;

        private bool killing = false;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            killing = false;
        }

        public virtual void KillProjectile()
        {
            if (!killing)
            {
                StartCoroutine(killProjectile());
                killing = true;
            }
        }

        protected virtual IEnumerator killProjectile()
        {
            if (killTime > 0f)
            {
                yield return new WaitForSeconds(killTime);
            }
            Destroy(gameObject);
        }
    }
}
