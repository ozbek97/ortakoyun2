using UnityEngine;

[System.Serializable]
public class Dialog
{
    public float baslangicZamani; // Oyun başladıktan kaç saniye sonra
    public float sure; // Kaç saniye gözükecek
    [TextArea(3, 5)]
    public string metin;
}