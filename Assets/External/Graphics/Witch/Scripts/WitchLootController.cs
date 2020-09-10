using System.Collections.Generic;
using UnityEngine;
using GDG;

namespace danielnyan
{
    // Credits: danielnyan
    [RequireComponent(typeof(IHero))]
    public class WitchLootController : MonoBehaviour
    {
        private IHero hero;

        void Start()
        {
            hero = GetComponent<IHero>();
            var boxCollider = gameObject.AddComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Item")
            {
                GameObject go = other.gameObject;
                DropBase drop = go.GetComponent<DropBase>();
                if (drop == null)
                {
                    return;
                }
                else if (drop.itemDrop.PickUpWhenInRange())
                {
                    takeItem(drop);
                }
            }
        }

        private void takeItem(DropBase drop)
        {
            IItem item = drop.TakeItem();
            item.OnPickUp(hero);
            SoundManager.Instance.PlayPickup();
        }
    }
}
