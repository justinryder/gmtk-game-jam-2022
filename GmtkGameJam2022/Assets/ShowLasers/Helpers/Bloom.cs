using System;
using UnityEngine;

/// <summary>
/// Bloom effect adapted by Jasper Flick of Catlike Coding
/// + added Single Pass Stereo support for VR
/// See https://catlikecoding.com/unity/tutorials/advanced-rendering/bloom/ for more details.
/// 
/// </summary>
[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class Bloom : MonoBehaviour {

    private const int PASS_DOWN_PRE_FILTER = 0;
    private const int PASS_DOWN = 1;
    private const int PASS_UP = 2;
    private const int PASS_APPLY_BLOOM = 3;

    [Range(0, 10)]
    public float intensity = 1;

    [Range(1, 16)]
    public int iterations = 4;

    [Range(0, 10)]
    public float threshold = 1;

    [Range(0, 1)]
    public float softThreshold = 0.5f;

    private readonly RenderTexture[] textures = new RenderTexture[16];

    [NonSerialized]
    private Material bloom;

    private Shader bloomShader;

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {

        if (bloom == null) {
            bloomShader = Shader.Find("SC Lasers/Bloom");
            bloom = new Material(bloomShader);
            bloom.hideFlags = HideFlags.HideAndDontSave;
        }

        float knee = threshold * softThreshold;
        Vector4 filter;
        filter.x = threshold;
        filter.y = filter.x - knee;
        filter.z = 2f * knee;
        filter.w = 0.25f / (knee + 0.00001f);
        bloom.SetVector("_Filter", filter);
        bloom.SetFloat("_Intensity", Mathf.GammaToLinearSpace(intensity));

        int width = source.width / 2;
        int height = source.height / 2;
        RenderTextureFormat format = source.format;

        RenderTexture currentDestination = textures[0] = RenderTexture.GetTemporary(width, height, 0, format, RenderTextureReadWrite.Default, 1, RenderTextureMemoryless.None, source.vrUsage);
        Graphics.Blit(source, currentDestination, bloom, PASS_DOWN_PRE_FILTER);
        RenderTexture currentSource = currentDestination;

        int i = 1;
        for (; i < iterations; i++) {
            width /= 2;
            height /= 2;
            if (height < 2) {
                break;
            }
            currentDestination = textures[i] = RenderTexture.GetTemporary(width, height, 0, format, RenderTextureReadWrite.Default, 1, RenderTextureMemoryless.None, source.vrUsage);
            Graphics.Blit(currentSource, currentDestination, bloom, PASS_DOWN);
            currentSource = currentDestination;
        }

        for (i -= 2; i >= 0; i--) {
            currentDestination = textures[i];
            textures[i] = null;
            Graphics.Blit(currentSource, currentDestination, bloom, PASS_UP);
            RenderTexture.ReleaseTemporary(currentSource);
            currentSource = currentDestination;
        }

        bloom.SetTexture("_SourceTex", source);
        Graphics.Blit(currentSource, destination, bloom, PASS_APPLY_BLOOM);

        RenderTexture.ReleaseTemporary(currentSource);
    }
}