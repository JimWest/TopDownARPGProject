using UnityEngine;
using System.Collections;

namespace TopDown
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        float _speed = 5f;

        [SerializeField]
        Transform _followObject;

        Vector3 _offset;

        void Start()
        {
            // Calculate the initial offset.
            if (!_followObject)
            {
                enabled = false;
                Debug.LogError("No object to follow");
                return;
            }
                
            _offset = transform.position - _followObject.position;
        }

        void Update()
        {
            // Create a postion the camera is aiming for based on the offset from the target.
            Vector3 targetCamPos = _followObject.position + _offset;

            // Smoothly interpolate between the camera's current position and it's target position.
            transform.position = Vector3.Lerp(transform.position, targetCamPos, _speed * Time.deltaTime);
        }
    }
}