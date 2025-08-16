using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class LightningControl : MonoBehaviour
{
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");

    [Header("Components")] 
    [SerializeField] private Transform beginPoint;
    [SerializeField] private GameObject targetPoint;
    [SerializeField] private LineRenderer lineRendererComponent;
    
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
    private int columns = 2;
    private int rows = 8;
    private int currentFrame;
    private readonly bool distanceBasedDisplacement = true;
    private readonly bool zDisplacement = false;
    
    [Header("Debug")]
    [SerializeField] private bool run;

    private void Awake()
    {
        // Khởi tạo cache
        instanceMaterials = new Material[materials.Length];
    }

    void Start()
    {
        Initialize();
        StartCoroutine(UpdateTiling());
    }
    
    void OnDisable()
    {
        if (lineRendererComponent != null)
            lineRendererComponent.enabled = false;
        run = false;
        StopCoroutine(UpdateTiling());
    }

    void OnEnable()
    {
        if (lineRendererComponent != null)
        {
            lineRendererComponent.enabled = true;
            Initialize();
        } 
        StartCoroutine(UpdateTiling());
    }

    private void Initialize()
    {
        max = rows * columns;

        // add LineRenderer nếu chưa có
        if (lineRendererComponent == null)
        {
            lineRendererComponent = gameObject.GetComponent<LineRenderer>();
            if (lineRendererComponent == null)
                lineRendererComponent = gameObject.AddComponent<LineRenderer>();
        }

        lineRendererComponent.positionCount = points;

        // ✅ lấy instance từ cache
        if (instanceMaterials[currentMaterialIndex] == null)
        {
            instanceMaterials[currentMaterialIndex] = Instantiate(materials[currentMaterialIndex]);
        }
        instanceMaterial = instanceMaterials[currentMaterialIndex];

        // apply material
        lineRendererComponent.material = instanceMaterial;

        // set tile size
        size = new Vector2(1f / columns, 1f / rows);
        instanceMaterial.SetTextureScale(MainTex, size);

        // get offsets array
        GetRandomOffsets();
        run = true;

        lineRendererComponent.sortingLayerName = "3";
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
    
    private void UpdateMaterial(int newRows, int newColumns, Material newMaterial)
    {
        rows = newRows;
        columns = newColumns;
        Initialize();
    }

    private IEnumerator UpdateTiling()
    {
        while (run)
        {
            if (targetPoint != null)
            {
                currentFrame++;
                if (currentFrame >= rows * columns)
                    currentFrame = 0;

                lineRendererComponent.positionCount = points;
                lineRendererComponent.startWidth = lineScale;
                lineRendererComponent.endWidth = lineScale;

                Vector3 startPos = beginPoint.position;
                Vector3 endPos = targetPoint.transform.position;
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

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Mouse0))
    //     {
    //         ChangeMaterial(currentMaterialIndex + 1);
    //     }
    // }

    private void ChangeMaterial(int value)
    {
        if (currentMaterialIndex == value) return;

        currentMaterialIndex = value >= materials.Length ? 0 : value;
        
        switch (currentMaterialIndex)
        {
            case 0:
            case 1:
            case 2:
                HandleMaterialChange(8, 2, materials[currentMaterialIndex]);
                break;
            case 3:
            case 4:
            case 5:
                HandleMaterialChange(16, 2, materials[currentMaterialIndex]);
                break;
            case 6:
            case 7:
            case 8:
                HandleMaterialChange(16, 4, materials[currentMaterialIndex]);
                break;
        }
    }

    private void HandleMaterialChange(int newRows, int newColumns, Material newMaterial)
    {
        UpdateMaterial(newRows, newColumns, newMaterial);
    }
}
