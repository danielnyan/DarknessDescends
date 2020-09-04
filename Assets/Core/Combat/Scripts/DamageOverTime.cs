using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDG
{
    [RequireComponent(typeof(Collider2D))]
    public class DamageOverTime : Damager
    {
        public float tick;
        private Dictionary<int, float> attacked = new Dictionary<int, float>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IDamagee damagee = collision.gameObject.GetComponent<IDamagee>();
            if (damagee != null)
            {
                int instanceId = collision.gameObject.GetInstanceID();
                if (!attacked.ContainsKey(instanceId))
                {
                    attacked.Add(instanceId, tick);
                    ApplyDamage(damagee, collision);
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            int instanceId = collision.gameObject.GetInstanceID();
            if (!attacked.ContainsKey(instanceId))
            {
                IDamagee damagee = collision.gameObject.GetComponent<IDamagee>();
                if (damagee != null)
                {
                    attacked.Add(instanceId, tick);
                    ApplyDamage(damagee, collision);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            int instanceId = collision.gameObject.GetInstanceID();
            if (attacked.ContainsKey(instanceId))
            {
                attacked.Remove(instanceId);
            }
        }

        private void FixedUpdate()
        {
            List<int> keys = new List<int>();
            foreach (int instanceId in attacked.Keys)
            {
                keys.Add(instanceId);
            }
            foreach (int instanceId in keys)
            {
                attacked[instanceId] -= Time.fixedDeltaTime;
                if (attacked[instanceId] < 0)
                {
                    attacked.Remove(instanceId);
                }
            }
        }
    }
}
