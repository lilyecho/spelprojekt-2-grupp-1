using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(TrollBehaviour))]
    public class CustomPointsTrollHandleEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            TrollBehaviour trollBehaviour = (TrollBehaviour) target;

            for (int i = 0; i < trollBehaviour.GetPatrolPoints.Length; i++)
            {
                EditorGUI.BeginChangeCheck();
        
                Vector3 patrolHandlePosition = Handles.PositionHandle(trollBehaviour.GetPatrolPoints[i], Quaternion.identity);
        
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(trollBehaviour, "Change patrolPoint's position");
                    trollBehaviour.GetPatrolPoints[i] = patrolHandlePosition;
                }
            }
        }
    }
}
