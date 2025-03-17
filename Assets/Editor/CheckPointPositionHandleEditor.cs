using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(CheckPointBehaviour))]
    public class CheckPointPositionHandleEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            CheckPointBehaviour checkPointBehaviour = (CheckPointBehaviour)target;
        
            EditorGUI.BeginChangeCheck();
        
            Vector3 newSpawnPoint = Handles.PositionHandle(checkPointBehaviour.SpawnPoint, Quaternion.identity);
        
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(checkPointBehaviour, "Change spawnPoint's position");
                checkPointBehaviour.SpawnPoint = newSpawnPoint;

            }
        }
    }
}
