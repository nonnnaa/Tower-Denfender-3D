using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LightningEffect : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float scrollSpeed = 2f;

    private LineRenderer lr;
    private Material mat;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        mat = lr.material;
    }

    void Update()
    {
        // Cập nhật vị trí đầu - cuối
        lr.SetPosition(0, startPoint.position);
        lr.SetPosition(1, endPoint.position);

        // Tạo hiệu ứng trôi texture
        float offset = Time.time * scrollSpeed;
        mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));

        // Có thể thử luôn với tham số shader custom
        // mat.SetTextureOffset("_ZapTex", new Vector2(offset, 0));
    }
}
