using Characters.Enemy.Troll.Scripts.States;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(TrollBehaviour))]
    public class CustomPointsTrollHandleEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            if (Application.isEditor)
            {
                TrollBehaviour trollBehaviour = (TrollBehaviour) target;

                Vector3 startPos = Vector3.zero;
                if (!Application.isPlaying)
                {
                    startPos = trollBehaviour.transform.position;
                }
                else
                {
                    startPos = trollBehaviour.StartPos;
                }
                
            
                for (int i = 0; i < trollBehaviour.LocalPatrolPoints.Length; i++)
                {
                    EditorGUI.BeginChangeCheck();
        
                    Vector3 patrolHandlePosition = Handles.PositionHandle(startPos+trollBehaviour.LocalPatrolPoints[i], Quaternion.identity);
        
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(trollBehaviour, "Change patrolPoint's position");
                        trollBehaviour.LocalPatrolPoints[i] = patrolHandlePosition-startPos;
                    }
                }
            }
        }
    }
}
