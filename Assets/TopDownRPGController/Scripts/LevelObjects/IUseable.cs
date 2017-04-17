using UnityEngine;
using System.Collections;

namespace TopDown
{
    public interface IUseable
    {

        void OnUse(GameObject user);
        bool CanBeUsed(GameObject user);
    } 
}
