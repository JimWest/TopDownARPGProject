using UnityEngine;

namespace TopDown.EntitySystem
{
    public class UseableLogic : EntityEventTrigger, IUseable
    {
        [SerializeField]
        protected bool _oneTimeActivation = true;

        bool _activated = false;
        public bool Activated
        {
            get
            {
                return _activated;
            }
            protected set
            {
                _activated = value;
            }
        }

        [EntityOutputEvent]
        public virtual void OnUse(GameObject user)
        {
            if (CanBeUsed(user))
            {
                Execute("OnUse");
                Activated = !Activated;
                if (Activated)
                    OnActivate();
                else
                    OnDeactivate();
            }
        }

        [EntityOutputEvent]
        public virtual void OnActivate()
        {
            Execute("OnActivate");
        }

        [EntityOutputEvent]
        public virtual void OnDeactivate()
        {
            Execute("OnDeactivate");
        }


        public virtual bool CanBeUsed(GameObject user)
        {
            bool canBeUsed = true;
            if (_oneTimeActivation)
            {
                canBeUsed = !Activated;
            }

            return enabled && canBeUsed;
        }
    }
}
