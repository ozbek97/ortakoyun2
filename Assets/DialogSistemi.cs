using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
 
public class DialogSistemi : MonoBehaviour
{
    [Header("Dialog Ayarları")]
    public GameObject dialogKutusu;
    public TextMeshProUGUI dialogText; // TMPro.TextMeshProUGUI
    public List<Dialog> dialogListesi = new List<Dialog>();
    private KarakterHareket karakterHareket;
    private bool dialogAktif = false;
    void Start()
    {
        karakterHareket = GetComponent<KarakterHareket>();
        if (dialogKutusu != null)
            dialogKutusu.SetActive(false);
        // Tüm dialogları sırayla başlat
        foreach (Dialog dialog in dialogListesi)
        {
            StartCoroutine(DialogGoster(dialog.baslangicZamani, dialog.sure, dialog.metin));
        }
    }
    IEnumerator DialogGoster(float beklemeSuresi, float dialogSuresi, string metin)
    {
        // Belirtilen süre kadar bekle
        yield return new WaitForSeconds(beklemeSuresi);
        // Dialogu göster
        dialogAktif = true;
        if (dialogKutusu != null)
            dialogKutusu.SetActive(true);
        if (dialogText != null)
            dialogText.text = metin;
        // Hareketi durdur (isteğe bağlı)
        if (karakterHareket != null)
            karakterHareket.otomatikHareketEtsin = false;
        // Dialog süresi kadar bekle
        yield return new WaitForSeconds(dialogSuresi);
        // Dialogu gizle
        if (dialogKutusu != null)
            dialogKutusu.SetActive(false);
        dialogAktif = false;
        // Hareketi devam ettir
        if (karakterHareket != null)
            karakterHareket.otomatikHareketEtsin = true;
    }
    // Manuel olarak dialog göstermek için
    public void DialogBaslat(float sure, string metin)
    {
        if (!dialogAktif)
        {
            StartCoroutine(DialogGoster(0f, sure, metin));
        }
    }
}