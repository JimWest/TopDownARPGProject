using UnityEngine;
using System.Collections;

namespace TopDown
{
    public class Pickup : MonoBehaviour
    {

        public GameObject pickupParticle;
        public AudioClip pickupSound;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        protected virtual void OnPickUp()
        {
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnPickUp();

                if (pickupParticle)
                    Instantiate(pickupParticle, transform.position, transform.rotation);

                if (pickupSound)
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);

                Destroy(this.gameObject);
            }
        }

        void OnTriggerExit(Collider other)
        {
        }
    } 
}
