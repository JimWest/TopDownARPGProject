using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

#if UNITY_EDITOR 
using UnityEditor;
#endif

namespace TopDown.EntitySystem
{
    public class EntityOutputEvent : Attribute { }

    [Serializable]
    public class TriggerEvent : UnityEvent { }

    [Serializable]
    public class Entry
    {
        public int eventID;
        public string eventName;
        public TriggerEvent callback = new TriggerEvent();
    }

    public abstract class EntityEventTrigger : MonoBehaviour
    {
        [SerializeField]
        bool _visualizeConnections = true;

        [HideInInspector]
        [SerializeField]
        private List<Entry> _Delegates;

        [HideInInspector]
        public List<Entry> Triggers
        {
            get
            {
                if (_Delegates == null)
                    _Delegates = new List<Entry>();
                return _Delegates;
            }
            set { _Delegates = value; }
        }

#if UNITY_EDITOR
        // function to create a gameobject with and added EntityEventTrigger
        protected static GameObject CreateGameObject<T>(MenuCommand menuCommand) where T : EntityEventTrigger
        {
            GameObject go = new GameObject(typeof(T).Name);
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            go.AddComponent<T>();
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
            return go;
        }
#endif

        protected void Execute(int id)
        {
            for (int i = 0, imax = Triggers.Count; i < imax; ++i)
            {
                var ent = Triggers[i];
                if (ent.eventID == id && ent.callback != null)
                    ent.callback.Invoke();
            }
        }

        protected void Execute(string methodName)
        {
            for (int i = 0, imax = Triggers.Count; i < imax; ++i)
            {
                var ent = Triggers[i];
                if (ent.eventName == methodName && ent.callback != null)
                    ent.callback.Invoke();
            }
        }


        [EntityOutputEvent]
        protected virtual void OnEnable()
        {
            Execute("OnEnable");
        }

        [EntityOutputEvent]
        protected virtual void OnDisable()
        {
            Execute("OnDisable");
        }

        [EntityOutputEvent]
        protected virtual void Start()
        {
            Execute("Start");
        }

        [EntityOutputEvent]
        protected virtual void OnDestroy()
        {
            Execute("OnDestroy");
        }

        [EntityOutputEvent]
        protected virtual void OnBecameVisible()
        {
            Execute("OnBecameVisible");
        }

        [EntityOutputEvent]
        protected virtual void OnBecameInvisible()
        {
            Execute("OnBecameInvisible");
        }

        [EntityOutputEvent]
        protected virtual void OnUpdate()
        {
            Execute("OnUpdate");
        }

        [EntityOutputEvent]
        protected virtual void OnFixedUpdate()
        {
            Execute("OnFixedUpdate");
        }

        [EntityOutputEvent]
        protected virtual void OnLateUpdate()
        {
            Execute("OnLateUpdate");
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (!_visualizeConnections)
                return;

            DrawGizmos(false);
        }


        void OnDrawGizmosSelected()
        {
            if (!_visualizeConnections)
                return;

            DrawGizmos(true);
        }


        void DrawGizmos(bool selected)
        {
            Gizmos.DrawIcon(transform.position, @"..\TopDown\Textures\Gizmos\testGizmo.png");

            if (!_visualizeConnections)
                return;

            // selected objects have a bit more brighter color
            Color lineColor = selected ? Color.yellow : new Color(1, 1, 0, 0.5f);
            Gizmos.color = lineColor;

            // show a line to all Targets
            for (int i = 0, imax = Triggers.Count; i < imax; ++i)
            {
                var ent = Triggers[i];
                if (ent.callback != null)
                {
                    for (int j = 0, jmax = ent.callback.GetPersistentEventCount(); j < jmax; ++j)
                    {
                        var target = ent.callback.GetPersistentTarget(j);
                        var method = ent.callback.GetPersistentMethodName(j);
                        if (target != null && !target.Equals("null") && method != null && !method.Equals(""))
                        {
                            Vector3 endPoint = ((Component)target).transform.position;

                            // make a loop if the target is itself
                            if ((endPoint - transform.position).magnitude < 0.1f)
                            {
                                Vector3 startTangent = transform.position + Vector3.right * 2;
                                Vector3 endTangent = transform.position + Vector3.forward * 2;
                                Handles.DrawBezier(transform.position, endPoint, startTangent, endTangent, lineColor, null, 1f);
                            }
                            else
                            {
                                // just make a line
                                Gizmos.DrawLine(transform.position, ((Component)target).transform.position);
                            }                            

                        }
                    }
                }

            }
        }
#endif


    }
}
