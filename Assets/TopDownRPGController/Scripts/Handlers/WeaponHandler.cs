using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TopDown;

namespace TopDown
{
    [RequireComponent(typeof(Pawn))]
    public class WeaponHandler : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> _weapons;
        [SerializeField]
        Transform _primaryAttachPoint;

        int _currentWeaponIndex = 0;
        Pawn _pawn;
        Weapon _activeWeapon;

        // Use this for initialization
        void Start()
        {
            _pawn = GetComponent<Pawn>();
            if (_primaryAttachPoint)
                _activeWeapon = _primaryAttachPoint.GetComponentInChildren<Weapon>();

            if (_activeWeapon)
                _activeWeapon.Player = _pawn.gameObject;

        }

        public bool IsPrimaryWeaponReady()
        {
            return isActiveAndEnabled && _activeWeapon && _activeWeapon.IsReady();
        }

        public void SetPrimaryWeaponAttacking(bool attacking)
        {
            if (!isActiveAndEnabled || !_activeWeapon)
                return;

            _activeWeapon.SetAttacking(attacking);
        }

        public void EquipNextWeapon()
        {
            if (!isActiveAndEnabled)
                return;

            _currentWeaponIndex++;
            if (_currentWeaponIndex + 1 > _weapons.Count)
                _currentWeaponIndex = 0;

            EquipWeapon(_weapons[_currentWeaponIndex]);

        }

        public void EquipPrevWeapon()
        {
            if (!isActiveAndEnabled)
                return;

            _currentWeaponIndex--;
            if (_currentWeaponIndex < 0)
                _currentWeaponIndex = _weapons.Count - 1;

            EquipWeapon(_weapons[_currentWeaponIndex]);

        }

        public void AddWeapon(GameObject weapon)
        {
            _weapons.Add(weapon);
        }

        public void RemoveWeapon(GameObject weapon)
        {
            _weapons.Remove(weapon);
        }

        void EquipWeapon(GameObject Weapon)
        {
            if (Weapon == null || _primaryAttachPoint == null)
                return;

            if (_activeWeapon)
                Destroy(_activeWeapon.gameObject);

            GameObject newWeapon = (GameObject)Instantiate(Weapon, _primaryAttachPoint.position, _primaryAttachPoint.rotation);
            newWeapon.transform.parent = _primaryAttachPoint;
            _activeWeapon = newWeapon.GetComponentInChildren<Weapon>();
            if (!_activeWeapon)
            {
                Debug.Log("Weapon Prefab has no Weapon script attached");
            }
            else
            {
                _activeWeapon.Player = _pawn.gameObject;
            }
        }

    }

}