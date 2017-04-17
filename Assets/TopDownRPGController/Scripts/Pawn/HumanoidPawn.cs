using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

namespace TopDown
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class HumanoidPawn : Pawn
    {

        [Header("Movement")]
        [SerializeField]
        PhysicMaterial _jumpPhysicsMaterial;
        [SerializeField]
        PhysicMaterial _groundPhysicsMaterial;
        [SerializeField]
        float _speed = 6f;
        [SerializeField]
        float _airSpeed = 4f;
        [SerializeField]
        float _turnSpeed = 10f;
        [SerializeField]
        float _airTurnSpeed = 5f;
        [SerializeField]
        float _rotationWaitAngle = 80;
        [SerializeField]
        AnimationCurve _slopeCurveModifier = new AnimationCurve(new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 1.0f), new Keyframe(180.0f, 0.0f));

        [Space(10)]
        [SerializeField]
        float _groundCheckDistance = 0.4f;
        [SerializeField]
        float _groundCheckOffset = 0.1f;
        [SerializeField]
        float stickToGroundHelperDistance = 0.5f;
        [SerializeField]
        float shellOffset = 0.1f;
        [SerializeField]
        float steppOffsetUpper = 0.45f;
        [SerializeField]
        float steppOffsetLower = -0.5f;
        [SerializeField]
        float steppThreshold = 0.05f;
        [SerializeField]
        float stepSmooth = 1f;

        [Space(10)]
        [SerializeField]
        bool _canJump = true;
        [SerializeField]
        bool _autoJump = true;
        [SerializeField]
        float _jumpUpPower = 6;
        [SerializeField]
        float _jumpForwardPower = 3;
        [SerializeField]
        float _autoJumpMagnitude = 0.3f;
        [SerializeField]
        float _autoJumpWaitTime = 0.2f;

        // objects        
        Rigidbody _rigidbody;
        CapsuleCollider _collider;
        Vector3 _input;
        Vector3 _movement;
        Vector3 _jumpDir;
        Vector3 _groundContactNormal;
        Quaternion _newRotation;

        float _lastAirTime = 0f;
        float _lastGroundTime = 0f;
        float _lastAutoJump = 0f;
        float _lastJump = 0f;
        float _lerpedLookAtWeight = 0f;
        float _useWaitTime = 1f;

        float _originalGroundCheckDistance;
        float _groundDistance;

        bool _onAttackAnimation;
        bool _onJumpUpAnimation;
        bool _onUsingAnimation;
        bool _using;
        bool _jumpHandled;
        bool _disableNextGroundcheck = false;

        IUseable _ueableObject;

        public IUseable UseableObject
        {
            get { return _ueableObject; }
            set { _ueableObject = value; }
        }

        #region Start and Update
        protected override void Start()
        {
            base.Start();
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();

            _originalGroundCheckDistance = _groundCheckDistance;
        }

        protected override void Update()
        {
            base.Update();
            UpdateAnimator();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
        }

        #endregion

        #region Pawn Callbacks
        public override void Move(Vector3 input)
        {
            _input = input;
            if (_input.magnitude > 1.0f)
                _input.Normalize();

            _attack = false;

            if (!IsDead)
            {
                UpdateMovement();
                UpdateRotation();
            }

            GroundCheck();
        }


        public override void Jump()
        {
            if (GetCanJump())
            {
                AddJumpForce();
                _lastJump = Time.time;
            }
        }

        public override void Attack()
        {
            if (_weaponHolder && _weaponHolder.IsPrimaryWeaponReady())
            {
                _attack = true;
            }
        }

        public override void Use()
        {
            if (_interactionLogic != null)
            {
                IUseable useableObject = _interactionLogic.UseableObject;
                if (useableObject != null && useableObject.CanBeUsed(gameObject) && (Time.time >= _lastUseTime + _useWaitTime))
                {
                    _lastUseTime = Time.time;
                    useableObject.OnUse(gameObject);
                }
            }
        }

        public virtual bool GetCanMove()
        {
            return !_onUsingAnimation && !_onAttackAnimation;
        }

        public virtual bool GetCanJump()
        {
            return _canJump && _onGround && !_onAttackAnimation;
        }
        #endregion

        #region Movement

        private float SlopeMultiplier()
        {
            float angle = Vector3.Angle(_groundContactNormal, transform.forward);
            //Debug.Log(_slopeCurveModifier.Evaluate(angle));
            return _slopeCurveModifier.Evaluate(angle);
        }

        private void StickToGroundHelper()
        {
            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, _collider.radius * (1.0f - shellOffset), Vector3.down, out hitInfo,
                                   ((_collider.height / 2f) - _collider.radius) +
                                   stickToGroundHelperDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
                {
                    _rigidbody.velocity = Vector3.ProjectOnPlane(_rigidbody.velocity, hitInfo.normal);
                }
            }
        }

        void StepOffset()
        {
            Vector3 rayStart = (transform.position + new Vector3(0, steppOffsetUpper, 0) + transform.forward * ((_collider).radius + 0.1f));
            float length = steppOffsetUpper - steppOffsetLower;

#if UNITY_EDITOR
            // helper to visualise the ground check ray in the scene view
            Debug.DrawLine(rayStart, rayStart + (Vector3.down * length), Color.yellow);
#endif

            if (_input.magnitude < 0.2f)
                return;

            LayerMask layerMask = ~(1 << gameObject.layer);

            var hit = new RaycastHit();
            Ray rayStep = new Ray(rayStart, Vector3.down);

            // Check if its a stair or a slope

            if (Physics.Raycast(rayStep, out hit, length, layerMask))
            {
                bool above = false;
                bool beneath = false;
                Vector3 capsuleBottom = (transform.position + _collider.center) + (Vector3.down * (_collider.height / 2f));

                if (hit.point.y >= (capsuleBottom.y + steppThreshold) && hit.point.y <= (capsuleBottom.y + steppOffsetUpper))
                {
                    // point is above
                    above = true;
                }
                else if (hit.point.y <= (capsuleBottom.y - steppThreshold) && hit.point.y >= (capsuleBottom.y - steppOffsetUpper))
                {
                    // point is beneath
                    beneath = true;
                }


                if (above || beneath)
                {
                    // cast if the point is reachable and nothing intersects, if yes it must be a slope

                    Vector3 ray2Start = hit.point + Vector3.up * 0.01f;
                    Vector3 ray2dir = capsuleBottom - ray2Start;
                    Ray ray2 = new Ray(ray2Start, ray2dir);
                    var hit2 = new RaycastHit();

#if UNITY_EDITOR
                    Debug.DrawLine(capsuleBottom, capsuleBottom + Vector3.right * .1f);
                    // helper to visualise the ground check ray in the scene view
                    Debug.DrawLine(ray2.origin, ray2.origin + (ray2.direction * ray2.direction.magnitude), Color.cyan);
#endif


                    if (!Physics.Raycast(ray2, out hit2, ray2.direction.magnitude, layerMask))
                    {
                        //Debug.Log("Slope");
                        if (Mathf.Abs(Vector3.Angle(hit.normal, Vector3.up)) < 85f)
                        {
                            _rigidbody.velocity = Vector3.ProjectOnPlane(_rigidbody.velocity, Vector3.Cross(capsuleBottom, ray2Start).normalized);
                        }
                    }
                    else
                    {
                        //Debug.Log("Stair");

                        // its a stair, teleport the player
                        Vector3 heightPoint = _rigidbody.position + _input * _speed;
                        if (above)
                            heightPoint.y = hit.point.y + 0.3f;
                        else
                            heightPoint.y = hit.point.y;

                        _rigidbody.position = Vector3.Lerp(_rigidbody.position, heightPoint, stepSmooth * Time.fixedDeltaTime);


                        // also kill the velocity 
                        _rigidbody.velocity = Vector3.zero;
                    }
                }
            }
        }

        void UpdateMovement()
        {
            if (!GetCanMove())
                return;

            // control and velocity handling is different when grounded and airborne:
            if (_onGround)
            {
                // Ground movement                
                _jumpHandled = false;

                // only move if rotated to the desired direction
                if (Quaternion.Angle(_rigidbody.rotation, _newRotation) <= _rotationWaitAngle)
                {
                    if (_input.magnitude > 0.1f)
                    {
                        _movement = _input * _speed;
                        //_rigidbody.MovePosition(_rigidbody.position + _movement * Time.fixedDeltaTime);
                        ApplyMovementForce(_movement * SlopeMultiplier());
                    }

                }
                else
                {
                    _movement.Set(0, 0, 0);
                }

                //StickToGroundHelper();
                StepOffset();
            }
            else
            {
                // Airmovement
                if (!_jumpHandled)
                {
                    if (_autoJump && _input.magnitude > _autoJumpMagnitude && (_lastAutoJump == 0 || Time.time >= _lastAutoJump + _autoJumpWaitTime))
                    {
                        // Auto jump!
                        AddJumpForce();
                        _lastAutoJump = Time.time;
                    }
                    else
                    {
                        _jumpHandled = true;
                    }

                }
                else
                {
                    if (!_onJumpUpAnimation)
                    {
                        // we allow some movement in air, but it's very different to when on ground
                        // (typically allowing a small change in trajectory)
                        _movement = _input * _airSpeed;

                        //velocity = Vector3.Lerp (velocity, airMove, Time.deltaTime * airControl);
                        //Vector3 extraGravityForce = (Physics.gravity * gravityMultiplier) - Physics.gravity;
                        //_rigidbody.MovePosition(_rigidbody.position + _movement);
                        ApplyMovementForce(_input * _airSpeed);
                    }
                }
            }
        }

        void ApplyMovementForce(Vector3 force)
        {
            // Calculate how fast we should be moving
            Vector3 targetVelocity = force;
            float _maxVelocityChange = 10f;

            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = _rigidbody.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -_maxVelocityChange, _maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -_maxVelocityChange, _maxVelocityChange);
            velocityChange.y = 0;
            _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        }

        void AddJumpForce()
        {
            // jump a bit forward despite of the input
            if (_input.magnitude < 0.3f)
            {
                _jumpDir = transform.forward * 0.3f;
            }
            else
            {
                _jumpDir = _input;
            }

            _rigidbody.velocity = _jumpDir * _jumpForwardPower + transform.up * _jumpUpPower;
            _jumpHandled = true;
            _onGround = false;
            _disableNextGroundcheck = true;
            _groundCheckDistance = 0.2f;
            // change material to jumpmaterial (no friction), so the character wont stick on walls
            _collider.material = _jumpPhysicsMaterial;
        }

        // Remove the velocity after landed, if not it will keep on jumping
        void OnLanded()
        {
            _collider.material = _groundPhysicsMaterial;
            _rigidbody.velocity.Set(0, 0, 0);
            _movement.Set(0, 0, 0);

            _groundCheckDistance = _originalGroundCheckDistance;
        }

        #endregion

        #region Rotation

        void UpdateRotation()
        {
            if (!GetCanMove())
                return;


            if (_input != Vector3.zero)
            {
                _newRotation = Quaternion.LookRotation(_input);
                float speed = 0;

                if (_onGround)
                    speed = _turnSpeed;
                else
                    speed = _airTurnSpeed;

                _rigidbody.MoveRotation(Quaternion.Lerp(_rigidbody.rotation, _newRotation, speed * Time.fixedDeltaTime));

            }
            else
            {
                // keep rotating if theres no new input
                Quaternion lerpedRotation = Quaternion.Lerp(_rigidbody.rotation, _newRotation, _turnSpeed * Time.fixedDeltaTime);
                _rigidbody.MoveRotation(lerpedRotation);
            }
        }

        #endregion

        #region Ground Check
        void GroundCheck()
        {
            if (_disableNextGroundcheck)
            {
                _disableNextGroundcheck = false;
                return;
            }

            RaycastHit hitInfo;
            bool oldOnGround = _onGround;
            // check every layer except player            
            LayerMask layerMask = ~(1 << gameObject.layer);

            // bottom center of the capsule collider
            Vector3 groundCheckStart = (transform.position + _collider.center) + (Vector3.down * (_collider.height / 2f));
            groundCheckStart = groundCheckStart + (Vector3.up * _groundCheckOffset);


#if UNITY_EDITOR
            // helper to visualise the ground check ray in the scene view
            Debug.DrawLine(groundCheckStart, groundCheckStart + (Vector3.down * _groundCheckDistance), Color.red);
            Debug.DrawLine(groundCheckStart + transform.forward * 0.25f, (groundCheckStart + transform.forward * 0.25f) + (Vector3.down * _groundCheckDistance * 2f), Color.blue);
#endif

            if (Physics.Raycast(groundCheckStart, Vector3.down, out hitInfo, _groundCheckDistance, layerMask, QueryTriggerInteraction.Ignore))
            {
                _onGround = true;
                _groundContactNormal = hitInfo.normal;
            }
            else
            {
                // Prevents autojump from stairs etc.
                if (oldOnGround && Physics.Raycast(groundCheckStart + transform.forward * 0.15f, Vector3.down, out hitInfo, _groundCheckDistance * 2, layerMask))
                {
                    _onGround = true;
                    _groundContactNormal = hitInfo.normal;
                }
                else
                {
                    _onGround = false;
                    _groundContactNormal = Vector3.up;
                }
            }

            if (!oldOnGround && _onGround)
            {
                OnLanded();
            }

            // remember when we were last in air, for jump delay
            if (!_onGround)
                _lastAirTime = Time.time;
            if (_onGround)
                _lastGroundTime = Time.time;

        }

        #endregion

        #region Animation

        void UpdateAnimator()
        {
            AnimatorStateInfo animState = _animator.GetCurrentAnimatorStateInfo(0);
            AnimatorStateInfo animState1 = _animator.GetCurrentAnimatorStateInfo(1);

            _animator.SetFloat("Speed", _input.magnitude * _speed, 0f, Time.deltaTime);
            _animator.SetBool("OnGround", _onGround);
            _animator.SetFloat("Jump", _rigidbody.velocity.y);

            // calculate which leg is behind, so as to leave that leg trailing in the jump animation
            // (This code is reliant on the specific run cycle offset in our animations,
            // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
            float runCycle = Mathf.Repeat(animState.normalizedTime, 1);
            float jumpLeg = (runCycle < 0.5f ? 1 : -1);
            if (_onGround)
            {
                _animator.SetFloat("JumpLeg", jumpLeg);
            }

            _animator.SetBool("Crouch", false);

            if (_attack)
            {
                if (Random.Range(1, 100) <= 10)
                {
                    _animator.SetBool("Attack", false);
                    _animator.SetBool("SpecialAttack", true);
                }
                else
                {
                    _animator.SetBool("Attack", true);
                    _animator.SetBool("SpecialAttack", false);
                }

            }
            else
            {
                _animator.SetBool("Attack", false);
                _animator.SetBool("SpecialAttack", false);

            }

            _animator.SetBool("Using", _using);

            _onAttackAnimation = animState.IsName("Attack") || animState.IsName("SpecialAttack");
            _onUsingAnimation = animState1.IsName("Using");
            _onJumpUpAnimation = animState.IsName("Climbing.Jump Up");
        }

        void AttackStart(string attackName)
        {
            _weaponHolder.SetPrimaryWeaponAttacking(true);
        }

        void AttackEnd(string attackName)
        {
            _weaponHolder.SetPrimaryWeaponAttacking(false);
        }

    }

    #endregion
}