using UnityEngine;

public class InfiniteBackground3 : MonoBehaviour
{
    [Header("3 Kopya Background")]
    public Transform bg1;
    public Transform bg2;
    public Transform bg3;

    [Header("Ayarlar")]
    public float scrollSpeed = 3f;
    public Camera mainCamera;
    [Tooltip("Kýl payý boþluk/çakýþma düzeltmesi. Pozitif boþluk kapatýr, negatif çakýþmayý azaltýr.")]
    public float widthFix = -0.01f;

    private float tileWidth;

    void Awake()
    {
        if (!mainCamera) mainCamera = Camera.main;
    }

    void Start()
    {
        if (!bg1 || !bg2 || !bg3)
        {
            Debug.LogError("bg1/bg2/bg3 atanmamýþ!");
            enabled = false; return;
        }

        tileWidth = GetWidth(bg1) + widthFix;

        // Yan yana hizala (bg1 solda kalacak þekilde)
        var p = bg1.position;
        bg2.position = new Vector3(p.x + tileWidth, p.y, p.z);
        bg3.position = new Vector3(p.x + 2f * tileWidth, p.y, p.z);
    }

    void Update()
    {
        Vector3 move = Vector3.left * scrollSpeed * Time.deltaTime;
        bg1.Translate(move);
        bg2.Translate(move);
        bg3.Translate(move);

        float camLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x;
        Recycle(bg1, camLeft);
        Recycle(bg2, camLeft);
        Recycle(bg3, camLeft);
    }

    void Recycle(Transform t, float camLeft)
    {
        float rightEdge = t.position.x + tileWidth / 2f;

        if (rightEdge < camLeft)
        {
            // Diðer ikisinin en saðýna at
            float maxX = Mathf.Max(bg1.position.x, Mathf.Max(bg2.position.x, bg3.position.x));
            t.position = new Vector3(maxX + tileWidth, t.position.y, t.position.z);
        }
    }

    float GetWidth(Transform t)
    {
        var sr = t.GetComponent<SpriteRenderer>();
        // bounds.size skale'ý da içerir, bu yüzden doðru geniþlik verir
        return sr ? sr.bounds.size.x : 0f;
    }

    void OnDrawGizmosSelected()
    {
        if (bg1)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(bg1.position, new Vector3(tileWidth, 3f, 0f));
        }
    }
}
