using UnityEngine;

public class KarakterHareket : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float yurumeHizi = 3f;
    public bool otomatikHareketEtsin = true;
    
    private void Update()
    {
        if (otomatikHareketEtsin)
        {
            // Otomatik olarak sağa doğru hareket
            transform.Translate(Vector3.right * yurumeHizi * Time.deltaTime);
        }
        else
        {
            // Manuel kontrol (isteğe bağlı)
            float yatayHareket = Input.GetAxis("Horizontal");
            transform.Translate(Vector3.right * yatayHareket * yurumeHizi * Time.deltaTime);
        }
    }
    
    // Hızı değiştirmek için metod
    public void HiziDegistir(float yeniHiz)
    {
        yurumeHizi = yeniHiz;
    }
}