// InfiniteSingleBackground.cs
using UnityEngine;

public class InfiniteSingleBackground : MonoBehaviour
{
    [Header("Görsel Ayarları")]
    public Transform background; // Tek görsel
    public float scrollSpeed = 5f;
    
    [Header("Teknik Ayarlar")]
    public float backgroundWidth = 20f;
    public bool autoScroll = true;
    public Camera mainCamera;

    private Vector3 startPosition;
    private float spriteWidth;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (background == null)
            background = transform;

        // Başlangıç pozisyonunu ve genişliği kaydet
        startPosition = background.position;
        
        // Sprite genişliğini hesapla
        if (background.GetComponent<SpriteRenderer>() != null)
        {
            spriteWidth = background.GetComponent<SpriteRenderer>().bounds.size.x;
        }
        else
        {
            spriteWidth = backgroundWidth;
        }

        Debug.Log($"Background width: {spriteWidth}");
    }

    void Update()
    {
        if (autoScroll)
        {
            ScrollBackground();
            CheckAndReposition();
        }
    }

    private void ScrollBackground()
    {
        background.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
    }

    private void CheckAndReposition()
    {
        // Görselin ekranın solundan ne kadar çıktığını hesapla
        float cameraLeftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        float bgRightEdge = background.position.x + (spriteWidth / 2);

        // Görsel tamamen ekranın solundan çıktı mı?
        if (bgRightEdge < cameraLeftEdge)
        {
            RepositionBackground();
        }
    }

    private void RepositionBackground()
    {
        // Yeni pozisyonu hesapla (mevcut pozisyon + genişlik)
        Vector3 newPosition = background.position;
        newPosition.x += spriteWidth * 1f; // Tam genişlik kadar kaydır

        background.position = newPosition;
        Debug.Log($"Background repositioned to: {newPosition}");
    }

    // Hızı değiştirmek için
    public void SetScrollSpeed(float newSpeed)
    {
        scrollSpeed = newSpeed;
    }

    // Debug için
    void OnDrawGizmos()
    {
        if (background != null && mainCamera != null)
        {
            // Görsel sınırlarını göster
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(background.position, new Vector3(spriteWidth, 5, 0));
            
            // Kamera sınırlarını göster
            float cameraHeight = mainCamera.orthographicSize * 2;
            float cameraWidth = cameraHeight * mainCamera.aspect;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(mainCamera.transform.position, new Vector3(cameraWidth, cameraHeight, 0));
        }
    }
}