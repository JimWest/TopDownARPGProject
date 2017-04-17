using UnityEngine;
using System.Collections;

namespace TopDown
{
    public class LivingMonoBehavior : MonoBehaviour
    {

        public delegate void AddHealthAction();
        public static event TakeDamageAction OnAddHealthEvent;

        public delegate void TakeDamageAction();
        public static event TakeDamageAction OnTakeDamageEvent;

        public delegate void DeathAction();
        public static event DeathAction OnDeathEvent;

        [Header("Health, Armor")]
        [SerializeField]
        float _maxHealth = 100f;
        [SerializeField]
        bool _invincible = false;

        private bool _dead;
        public bool IsDead
        {
            get
            {
                return _dead;
            }
        }

        private float _health;

        protected virtual void Start()
        {
            _health = _maxHealth;
        }

        protected virtual void Update()
        {

        }

        public void AddHealth(float amount)
        {
            _health = Mathf.Min(_health += amount, _maxHealth);
            OnAddHealth();

            if (OnAddHealthEvent != null)
                OnAddHealthEvent();
        }


        public void DeductHealth(float amount)
        {
            DeductHealth(amount, null, transform.position);
        }

        public void DeductHealth(float amount, GameObject doer)
        {
            DeductHealth(amount, doer, transform.position);
        }

        public void DeductHealth(float amount, GameObject doer, Vector3 position)
        {
            if (!_invincible && GetCanTakeDamage(doer, position))
            {   
                _health = Mathf.Max(_health -= amount, 0);
                OnTakeDamage();

                if (OnTakeDamageEvent != null)
                    OnTakeDamageEvent();

                if (_health <= 0)
                {
                    TriggerDead();
                }
            }
        }

        protected void TriggerDead()
        {
            if (!_dead)
            {
                _dead = true;

                if (GetComponent<Collider>())
                {
                    GetComponent<Collider>().enabled = false;

                    if (GetComponent<Rigidbody>())
                    {
                        GetComponent<Rigidbody>().isKinematic = true;
                    }
                }

                OnDeath();

                if (OnDeathEvent != null)
                    OnDeathEvent();
            }
        }

        protected virtual bool GetCanTakeDamage(GameObject doer, Vector3 position)
        {
            return true;
        }

        protected virtual void OnAddHealth() { }
        protected virtual void OnTakeDamage() { }
        protected virtual void OnDeath() { }

    } 
}
