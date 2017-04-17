using UnityEngine;
using System.Collections;

namespace TopDown
{
    public class Controller : MonoBehaviour
    {

        protected Pawn _pawn;

        protected bool _canControl = true;
        public bool CanControl
        {
            get
            {
                return _canControl;
            }
            set
            {
                _canControl = value;
            }

        }

        protected virtual void Start()
        {
            _pawn = transform.parent.GetComponentInChildren<Pawn>();

            if (_pawn == null)
            {
                Debug.Log("No pawn found, deactiviating this controller");
                gameObject.SetActive(false);
            }
        }

    }
}
