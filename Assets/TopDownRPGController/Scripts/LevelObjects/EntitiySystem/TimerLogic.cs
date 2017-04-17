using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor;
#endif

namespace TopDown.EntitySystem
{
    public class TimerLogic : EntityEventTrigger
    {

        [SerializeField]
        float _time = 10;
        [SerializeField]
        bool _autoStart = false;
        [SerializeField]
        bool _autoRestart = false;

        float _currentTime;
        bool _started;

#if UNITY_EDITOR
        [MenuItem("GameObject/Entity/TimerLogic", false, 10)]
        static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            CreateGameObject<TimerLogic>(menuCommand);
        }
#endif

        protected override void Start()
        {
            _currentTime = 0;
            if (_autoStart)
                StartTimer();

            base.Start();
        }

        void Update()
        {
            if (!_started)
                return;

            _currentTime += Time.deltaTime;
            if (_currentTime >= _time)
            {
                OnTimeElapsed();
                if (_autoRestart)
                {
                    ResetTimer();
                    StartTimer();
                }
                else
                {
                    _started = false;
                }
            }

        }

        public void StartTimer()
        {
            _started = true;
        }

        public void PauseTimer()
        {
            _started = false;
        }

        public void ResetTimer()
        {
            _started = false;
            _currentTime = 0;
        }

        [EntityOutputEvent]
        void OnTimeElapsed()
        {
            Execute("OnTimeElapsed");
        }
    }
}
