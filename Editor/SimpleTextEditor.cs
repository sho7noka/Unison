using System;

namespace Unison
{
#if UNITY_EDITOR
    using UnityEditor;
    using UnityEngine;

    public class SimpleTextEditor : EditorWindow
    {
        [MenuItem("Examples/GUILayout TextField")]
        static void Init()
        {
            EditorWindow window = GetWindow(typeof(SimpleTextEditor));
            window.Show();
        }

        void OnGUI()
        {
            GUILayout.Label("Select an object in the hierarchy view");
            if (Selection.activeGameObject)
                Selection.activeGameObject.name =
                    EditorGUILayout.TextField("Object Name: ", Selection.activeGameObject.name);
            this.Repaint();
        }
    }
#endif
    
#if GODOT

#endif
    
#if WPF

#endif
}