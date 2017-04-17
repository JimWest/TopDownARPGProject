using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor;
#endif

namespace TopDown.EntitySystem
{
    public class TriggerLogic : EntityEventTrigger
    {
        [SerializeField]
        string[] _activationTags = { "Player" };

        [SerializeField]
        protected bool _oneTimeActivation = false;

        protected bool _triggered = false;

#if UNITY_EDITOR
        [MenuItem("GameObject/Entity/TriggerLogic", false, 10)]
        static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            CreateGameObject<TriggerLogic>(menuCommand);
        }
#endif

        protected bool ShouldTrigger(Collider other)
        {
            bool shouldTrigger = true;
            if (_oneTimeActivation)
                shouldTrigger = !_triggered;

            bool tagOk = false;
            foreach (string tag in _activationTags)
            {
                if (other.CompareTag(tag))
                {
                    tagOk = true;
                    break;
                }
            }

            return shouldTrigger && tagOk;
        }

        [EntityOutputEvent]
        void OnTriggerEnter(Collider other)
        {
            if (ShouldTrigger(other))
            {
                Execute("OnTriggerEnter");
                _triggered = true;
            }
        }

        [EntityOutputEvent]
        void OnTriggerExit(Collider other)
        {
            if (ShouldTrigger(other))
            {
                Execute("OnTriggerExit");
                _triggered = true;
            }
        }

        [EntityOutputEvent]
        public void OnTriggerStay(Collider other)
        {
            if (ShouldTrigger(other))
            {
                Execute("OnTriggerStay");
                _triggered = true;
            }
        }
    }
}

