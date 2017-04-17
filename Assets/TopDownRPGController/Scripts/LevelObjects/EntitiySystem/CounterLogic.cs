using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor;
#endif

namespace TopDown.EntitySystem
{
    public class CounterLogic : EntityEventTrigger
    {

        [SerializeField]
        int _startValue = 0;
        [SerializeField]
        int _maxValue = 1;
        [SerializeField]
        int _minValue = 0;
        [SerializeField]
        bool _resetAfterReached = false;

        int _currentValue = 0;

#if UNITY_EDITOR
        [MenuItem("GameObject/Entity/CounterLogic", false, 10)]
        static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            CreateGameObject<CounterLogic>(menuCommand);
        }
#endif

        protected override void Start()
        {
            _currentValue = _startValue;
            base.Start();
        }

        public void Add()
        {
            Add(1);
        }

        public void Add(int amount)
        {
            _currentValue = Mathf.Min(_currentValue + amount, _maxValue);
            CheckCount();
        }

        public void Sub()
        {
            Sub(1);
        }

        public void Sub(int amount)
        {
            _currentValue = Mathf.Max(_currentValue - 1, _minValue);
            CheckCount();
        }

        public void Reset()
        {
            _currentValue = _startValue;
        }

        private void CheckCount()
        {
            bool reached = false;

            if (_currentValue >= _maxValue)
            {
                OnMaxCountReached();
                reached = true;
            }
            else if (_currentValue <= _minValue)
            {
                OnMinCountReached();
                reached = true;
            }

            OnValueChanged();

            if (reached && _resetAfterReached)
                Reset();
        }

        [EntityOutputEvent]
        void OnMaxCountReached()
        {
            Execute("OnMaxCountReached");
        }

        [EntityOutputEvent]
        void OnMinCountReached()
        {
            Execute("OnMinCountReached");
        }

        [EntityOutputEvent]
        void OnValueChanged()
        {
            Execute("OnValueChanged");
        }

    }
}