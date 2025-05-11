using UnityEngine;

[RequireComponent(typeof(Camera))]
public class EffectFilterController : MonoBehaviour
{
    public Material ShaderMaterial;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, ShaderMaterial);
    }
}