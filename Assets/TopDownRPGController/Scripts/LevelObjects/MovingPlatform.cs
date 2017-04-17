using UnityEngine;
using UnityStandardAssets.Utility;

namespace TopDown
{
    [RequireComponent(typeof(WaypointProgressTracker))]
    [RequireComponent(typeof(Rigidbody))]
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField]
        float _speed = 1f;
        [SerializeField]
        bool _autoStart = true;
        [SerializeField]
        float _wpWaitingTime = 1.0f;
        [SerializeField]
        bool _stopAfterWaypoint = false;

        WaypointProgressTracker _wayPointProgress;
        Rigidbody _rigidBody;
        Vector3 _targetPos;
        Vector3 _moveVec;

        bool _moving;
        float _waitingEndTime;
 
        // Use this for initialization
        void Start()
        {
            if (_autoStart)
                _moving = true;

            _rigidBody = GetComponent<Rigidbody>();
            _wayPointProgress = GetComponentInChildren<WaypointProgressTracker>();
            _targetPos = _wayPointProgress.target.position;
        }

        void FixedUpdate()
        {

            if (!_moving)
                return;

            _moveVec = _wayPointProgress.target.position - _rigidBody.position;
            _moveVec.Normalize();

            if (_wayPointProgress.target.position != _targetPos)
            {
                _targetPos = _wayPointProgress.target.position;
                _waitingEndTime = Time.time + _wpWaitingTime;

                if (_stopAfterWaypoint)
                {
                    _moving = false;
                }
            }

            if (Time.time >= _waitingEndTime)
            {
                // we're using ridigbody movement here so other rigidbodys won't fall of the plattform but move with it
                _rigidBody.MovePosition(_rigidBody.position + _moveVec * _speed * Time.fixedDeltaTime);
            }
        }

        public void StartMoving()
        {
            _moving = true;
            _targetPos = _wayPointProgress.target.position;
        }

        public void StopMoving()
        {
            _moving = false;
        }

        public void ToggleMoving()
        {
            _moving = !_moving;
            _targetPos = _wayPointProgress.target.position;
        }
    }
}



