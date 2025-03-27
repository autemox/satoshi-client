using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Renderer))]
public class AutoTiling : MonoBehaviour
{
    public Vector2 textureScale = Vector2.one;

    void OnValidate()
    {
        UpdateTiling();
    }

    void Start()
    {
        UpdateTiling();
    }

    void UpdateTiling()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null && renderer.sharedMaterial != null)
        {
            Vector3 scale = transform.localScale;
            Vector2 adjustedTiling = new Vector2(scale.x * textureScale.x, scale.z * textureScale.y);
            renderer.sharedMaterial.mainTextureScale = adjustedTiling;
        }
    }
}