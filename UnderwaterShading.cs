using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UnderwaterShading : MonoBehaviour
{
    public Shader currentShader;
    public Color waveColor1 = Color.white;
    public Color waveColor2 = Color.blue;
    public float waveSpeed = 1f;
    public float alpha = 1f;
    private Material currentMaterial = null;
    public Material material = null;
    /*Material material
    {
        get
        {
            if (currentMaterial == null)
            {
                currentMaterial = new Material(currentShader);
                currentMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return currentMaterial;
        }
    }*/
    void Start()
    {
        if (!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }
        if (!currentShader && !currentShader.isSupported)
        {
            enabled = false;
        }
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        
        if (currentShader != null)
        {
            material.SetFloat("_WaveSpeed", waveSpeed);
            material.SetColor("_Color1", waveColor1);
            material.SetColor("_Color2", waveColor2);
            Graphics.Blit(src, dest, currentMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }

    void Update()
    {
        waveSpeed = Mathf.Clamp(waveSpeed, 0f, 10f);
        alpha = Mathf.Clamp(alpha, -1f, 10f);
    }

    void OnDisable()
    {
        if (currentMaterial)
            DestroyImmediate(currentMaterial);
    }
}
