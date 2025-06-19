using UnityEngine;

public class TransparentObject : MonoBehaviour
{
    [Header("Transparency Settings")]
    public float transparentAlpha = 0.3f;
    public float fadeSpeed = 2f;
    public float stayTransparentDuration = 1.5f;

    private float currentAlpha = 1f;
    private float timer = 0f;
    private bool isTransparent = false;

    private Material[] materials;

    void Start()
    {
        var renderers = GetComponentsInChildren<MeshRenderer>();
        var mats = new System.Collections.Generic.List<Material>();

        foreach (var renderer in renderers)
        {
            foreach (var mat in renderer.materials)
            {
                URPFadeHelper.SetMaterialTransparent(mat); // Setup for transparency
                mats.Add(mat);
            }
        }

        materials = mats.ToArray();
    }

    public void BecomeTransparent()
    {
        isTransparent = true;
        timer = stayTransparentDuration;
    }

    void Update()
    {
        float targetAlpha = isTransparent ? transparentAlpha : 1f;
        currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime * fadeSpeed);

        foreach (var mat in materials)
        {
            if (mat.HasProperty("_BaseColor"))
            {
                Color c = mat.GetColor("_BaseColor");
                c.a = currentAlpha;
                mat.SetColor("_BaseColor", c);
            }
        }

        if (isTransparent)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
                isTransparent = false;
        }
    }
}
