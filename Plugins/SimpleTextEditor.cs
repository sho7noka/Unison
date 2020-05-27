using System;
using System.Reflection;

namespace Unison
{

#if UNITY_EDITOR    
    using UnityEditor;
    using UnityEngine;
    using UnityEditor.Scripting.Python;

    /// <summary>
    /// 
    /// </summary>
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

        [MenuItem("Assets/OpenInScriptEditor")]
        private static void OpenEditor()
        {
            var path = AssetDatabase.GUIDToAssetPath(Selection.activeGameObject.name);
            var m_codeContents = System.IO.File.ReadAllText(path);
            
            var fields = typeof(PythonConsoleWindow).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (field.Name == "m_textFieldCode")
                    field.SetValue("value", m_codeContents);
            }
        }  
    }

    /// <summary>
    /// 
    /// </summary>
    public class SaveHook : UnityEditor.AssetModificationProcessor
    {
        private static bool in_python = false;
        private static string[] OnWillSaveAssets(string[] paths)
        {
            foreach (var path in paths)
            {                
                if (path.EndsWith(".py"))
                    in_python = true;
            }
            if (in_python)
                AssetDatabase.Refresh();
            return paths;
        }

        private static void OnStatusUpdated()
        {
            
        }
    }
#endif
    
#if GODOT
    using Godot;

    public class SimpleTextEditor
    {

    }
#endif
    
#if WPF

    public class SimpleTextEditor
    {

    }
#endif
    
}