using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject // ScriptableObject zaten serializable olduğu için [Serializable] gerekmez
{
    [Header("Diyalog İçeriği")]
    [TextArea(3, 5)] public string text;
    public CharacterSO character;

    [Header("Seçenekler")]
    public DialogueChoice[] choices; // DialogueChoice sınıfı zaten serializable

    [Header("Özel Durumlar")]
    public bool isGossipReveal;
}