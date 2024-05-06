using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyAI))]
public class EnemyAIEditor : Editor
{
    private void OnSceneGUI()
    {
        if (EditorApplication.isPlaying)
        {
            EnemyAI fov = (EnemyAI)target;
            float visionDist = fov.enemyType == EnemyAI.EnemyType.Blind ? fov.currentState.visDist / 1.3f : fov.currentState.visDist;
            // Draw Enemy Sight Range
            Handles.color = Color.white;
            Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, visionDist);

            Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.currentState.visAngle / 2);
            Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.currentState.visAngle / 2);

            Handles.color = Color.yellow;
            Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * visionDist);
            Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * visionDist);

            if (fov.currentState.CanSeePlayer())
            {
                Handles.color = Color.green;
                Handles.DrawLine(fov.transform.position, fov.player.transform.position);
            }
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}