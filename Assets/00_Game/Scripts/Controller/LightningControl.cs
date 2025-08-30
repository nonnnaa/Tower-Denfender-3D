using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class LightningControl : MonoBehaviour
{
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");

    [Header("Components")] 
    [SerializeField] private Transform beginPoint;
    [SerializeField] private Transform targetPoint;
    [SerializeField] private LineRenderer lineRendererComponent;
    [SerializeField] private Transform impactLightningTransform;
    
    [Header("Lightning Settings")]
    [SerializeField] private Material[] materials;
    [SerializeField] private bool randomize;
    [SerializeField ,Range(24, 60)] private int framesPerSecond = 10;
    [SerializeField ,Range(2, 15)] private int points = 5;
    [SerializeField ,Range(0, 1)] private float pointsDisplacement = 0.1f;
    [SerializeField ,Range(1, 10)] private float lineScale = 1f;
    
    // Cache
    private Vector2[] offsets;
    private Vector2 size;
    [SerializeField] private int currentMaterialIndex;
    private Material instanceMaterial;
    private Material[] instanceMaterials; // cache các instance
    private int max; 
    //[SerializeField] 
    private int columns = 2;
    //[SerializeField] 
    private int rows = 16;
    private int currentFrame;
    private readonly bool distanceBasedDisplacement = true;
    private readonly bool zDisplacement = false;
    
    [Header("Debug")]
    [SerializeField] private bool run;

    
    
    private void Awake()
    {
        instanceMaterials = new Material[materials.Length];
    }

    private void OnEnable()
    {
        Initialize();
        StartCoroutine(UpdateTiling());
        if (lineRendererComponent != null)
            lineRendererComponent.enabled = true;
        targetPoint.SetParent(transform);
    }

    void OnDisable()
    {
        if (lineRendererComponent != null)
            lineRendererComponent.enabled = false;
        run = false;
        StopCoroutine(UpdateTiling());
    }

    public void SetTarget(Transform newTarget)
    {
        // if (Physics.Raycast(transform.position, (newTarget.position - transform.position).normalized, 
        //         out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask($"Enemy")))
        // {
           // targetPoint.position = hit.point;
           // targetPoint.LookAt(newTarget);
           
           targetPoint.position = newTarget.position;
           targetPoint.rotation = newTarget.rotation;
           targetPoint.SetParent(newTarget);
        //}
    }

    private void Initialize()
    {
        lineRendererComponent.positionCount = points;
        lineRendererComponent.sortingLayerName = "3";
        instanceMaterials[currentMaterialIndex] = Instantiate(materials[currentMaterialIndex]);
        instanceMaterial = instanceMaterials[currentMaterialIndex];
        run = true;
        ChangeMaterial(currentMaterialIndex);
    }

    private void GetRandomOffsets()
    {
        offsets = new Vector2[rows * columns];
        for (int i = 0; i < max; i++)
        {
            int col = i % columns;
            int row = i / columns;
            offsets[i] = new Vector2(col / (float)columns, row / (float)rows);
        }
    }
    
    private void UpdateMaterial()
    {
        max = rows * columns;
        if (instanceMaterials[currentMaterialIndex] == null)
        {
            instanceMaterials[currentMaterialIndex] = Instantiate(materials[currentMaterialIndex]);
        }
        instanceMaterial = instanceMaterials[currentMaterialIndex];
        lineRendererComponent.material = instanceMaterial;
        size = new Vector2(1f / columns, 1f / rows);
        instanceMaterial.SetTextureScale(MainTex, size);
        GetRandomOffsets();
    }

    private IEnumerator UpdateTiling()
    {
        while (run)
        {
            if (targetPoint != null)
            {
                if (Physics.Raycast(transform.position, (targetPoint.position - transform.position).normalized, 
                        out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask($"Enemy")))
                {
                    impactLightningTransform.position = hit.point;
                    impactLightningTransform.LookAt(transform);
                }
                currentFrame++;
                if (currentFrame >= rows * columns)
                    currentFrame = 0;

                lineRendererComponent.positionCount = points;
                lineRendererComponent.startWidth = lineScale;
                lineRendererComponent.endWidth = lineScale;

                Vector3 startPos = beginPoint.position;
                Vector3 endPos = targetPoint.position;
                lineRendererComponent.SetPosition(0, startPos);
                lineRendererComponent.SetPosition(points - 1, endPos);

                if (points >= 3)
                {
                    for (int i = 1; i < points - 1; i++)
                    {
                        float scale = (float)i / (points - 1);
                        var pos = Vector3.Lerp(startPos, endPos, scale);

                        if (distanceBasedDisplacement)
                        {
                            float distance = Vector3.Distance(startPos, endPos);
                            distance = distance * pointsDisplacement / points;
                            pos.y += Random.Range(-distance, distance);
                            pos.x += Random.Range(-distance, distance);
                            if (zDisplacement) pos.z += Random.Range(-distance, distance);
                        }
                        else
                        {
                            pos.y += Random.Range(-pointsDisplacement, pointsDisplacement);
                            pos.x += Random.Range(-pointsDisplacement, pointsDisplacement);
                            if (zDisplacement) pos.z += Random.Range(-pointsDisplacement, pointsDisplacement);
                        }
                        lineRendererComponent.SetPosition(i, pos);
                    }
                }

                if (randomize)
                {
                    if (Random.Range(0, 2) == 0) // fix: trả về 0 hoặc 1
                    {
                        size.y *= -1;
                        if (Random.Range(0, 2) == 0)
                            size.x *= -1;
                        instanceMaterial.SetTextureScale(MainTex, size);
                    }
                    Vector2 offset = offsets[Random.Range(0, max)];
                    instanceMaterial.SetTextureOffset(MainTex, offset);
                }
                else
                {
                    int col = currentFrame % columns;
                    int row = currentFrame / columns;
                    Vector2 offset = new Vector2(col / (float)columns, row / (float)rows);
                    instanceMaterial.SetTextureOffset(MainTex, offset);
                }

                lineRendererComponent.sortingLayerName = "3";
                lineRendererComponent.sortingOrder = 500;

                yield return new WaitForSeconds(1f / framesPerSecond);
            }
            else
            {
                yield return new WaitForSeconds(1f / framesPerSecond);
            }
        }
    }

    private void ChangeMaterial(int value)
    {
        currentMaterialIndex = value;

        switch (currentMaterialIndex)
        {
            case 0:
            case 1:
            case 2:
                rows = 8;
                columns = 2;
                break;
            case 3:
            case 4:
            case 5:
                rows = 16;
                columns = 2;
                break;
            case 6:
            case 7:
            case 8:
                rows = 16;
                columns = 4;
                break;
        }
        UpdateMaterial();
    }
}
