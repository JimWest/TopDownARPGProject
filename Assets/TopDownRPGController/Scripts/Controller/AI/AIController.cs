using UnityEngine;
using System.Collections;

namespace TopDown
{
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

    public class AIController : MonoBehaviour
    {

        [SerializeField]
        Transform _target; // target to aim for
        [SerializeField]
        bool _randomPosition;
        [SerializeField]
        float _randomWalkRadius = 20;

        Vector3 _oldTargetPos;
        Pawn _pawn;
        UnityEngine.AI.NavMeshAgent _agent;
        bool _targetReached;

        void Start()
        {

            _agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            _pawn = GetComponentInParent<Pawn>();

            _targetReached = true;

            if (_randomPosition)
            {
                _target = new GameObject().transform;
                Random.InitState((int)System.DateTime.Now.Ticks);
            }
            else
            {
                if (_target)
                {
                    _oldTargetPos = _target.position;
                    _agent.SetDestination(_target.position);
                }
            }

            _agent.updateRotation = false;
            _agent.updatePosition = false;

        }

        void FixedUpdate()
        {
            if (_randomPosition && _targetReached)
            {
                Vector3 randomDirection = Random.insideUnitSphere * _randomWalkRadius;
                randomDirection += transform.position;
                UnityEngine.AI.NavMeshHit hit;
                UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, _randomWalkRadius, 1);
                _target.position = hit.position;
                _oldTargetPos = hit.position;
                _agent.SetDestination(_target.position);

                _targetReached = false;

            }


            if (_target != null)
            {

                // recalculate path if needed
                if (Vector3.Distance(_target.position, _oldTargetPos) > 1f)
                {
                    _oldTargetPos = _target.position;
                    _agent.SetDestination(_target.position);
                }

                // custom off mesh traversal code
                if (_agent.isOnOffMeshLink)
                {
                    Vector3 offMeshDir = (_agent.currentOffMeshLinkData.endPos - _agent.currentOffMeshLinkData.startPos).normalized;
                    Vector3 endPos = _agent.currentOffMeshLinkData.endPos + Vector3.up * _agent.baseOffset + offMeshDir * 0.05f;
                    Vector3 offMeshDist = transform.position - endPos;
                    offMeshDist.y = 0;

                    if (Mathf.Abs(offMeshDist.sqrMagnitude) < 0.1f)
                    {
                        _agent.CompleteOffMeshLink();
                    }
                    else
                    {
                        _pawn.Move((endPos - transform.position));
                    }
                }
                else
                {
                    //    // use the values to move the character
                    _pawn.Move(_agent.desiredVelocity.normalized);
                    _agent.nextPosition = transform.position;

                }


                // warp the agent if needed (sometimes the agent thinks its on a highter level, but the character is still on ground)
                if (_pawn.IsOnGround && Mathf.Abs(_agent.nextPosition.y - transform.position.y) > 0.5f)
                {
                    _agent.Warp(transform.position);
                    _agent.SetDestination(_target.position);
                }



                if ((_agent.desiredVelocity.normalized == Vector3.zero) || (_agent.remainingDistance <= _agent.stoppingDistance + 0.01))
                {
                    _targetReached = true;
                }

            }
            else
            {
                // We still need to call the character's move function, but we send zeroed input as the move param.
                _pawn.Move(Vector3.zero);
            }


        }

        public void SetTarget(Transform target)
        {
            _oldTargetPos = target.position;
            _target = target;
        }
    }
}
