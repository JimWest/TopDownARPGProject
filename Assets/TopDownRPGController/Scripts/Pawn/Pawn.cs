using UnityEngine;
using System.Collections;

namespace TopDown
{
    // Handles animations and model specific stuff
    public abstract class Pawn : LivingMonoBehavior
    {
        [SerializeField]
        protected float _attackWaitTime = 0.5f;

        protected Animator _animator;
        protected float _lastAttackTime;
        protected float _lastUseTime;
        protected bool _attack;
        protected bool _isAttacking;
        public bool IsAttacking
        {
            get
            {
                return _isAttacking;
            }
            protected set
            {
                _isAttacking = value;
            }
        }

        protected float _currentSpeed;
        public float CurrentSpeed
        {
            get
            {
                return _currentSpeed;
            }
            protected set
            {
                _currentSpeed = value;
            }
        }

        protected bool _onGround = true;
        public bool IsOnGround
        {
            get { return _onGround; }
        }

        public virtual bool CanMove
        {
            get { return true; }
        }

        protected WeaponHandler _weaponHolder;
        public WeaponHandler WeaponHolder
        {
            get { return _weaponHolder; }
        }

        protected InteractionLogic _interactionLogic;


        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            SetUpAnimator();

            _weaponHolder = GetComponentInChildren<WeaponHandler>();
            _interactionLogic = GetComponentInChildren<InteractionLogic>();
        }


        void SetUpAnimator()
        {
            // this is a ref to the animator component on the root.
            _animator = GetComponent<Animator>();

            // we use avatar from a child animator component if present
            // (this is to enable easy swapping of the character model as a child node)
            foreach (var childAnimator in GetComponentsInChildren<Animator>())
            {
                if (childAnimator != _animator)
                {
                    _animator.avatar = childAnimator.avatar;
                    Destroy(childAnimator);
                    break;
                }
            }
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            IsAttacking = _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");

            _animator.SetBool("Attack", _attack);
            //_animator.SetFloat("Speed", _currentSpeed);
        }

        protected virtual void LateUpdate()
        {
            _animator.SetBool("Hurt", false);
        }

        public abstract void Move(Vector3 input);
        public abstract void Jump();
        public abstract void Attack();
        public abstract void Use();

        protected override void OnTakeDamage()
        {
            _animator.SetBool("Hurt", true);
        }

        protected override void OnDeath()
        {
            _animator.SetFloat("Speed", 0);
            _animator.SetBool("Death", true);
        }

    } 
}
