using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Test
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
                MethodInfo methodInfo = typeof(TestClass).GetMethod("MethodTest", BindingFlags.Instance | BindingFlags.Public);
                object[] objects = { 1, 2 };
                methodInfo.Invoke(new TestClass(), objects);
            }
        }
    }

    public class TestClass
    {
        public void MethodTest(int a, int b)
        {
            LogManager.Log(a,b);
        }
    }
}