using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

namespace TopDown
{
    public class PlayerController : Controller
    {

        [SerializeField]
        float _inputDamping = 6f;
        [SerializeField]
        float _inputThreshold = 0.2f;

        GameObject _mainCamera;
        Vector3 _input;
        WeaponHandler _weaponHolder;
        InteractionLogic _interactionLogic;

        float _lastH;
        float _lastV;
        bool _jump;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        protected virtual void Update()
        {

            if (_pawn.IsDead)
                return;

            if (CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                _jump = true;
            }

            if (CrossPlatformInputManager.GetButtonDown("Fire1"))
            {
                _pawn.Attack();
            }

            if (CrossPlatformInputManager.GetButtonDown("Use"))
            {
                _pawn.Use();
            }

            float mouseRotation = (CrossPlatformInputManager.GetAxisRaw("Mouse ScrollWheel"));
            if (mouseRotation > 0)
            {
                _pawn.WeaponHolder.EquipNextWeapon();
            }
            else if (mouseRotation < 0)
            {
                _pawn.WeaponHolder.EquipPrevWeapon();
            }

        }

        protected virtual void FixedUpdate()
        {
            if (_pawn.IsDead)
                return;

            // Store the input axes.
            float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            float v = CrossPlatformInputManager.GetAxisRaw("Vertical");

            if (!CanControl || !_pawn.CanMove)
            {
                h = 0;
                v = 0;
                _lastH = 0;
                _lastV = 0;
            }
            else
            {
                if (Mathf.Abs(h) < _inputThreshold)
                    h = 0;

                if (Mathf.Abs(v) < _inputThreshold)
                    v = 0;

                h = Mathf.Lerp(_lastH, h, Time.deltaTime * _inputDamping);
                _lastH = h;

                v = Mathf.Lerp(_lastV, v, Time.deltaTime * _inputDamping);
                _lastV = v;

                // calculate move direction to pass to character
                if (_mainCamera != null)
                {
                    // calculate camera relative direction to move:
                    _input = v * Vector3.Scale(_mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized + h * _mainCamera.transform.right;
                }
                else
                {
                    // we use world-relative directions in the case of no main camera
                    _input = v * Vector3.forward + h * Vector3.right;
                }
            }

            _pawn.Move(_input);

            // jump needs to be called after move cause it effects the grounded values etc., it also needed to be called in fixed update
            if (_jump)
                _pawn.Jump();

            _jump = false;

        }

    }
}
