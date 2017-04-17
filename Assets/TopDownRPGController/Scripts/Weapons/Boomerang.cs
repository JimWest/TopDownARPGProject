using UnityEngine;
using System.Collections;

namespace TopDown
{
    public class Boomerang : Weapon
    {
        [SerializeField]
        float _speed = 10f;
        [SerializeField]
        float _maxRange = 10f;
        [SerializeField]
        Vector3 _startRotation;
        [SerializeField]
        float _rotationSpeed = 500f;

        protected Vector3 _targetPos;
        protected BoomerangState _actionState;
        protected GameObject _oldParent;
        protected Quaternion _oldRotation;
        protected Vector3 _oldPosition;
        protected TrailRenderer _trail;
        protected bool _flying;

        void Start()
        {
            _actionState = BoomerangState.idle;

            if (transform.parent)
            {
                _oldParent = transform.parent.gameObject;
                _oldPosition = transform.localPosition;
                _oldRotation = transform.localRotation;
            }

            _trail = GetComponent<TrailRenderer>();
        }

        void Update()
        {
            if (_isAttacking && _actionState == BoomerangState.idle)
            {
                _targetPos = transform.position + _player.transform.forward * _maxRange;
                _actionState = BoomerangState.flying;
                _flying = true;
                transform.parent = null;
                transform.rotation = Quaternion.Euler(_startRotation);
                _trail.enabled = true;
            }

            if (_flying)
            {
                if (_actionState == BoomerangState.flyingBack)
                {
                    _targetPos = _oldParent.transform.position;
                }

                transform.position = Vector3.MoveTowards(transform.position, _targetPos, _speed * Time.deltaTime);
                transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0, Space.World);

                if (Vector3.Distance(transform.position, _targetPos) <= 0.1f)
                {
                    if (_actionState == BoomerangState.flying)
                    {
                        _actionState = BoomerangState.flyingBack;
                    }
                    else
                    {
                        _flying = false;
                        _isAttacking = false;
                        _actionState = BoomerangState.idle;
                        transform.parent = _oldParent.transform;
                        transform.localPosition = _oldPosition;
                        transform.localRotation = _oldRotation;

                        _trail.enabled = false;
                    }

                }
            }

        }

        public override bool IsReady()
        {
            return !_flying;
        }

        public override void SetAttacking(bool state)
        {
            if (!_flying)
            {
                _isAttacking = state;
            }
        }

        protected enum BoomerangState
        {
            idle,
            flying,
            flyingBack
        }
    } 
}

