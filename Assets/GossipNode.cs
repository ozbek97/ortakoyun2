using UnityEngine;

[CreateAssetMenu(fileName = "New Gossip", menuName = "Dialogue/Gossip")]
public class GossipNode : ScriptableObject
{
    [TextArea(3, 5)] public string gossipText;
    public CharacterSO aboutCharacter; // Dedikodunun hedefi

    [Header("Mekanikler")]
    [Range(0, 100)] public int credibility = 50; // % kaç ihtimal doğru
    public int stressOnSpread = 20; // Yayınca stres artışı
}