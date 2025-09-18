using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DisruptorLightningControl : MonoBehaviour
{
    private LineRenderer mainLine;
    private LineRenderer glowLine;
    private List<Transform> targets;
    private PoolableObject poolObj;

    [Header("Lightning Settings")]
    [SerializeField] private float lifeTime = 0.4f;
    [SerializeField] private float noiseStrength = 1.5f;
    [SerializeField] private float widthMultiplier = 4f;
    [SerializeField] private int segmentsPerTarget = 10;

    [Header("Materials")]
    [SerializeField] private Material mainMaterial;
    [SerializeField] private Material glowMaterial;

    private float timer;

    private void Awake()
    {
        poolObj = GetComponent<PoolableObject>();

        // Line chính
        mainLine = GetComponent<LineRenderer>();
        SetupLineRenderer(mainLine, mainMaterial, widthMultiplier);

        // Glow line
        GameObject glowObj = new GameObject("GlowLine");
        glowObj.transform.SetParent(transform, false);
        glowLine = glowObj.AddComponent<LineRenderer>();
        SetupLineRenderer(glowLine, glowMaterial, widthMultiplier * 2f);
    }

    private void SetupLineRenderer(LineRenderer lr, Material mat, float width)
    {
        lr.positionCount = 0;
        lr.material = mat;
        lr.widthMultiplier = width;
        lr.textureMode = LineTextureMode.Tile;
        lr.alignment = LineAlignment.View;

        lr.widthCurve = new AnimationCurve(
            new Keyframe(0f, 1f),
            new Keyframe(0.5f, 1.2f),
            new Keyframe(1f, 1f)
        );

        lr.colorGradient = CreateGradient();
    }

    private Gradient CreateGradient()
    {
        Gradient g = new Gradient();
        g.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(Color.white, 0f),
                new GradientColorKey(new Color(0.3f, 0.8f, 1f), 0.7f),
                new GradientColorKey(new Color(0.1f, 0.3f, 0.9f), 1f)
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(0f, 1f)
            }
        );
        return g;
    }

    public void SetTargets(List<Transform> newTargets, PoolableObject pooled)
    {
        targets = newTargets;
        poolObj = pooled; // lưu reference pool object
        UpdateLine();
        timer = lifeTime;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (targets == null || targets.Count == 0) return;

        UpdateLine();

        // Fade out
        timer -= Time.deltaTime;
        float alpha = Mathf.Clamp01(timer / lifeTime);

        ApplyFade(mainLine, alpha);
        ApplyFade(glowLine, alpha * 0.5f);

        if (timer <= 0f)
        {
            // trả lại Pool
            PoolManager.Instance.ReleaseToPool(CONSTANT.ProjectileName.ProjectileDisruptorBullet, poolObj);
        }
    }

    private void ApplyFade(LineRenderer lr, float alpha)
    {
        Gradient g = lr.colorGradient;
        GradientAlphaKey[] aKeys = g.alphaKeys;
        for (int i = 0; i < aKeys.Length; i++)
            aKeys[i].alpha = alpha;
        g.alphaKeys = aKeys;
        lr.colorGradient = g;
    }

    private void UpdateLine()
    {
        if (targets == null || targets.Count == 0) return;

        int totalSegments = targets.Count * segmentsPerTarget;
        mainLine.positionCount = totalSegments + 1;
        glowLine.positionCount = totalSegments + 1;

        Vector3 startPos = transform.position;
        int index = 0;
        mainLine.SetPosition(index, startPos);
        glowLine.SetPosition(index++, startPos);

        foreach (Transform target in targets)
        {
            if (target == null) continue;

            Vector3 endPos = target.position;
            for (int s = 1; s <= segmentsPerTarget; s++)
            {
                float t = (float)s / segmentsPerTarget;
                Vector3 pos = Vector3.Lerp(startPos, endPos, t);

                // jitter mạnh hơn ở giữa
                float jitter = noiseStrength * Mathf.Sin(t * Mathf.PI);
                pos += new Vector3(
                    Random.Range(-jitter, jitter),
                    Random.Range(-jitter, jitter),
                    Random.Range(-jitter, jitter)
                );

                mainLine.SetPosition(index, pos);
                glowLine.SetPosition(index++, pos);
            }

            startPos = endPos;
        }
    }
}
