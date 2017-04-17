using UnityEngine;
using System.Collections;

namespace TopDown
{
    public class AnimatedSwitch : MonoBehaviour
    {
        Animator _animator;
        bool _activated = false;

        // unfortunately, properties wont work with the Event System, so we need to use these functions
        public void Activate()
        {
            _activated = true;
        }

        public void Deactivate()
        {
            _activated = false;
        }

        public void Toggle()
        {
            _activated = !_activated;
        }

        void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            
        }

        void Update()
        {
            if (_animator)
                _animator.SetBool("Activate", _activated);
        }


    }
}
