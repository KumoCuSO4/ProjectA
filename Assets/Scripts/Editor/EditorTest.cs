using System;
using System.Reflection;
using Event;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class EditorTest : EditorWindow
    {
        [MenuItem("Tools/EditorTest")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(EditorTest), false, "My Window");
        }

        void OnGUI()
        {
            if (GUILayout.Button("Click Test"))
            {
                using (EventTest eventTest = new EventTest())
                {
                    eventTest.Test(111);
                }
                EventManager _eventManager = EventManager.Get();
                _eventManager.TriggerEvent(Events.TEST, 345);
                _eventManager.TriggerEvent(Events.TEST, 456);
            }
        }
    }
}