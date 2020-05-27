using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.LowLevel;
using UnityEditor;

// https://www.slideshare.net/UnityTechnologiesJapan/unity-111054310
// https://baba-s.hatenablog.com/entry/2017/12/04/090000#属性
// https://docs.unity3d.com/ScriptReference/PlayerLoop.Initialization.html
// https://docs.unity3d.com/ja/2019.3/Manual/BestPracticeUnderstandingPerformanceInUnity1.html
// https://docs.unity3d.com/ja/current/ScriptReference/MonoBehaviour.html

namespace Unison
{
    public class InterruptUnity
    {
        static PlayerLoopSystem[] originalPlayerLoop, updatedPlayerLoop;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            var playerLoop = PlayerLoop.GetDefaultPlayerLoop();

            originalPlayerLoop = playerLoop.subSystemList[2].subSystemList; // Update

            var subSystem = new List<PlayerLoopSystem>(originalPlayerLoop);
            subSystem.RemoveAll(c => c.type == typeof(FixedUpdate.PhysicsFixedUpdate));
            updatedPlayerLoop = subSystem.ToArray();

            PhysicsOFF();
            EditorApplication.QueuePlayerLoopUpdate();
        }

        public static void PhysicsOFF()
        {
            var playerLoop = PlayerLoop.GetDefaultPlayerLoop();
            var sub = playerLoop.subSystemList[2];
            sub.subSystemList = updatedPlayerLoop;
            playerLoop.subSystemList[2] = sub;
            PlayerLoop.SetPlayerLoop(playerLoop);
        }

        public static void PhysicsON()
        {
            var playerLoop = PlayerLoop.GetDefaultPlayerLoop();
            var sub = playerLoop.subSystemList[2];
            sub.subSystemList = originalPlayerLoop;
            playerLoop.subSystemList[2] = sub;
            PlayerLoop.SetPlayerLoop(playerLoop);
        }
        
        [MenuItem("Assets/Generate Sample Script")]
        public static void GenerateSampleScript()
        {
            // アセットのパスを作成
            var filePath = "Assets/GenerateTest/Sample.cs";

            var assetPath = AssetDatabase.GenerateUniqueAssetPath(filePath);
            EditorApplication.ExecuteMenuItem("");
            AssetDatabase.ImportAsset(assetPath);
            AssetDatabase.Refresh();
        }
    }
}
#endif