// CameraFollowX.cs
using UnityEngine;

public class CameraFollowX : MonoBehaviour
{
    [Header("Takip Hedefi")]
    public Transform target; // Takip edilecek karakter/nesne
    
    [Header("Takip Ayarları")]
    public float followSpeed = 5f;
    public bool smoothFollow = true;
    public float xOffset = 0f; // X ekseninde offset
    
    [Header("Sınırlamalar")]
    public bool useBounds = false;
    public float minX = -10f;
    public float maxX = 10f;
    
    private Vector3 initialOffset;
    private Vector3 targetPosition;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Takip hedefi atanmamış!");
            return;
        }
        
        // Başlangıç offset'ini kaydet
        initialOffset = transform.position - target.position;
    }

    void LateUpdate()
    {
        if (target == null) return;
        
        FollowTarget();
    }

    private void FollowTarget()
    {
        // Hedef pozisyonu hesapla
        targetPosition = target.position + initialOffset;
        targetPosition.y = transform.position.y; // Y eksenini sabit tut
        targetPosition.z = transform.position.z; // Z eksenini sabit tut
        targetPosition.x += xOffset; // X offset ekle

        // Sınırlamaları uygula
        if (useBounds)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        }

        // Takip işlemi
        if (smoothFollow)
        {
            // Yumuşak takip (Lerp)
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
        else
        {
            // Anında takip
            transform.position = targetPosition;
        }
    }

    // Kamera sınırlarını değiştirmek için
    public void SetCameraBounds(float newMinX, float newMaxX)
    {
        minX = newMinX;
        maxX = newMaxX;
        useBounds = true;
    }

    // Offset'i dinamik olarak değiştirmek için
    public void SetXOffset(float newOffset)
    {
        xOffset = newOffset;
    }

    // Takip hızını değiştirmek için
    public void SetFollowSpeed(float newSpeed)
    {
        followSpeed = newSpeed;
    }

    // Debug için kamera sınırlarını göster
    void OnDrawGizmosSelected()
    {
        if (useBounds)
        {
            Gizmos.color = Color.yellow;
            Vector3 minPos = new Vector3(minX, transform.position.y - 10, transform.position.z);
            Vector3 maxPos = new Vector3(maxX, transform.position.y + 10, transform.position.z);
            Gizmos.DrawLine(minPos, maxPos);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector3(minX, transform.position.y, transform.position.z), new Vector3(0.5f, 20f, 0.5f));
            Gizmos.DrawWireCube(new Vector3(maxX, transform.position.y, transform.position.z), new Vector3(0.5f, 20f, 0.5f));
        }
    }
}