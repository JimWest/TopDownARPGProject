using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor;
#endif

namespace TopDown.EntitySystem
{
    public class BranchLogic : EntityEventTrigger
    {
        [SerializeField]
        bool _testValue;

        public bool TestValue
        {
            get
            {
                return _testValue;
            }
            set
            {
                _testValue = value;
            }
        }

#if UNITY_EDITOR 
        [MenuItem("GameObject/Entity/BranchLogic", false, 10)]
        static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            CreateGameObject<BranchLogic>(menuCommand);
        }
#endif

        public void Test()
        {
            if (_testValue)
                OnTrue();
            else
                OnFalse();
        }

        public void Toggle()
        {
            _testValue = !_testValue;
        }

        [EntityOutputEvent]
        void OnTrue()
        {
            Execute("OnTrue");
        }

        [EntityOutputEvent]
        void OnFalse()
        {
            Execute("OnFalse");
        }


    }
}
