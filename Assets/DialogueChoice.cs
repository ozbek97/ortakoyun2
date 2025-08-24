using UnityEngine;
[System.Serializable]
public class DialogueChoice
{
    [Tooltip("Oyuncunun göreceği seçenek metni")]
    [TextArea(1, 2)] public string choiceText;

    [Tooltip("Bu seçeneğin götürdüğü sonraki diyalog")]
    public DialogueNode nextNode;

    [Header("Oyun Mekanikleri")]
    [Tooltip("Stres seviyesine etkisi (-/+ değerler)")]
    public int stressChange = 0;

    [Tooltip("Konuşulan karakterle ilişki değişimi")]
    public int relationshipChange = 0;

    [Tooltip("Dedikodu içeriyorsa bu alanı doldur")]
    public GossipNode gossipInfo; // Opsiyonel
}