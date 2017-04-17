using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor;
#endif

namespace TopDown.EntitySystem
{
    public class CompareLogic : EntityEventTrigger
    {

        [SerializeField]
        int _valueA = 0;
        [SerializeField]
        int _valueB = 0;

        public int ValueA
        {
            get
            {
                return _valueA;
            }
            set
            {
                _valueA = value;
            }
        }

        public int ValueB
        {
            get
            {
                return _valueB;
            }
            set
            {
                _valueB = value;
            }
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Entity/CompareLogic", false, 10)]
        static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            CreateGameObject<CompareLogic>(menuCommand);
        }
#endif

        public void Compare()
        {
            int compValue = _valueA.CompareTo(_valueB);
            if (compValue == 0)
            {
                OnEqualTo();
            }
            else
            {
                OnNotEqualTo();

                if (compValue < 0)
                {
                    OnLessThan();
                }
                else
                {
                    OnGreaterThan();
                }
            }
        }


        // A == B
        [EntityOutputEvent]
        void OnEqualTo()
        {
            Execute("OnEqualTo");
        }

        // A != B
        [EntityOutputEvent]
        void OnNotEqualTo()
        {
            Execute("OnNotEqualTo");
        }

        // A < B
        [EntityOutputEvent]
        void OnLessThan()
        {
            Execute("OnLessThan");
        }

        // A > B
        [EntityOutputEvent]
        void OnGreaterThan()
        {
            Execute("OnGreaterThan");
        }
    }
}
