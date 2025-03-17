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

            Vector3 trollPos = trollBehaviour.transform.position;
            
            for (int i = 0; i < trollBehaviour.patrolPoints.Length; i++)
            {
                EditorGUI.BeginChangeCheck();
        
                Vector3 patrolHandlePosition = Handles.PositionHandle(trollBehaviour.patrolPoints[i]+trollPos, Quaternion.identity);
        
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(trollBehaviour, "Change patrolPoint's position");
                    trollBehaviour.patrolPoints[i] = patrolHandlePosition-trollPos;
                }
            }
        }
    }
}
