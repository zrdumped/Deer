using UnityEngine;
using System.Collections;


namespace MalbersAnimations
{
    /// <summary>
    /// Simple Script to make damage to every Animal
    /// </summary>
    public class MakeDamage : MonoBehaviour
    {
        public float damageMultiplier = 1;

        private Animal myAnimal;
        private Collider _collider;

        public Collider Collider
        {
            get
            {
                if (!_collider)
                {
                    _collider = GetComponent<Collider>(); ;
                }
                return _collider;
            }
        }

        void Start()
        {
            myAnimal = transform.GetComponentInParent<Animal>();                //Get the Animal Component on the Root of this Animal
            if (Collider)
            {
                Collider.isTrigger = true;
               // Collider.enabled = false;
            }
            else
            {
                Debug.LogWarning(name + " needs a Collider so 'AttackTrigger' can function correctly");
            }
        }
        void OnTriggerEnter(Collider other)
        {
            if (other.transform.root == transform.root) return;                      //Don't hit yourself

            Vector3 direction = -other.bounds.center + GetComponent<Collider>().bounds.center;

            DamageValues DV = new DamageValues(direction, damageMultiplier * (myAnimal ? myAnimal.attackStrength : 1));

            if (other.isTrigger) return; // just collapse when is a collider what we are hitting

            if (myAnimal)
            {
                if (myAnimal.IsAttacking)
                {
                    myAnimal.IsAttacking = false;

                    other.transform.SendMessageUpwards("getDamaged", DV, SendMessageOptions.DontRequireReceiver);
                }
            }
            else
            {
                other.transform.SendMessageUpwards("getDamaged", DV, SendMessageOptions.DontRequireReceiver);
            }
        }
    }


}
