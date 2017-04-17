using UnityEngine;
using System.Collections;

namespace TopDown
{
    public class DamageTrigger : MonoBehaviour
    {

        [SerializeField]
        float _damage = 10f;

        void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger)
                return;

            LivingMonoBehavior health = other.GetComponent<LivingMonoBehavior>();
            if (health)
            {
                health.DeductHealth(_damage);
            }
        }
    }
}
