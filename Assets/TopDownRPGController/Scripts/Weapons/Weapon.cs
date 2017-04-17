using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace TopDown
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]
        protected int _damageOnHit = 10;
        [SerializeField]
        protected float _forceOnImpact = 5000f;
        [SerializeField]
        protected Texture2D _icon;
        [SerializeField]
        protected List<AudioClip> _sounds;
        [SerializeField]
        protected Transform _hitTransform;
        [SerializeField]
        protected GameObject _hitPrefab;

        protected GameObject _player;
        public GameObject Player
        {
            get
            {
                return _player;
            }
            set
            {
                _player = value;
            }
        }

        protected bool _isAttacking;
        protected float _lastAttack;
        protected AudioSource _audioObject;
        protected BoxCollider _collider;

        void Start()
        {
            _collider = GetComponentInChildren<BoxCollider>();
            _collider.enabled = false;
            _isAttacking = false;

            if (!_hitTransform)
                _hitTransform = transform;
        }

        void Update()
        {
        }

        void OnTriggerEnter(Collider other)
        {
            if (_isAttacking && !other.isTrigger && !other.CompareTag("Player"))
            {
                Attack(other.gameObject);
            }
        }

        void OnTriggerExit(Collider other)
        {
        }

        public virtual void SetAttacking(bool state)
        {
            if (!gameObject.activeSelf && !_collider)
                return;

            _collider.enabled = state;
            _isAttacking = state;
        }

        protected void Attack(GameObject enemy)
        {
            if (!gameObject.activeSelf)
                return;

            if (enemy.GetComponent<Rigidbody>())
            {
                Vector3 direction = (enemy.transform.position - transform.position);
                enemy.GetComponent<Rigidbody>().AddForce(direction.normalized * _forceOnImpact);
            }

            // find parent
            LivingMonoBehavior enemyHealth = enemy.GetComponentInChildren<LivingMonoBehavior>();
            if (enemyHealth && !enemyHealth.IsDead)
            {
                enemyHealth.DeductHealth(_damageOnHit, _player);
            }



            if (_hitPrefab)
            {
                GameObject hitObject = (GameObject)Instantiate(_hitPrefab, _hitTransform.position, Quaternion.identity);
            }


            PlayAttackSound(_hitTransform.position);
        }

        public virtual bool IsReady()
        {
            return gameObject.activeSelf && !_isAttacking;
        }

        protected virtual void PlayAttackSound(Vector3 position)
        {

            if (_sounds.Any())
            {
                AudioSource.PlayClipAtPoint(_sounds[Random.Range(0, _sounds.Count - 1)], position);
            }
        }



    } 
}
