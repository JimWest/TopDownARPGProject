using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

namespace TopDown.EntitySystem.Editor
{
    [CustomEditor(typeof(EntityEventTrigger), true)]
    public class EntityEventTriggerEditor : UnityEditor.Editor
    {

        SerializedProperty _DelegatesProperty;

        GUIContent _IconToolbarMinus;
        GUIContent _EventIDName;
        GUIContent[] _EventTypes;
        GUIContent _AddButonContent;

        protected virtual void OnEnable()
        {            
            _DelegatesProperty = serializedObject.FindProperty("_Delegates");
            _AddButonContent = new GUIContent("Add New Event Type");
            _EventIDName = new GUIContent("");
            // Have to create a copy since otherwise the tooltip will be overwritten.
            _IconToolbarMinus = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"));
            _IconToolbarMinus.tooltip = "Remove all events in this list.";

            // find all usable events using reflection and Linq
            string[] eventNames = serializedObject.targetObject.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                .Where(m => m.GetCustomAttributes(typeof(EntityOutputEvent), false).Length > 0)
                .Select(x => x.Name)
                .ToArray();

            _EventTypes = new GUIContent[eventNames.Length];
            for (int i = 0; i < eventNames.Length; ++i)
            {
                _EventTypes[i] = new GUIContent(eventNames[i]);
            }
        }


        public override void OnInspectorGUI()
        {

            //show default inspector for inspector fields
            DrawDefaultInspector();

            serializedObject.Update();
            int toBeRemovedEntry = -1;

            EditorGUILayout.Space();

            Vector2 removeButtonSize = GUIStyle.none.CalcSize(_IconToolbarMinus);

            for (int i = 0; i < _DelegatesProperty.arraySize; ++i)
            {
                SerializedProperty delegateProperty = _DelegatesProperty.GetArrayElementAtIndex(i);
                SerializedProperty callbacksProperty = delegateProperty.FindPropertyRelative("callback");
                SerializedProperty nameProperty = delegateProperty.FindPropertyRelative("eventName");
                _EventIDName.text = nameProperty.stringValue;

                EditorGUILayout.PropertyField(callbacksProperty, _EventIDName);
                Rect callbackRect = GUILayoutUtility.GetLastRect();

                Rect removeButtonPos = new Rect(callbackRect.xMax - removeButtonSize.x - 8, callbackRect.y + 1, removeButtonSize.x, removeButtonSize.y);
                if (GUI.Button(removeButtonPos, _IconToolbarMinus, GUIStyle.none))
                {
                    toBeRemovedEntry = i;
                }

                EditorGUILayout.Space();
            }

            if (toBeRemovedEntry > -1)
            {
                RemoveEntry(toBeRemovedEntry);
            }

            Rect btPosition = GUILayoutUtility.GetRect(_AddButonContent, GUI.skin.button);
            const float addButonWidth = 200f;
            btPosition.x = btPosition.x + (btPosition.width - addButonWidth) / 2;
            btPosition.width = addButonWidth;
            if (GUI.Button(btPosition, _AddButonContent))
            {
                ShowAddTriggermenu();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void RemoveEntry(int toBeRemovedEntry)
        {
            _DelegatesProperty.DeleteArrayElementAtIndex(toBeRemovedEntry);
        }

        void ShowAddTriggermenu()
        {
            // Now create the menu, add items and show it
            GenericMenu menu = new GenericMenu();
            for (int i = 0; i < _EventTypes.Length; ++i)
            {
                bool active = true;

                // Check if we already have a Entry for the current eventType, if so, disable it
                for (int p = 0; p < _DelegatesProperty.arraySize; ++p)
                {
                    SerializedProperty delegateEntry = _DelegatesProperty.GetArrayElementAtIndex(p);
                    SerializedProperty eventProperty = delegateEntry.FindPropertyRelative("eventID");
                    if (eventProperty.intValue == i)
                    {
                        active = false;
                    }
                }
                if (active)
                    menu.AddItem(_EventTypes[i], false, OnAddNewSelected, i);
                else
                    menu.AddDisabledItem(_EventTypes[i]);
            }
            menu.ShowAsContext();
            Event.current.Use();
        }

        private void OnAddNewSelected(object index)
        {
            int selected = (int)index;

            // Store the eventID and name into the m_DelegatesProperty so Execute can find it later
            _DelegatesProperty.arraySize += 1;
            SerializedProperty delegateEntry = _DelegatesProperty.GetArrayElementAtIndex(_DelegatesProperty.arraySize - 1);
            SerializedProperty eventProperty = delegateEntry.FindPropertyRelative("eventID");
            SerializedProperty nameProperty = delegateEntry.FindPropertyRelative("eventName");
            eventProperty.intValue = selected;
            nameProperty.stringValue = _EventTypes[selected].text;
            serializedObject.ApplyModifiedProperties();
        }
    }
}