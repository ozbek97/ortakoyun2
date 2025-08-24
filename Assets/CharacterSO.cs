using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Dialogue/Character")]
public class CharacterSO : ScriptableObject
{
    [Header("Temel Bilgiler")]
    public string characterName;
    public Sprite portrait;
    public AudioClip voiceSound; // Opsiyonel ses efekti

    [Header("İlişki Sistemi")]
    [Tooltip("-100 (Düşman) ile +100 (Dost) arası")]
    [Range(-100, 100)] public int initialRelationship = 0;
    
    [TextArea(2, 4)] public string description;
}