using System;
using UnityEngine;

[ExecuteInEditMode]
public class BillboardEffect : MonoBehaviour
{
    [NonSerialized] public Camera targetCamera;
    [SerializeField] private bool lockYAxis = true; // Option to lock Y-axis rotation
    
    void OnEnable()
    {
        // Default to scene camera if none assigned
        if (targetCamera == null)
        {
            if (Application.isPlaying)
                targetCamera = Camera.main;
            #if UNITY_EDITOR
            else if (UnityEditor.SceneView.lastActiveSceneView != null)
                targetCamera = UnityEditor.SceneView.lastActiveSceneView.camera;
            #endif
        }
    }
    
    void LateUpdate() // Using LateUpdate ensures this happens after all Updates
    {
        if (targetCamera == null) return;
        
        // Full billboard effect - always face camera directly
        transform.forward = targetCamera.transform.forward * -1; // Point toward camera
        
        // If we need to lock the Y-axis rotation (keep sprite upright)
        if (lockYAxis)
        {
            // Keep the original y-rotation but adopt camera rotation for x and z
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.y = 0;
            transform.eulerAngles = eulerAngles;
        }
    }
    
    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!Application.isPlaying && targetCamera == null && UnityEditor.SceneView.lastActiveSceneView != null)
        {
            transform.forward = UnityEditor.SceneView.lastActiveSceneView.camera.transform.forward * -1;
            
            if (lockYAxis)
            {
                Vector3 eulerAngles = transform.eulerAngles;
                eulerAngles.y = 0;
                transform.eulerAngles = eulerAngles;
            }
        }
    }
    #endif
}