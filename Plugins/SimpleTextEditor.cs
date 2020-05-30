using System;
using System.Reflection;
using Unison.Bind;
using RPC = Unison.Bind.RPC;

#if UNITY_EDITOR  
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

/// <summary>
/// 
/// </summary>
public class PythonExtension
{
    [MenuItem("Assets/OpenInScriptEditor")]
    private static void OpenEditor()
    {
        var path = AssetDatabase.GUIDToAssetPath(Selection.activeGameObject.name);
        var m_codeContents = System.IO.File.ReadAllText(path);

        // NOTE: using UnityEditor.Scripting.Python;
        var fields = typeof(PythonConsoleWindow).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var field in fields)
        {
            if (field.Name == "m_textFieldCode")
                field.SetValue("value", m_codeContents);
        }
    }
}

namespace UnityEngine.Scripting.Command
{
    /// <summary>
    /// 
    /// </summary>
    public class Execute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        public static void ConsoleCommand(string cmd)
        {
            Console.Write(cmd);
        }
    }
}

/// <summary>
/// for Unity
/// </summary>
public class SimpleTextEditor : EditorWindow
{
    [MenuItem("Examples/GUILayout TextField")]
    private static void Init()
    {
        var window = GetWindow(typeof(SimpleTextEditor));
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Select an object in the hierarchy view");
        if (Selection.activeGameObject)
            Selection.activeGameObject.name =
                EditorGUILayout.TextField("Object Name: ", Selection.activeGameObject.name);
        Repaint();
    }

    [DidReloadScripts(1)]
    private static void OnDidReloadScripts()
    {
        // PyInterpreter.Client();
        RPC.Server();
        RPC.Server();
        PythonBinder.Gen().Compile("Client.dll");
        // https://baba-s.hatenablog.com/entry/2017/12/04/090000#スクリプトが読み込まれた時
    }
}

/// <summary>
/// 
/// </summary>
public class SaveHook : AssetModificationProcessor
{
    private static bool in_python = false;

    private void OnWillSaveAssets(string[] paths)
    {
        foreach (var path in paths)
        {
            if (path.EndsWith(".py"))
            {
                in_python = true;
                break;
            }

            if (path.EndsWith(".cs"))
            {
                RPC.Server("localhost", 8888);
                break;
            }
        }

        if (in_python)
            AssetDatabase.Refresh();
    }
}
#endif

#if GODOT
    using Godot;

    /// <summary>
    /// for Godot
    /// </summary>
    public class SimpleTextEditor
    {

    }
#endif

#if WPF
    /// <summary>
    /// for Wpf
    /// </summary>
    public class SimpleTextEditor
    {

    }
#endif