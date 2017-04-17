using UnityEngine;
using System.Collections;

namespace TopDown
{
    public class ClearSight : MonoBehaviour
    {

        [SerializeField]
        float _distanceToPlayer = 5.0f;

        void OnTriggerEnter(Collider other)
        {
            TransparentLogic transparentLogic = other.GetComponentInChildren<TransparentLogic>();
            if (transparentLogic == null) return;

            transparentLogic.FadeOut();
        }

        void OnTriggerExit(Collider other)
        {
            TransparentLogic transparentLogic = other.GetComponentInChildren<TransparentLogic>();
            if (transparentLogic == null) return;

            transparentLogic.FadeIn();
        }

    }
}
