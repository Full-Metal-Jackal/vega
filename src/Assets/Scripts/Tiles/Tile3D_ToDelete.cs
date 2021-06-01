using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;


// DON'T USE THIS SHIT IT DIDn'T WORK WELL I TELL YOU
// I'll delete this as soon as we get a working n flexible system

[ExecuteAlways]
public class Tile3D_Failed : MonoBehaviour
{
    public Texture texture;
    protected MeshRenderer meshRenderer;

    void Start()
    {
        UpdateTexture();
    }

    void OnValidate()
    {
        UpdateTexture();
    }

    protected virtual void UpdateTexture()
    {
        if (!texture)
        {
            Debug.LogError($"{this} has no texture set.");
            return;
        }
        if (!UpdateMeshRenderer())
        {
            Debug.LogError($"{this} has no mesh renderer in its children.");
            return;
        }

        // Prevent this script running in Prefab Mode; <TODO> needs a separate visualization implementation.
        if (PrefabStageUtility.GetCurrentPrefabStage())
            return;

        if (EditorApplication.isPlaying)
		{
            meshRenderer.material.mainTexture = texture;
        }
        else if (meshRenderer.sharedMaterial)
        {
            // We instantiate the material in Edit Mode in order to prevent the material leak error.
            meshRenderer.sharedMaterial = new Material(meshRenderer.sharedMaterial)
            {
                mainTexture = texture
            };
        }
    }

    protected bool UpdateMeshRenderer() => (meshRenderer = GetComponentInChildren<MeshRenderer>());
}
