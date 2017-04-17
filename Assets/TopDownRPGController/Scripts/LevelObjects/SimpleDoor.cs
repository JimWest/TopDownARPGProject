using UnityEngine;
using System.Collections;

namespace TopDown
{

    public enum OpeningDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public class SimpleDoor : MonoBehaviour
    {
        [SerializeField]
        Vector3 _movementDirection = Vector3.up;
        [SerializeField]
        float _movingAmount = 3f;
        [SerializeField]
        float _speed = 2f;
        [SerializeField]
        bool _locked = false;

        bool _moving;
        Vector3 _closedPosition;
        Vector3 _endpos;
        Vector3 _dirVector;
        Vector3 _startPos;

        void Start()
        {
            _closedPosition = transform.position;
            _movementDirection = _movementDirection.normalized;
        }

        // we're using fixed update cause the camera is also using fixed update (would look like lagging if we're using normal update)
        void Update()
        {
            if (!_moving)
                return;

            // the object reached the destination
            if (Vector3.Distance(transform.position, _endpos) <= 0.01f)
            {
                _moving = false;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _endpos, Time.deltaTime * _speed);
            }
        }

        public void Open()
        {
            if (_locked)
                return;

            _moving = true;
            _endpos = _closedPosition + (_movementDirection * _movingAmount);
        }

        public void Close()
        {
            _moving = true;
            _endpos = _closedPosition;
        }

        public void Lock()
        {
            _locked = true;
        }

        public void Unlock()
        {
            _locked = false;
        }
    }

}
