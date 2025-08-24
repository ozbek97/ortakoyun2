using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text dialogueText;
    public Image characterPortrait;
    public Transform choicesParent;
    public GameObject choiceButtonPrefab;
    public Slider stressMeter;
    public TMP_Text relationshipText;
    public GameObject gossipPanel;
    public CharacterSO defaultCharacter;

    [Header("Game Data")]
    [Range(0, 100)] public int currentStress = 50;
    private Dictionary<CharacterSO, int> _relationships = new Dictionary<CharacterSO, int>();
    private DialogueNode _currentNode;
    private List<GameObject> _activeChoiceButtons = new List<GameObject>();

    void Start()
    {
        // Null kontrolleri
        if (stressMeter == null) Debug.LogError("StressMeter not assigned!");
        if (relationshipText == null) Debug.LogError("RelationshipText not assigned!");
        
        UpdateStressUI();
        if (gossipPanel) gossipPanel.SetActive(false);
    }

    public void StartDialogue(DialogueNode startNode)
    {
        if (startNode == null)
        {
            Debug.LogError("StartNode is null!");
            return;
        }

        _currentNode = startNode;
        UpdateDialogueUI();
    }

    private void UpdateDialogueUI()
    {
        // Temizlik
        foreach (var button in _activeChoiceButtons) 
        {
            if (button != null) Destroy(button);
        }
        _activeChoiceButtons.Clear();

        // Null kontrolleri
        if (_currentNode == null)
        {
            Debug.LogWarning("CurrentNode is null!");
            return;
        }

        // Diyalog içeriği
        if (dialogueText != null)
            dialogueText.text = _currentNode.text ?? "[No dialogue text]";

        // Karakter bilgileri
        CharacterSO currentCharacter = _currentNode.character != null ? _currentNode.character : defaultCharacter;
        
        if (characterPortrait != null)
            characterPortrait.sprite = currentCharacter?.portrait;

        UpdateRelationshipUI(currentCharacter);

        // Seçenek butonları
        if (_currentNode.choices != null && choicesParent != null && choiceButtonPrefab != null)
        {
            foreach (var choice in _currentNode.choices)
            {
                if (choice != null) 
                    CreateChoiceButton(choice);
            }
        }

        // Dedikodu paneli
        if (_currentNode.isGossipReveal && gossipPanel != null)
        {
            gossipPanel.SetActive(true);
            var gossipTMP = gossipPanel.GetComponentInChildren<TMP_Text>();
            if (gossipTMP != null)
                gossipTMP.text = "Dedikodu: " + (_currentNode.text ?? "");
        }
    }

    private void CreateChoiceButton(DialogueChoice choice)
{
    if (choiceButtonPrefab == null || choicesParent == null)
    {
        Debug.LogError("Button prefab or parent not assigned!");
        return;
    }

    GameObject buttonObj = Instantiate(choiceButtonPrefab, choicesParent);
    if (buttonObj == null) return;

    // YENİ EKLENEN KISIM (Buton pozisyon ayarı)
    RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
    buttonRect.anchoredPosition = Vector2.zero;
    buttonRect.localScale = Vector3.one;

    TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
    Button button = buttonObj.GetComponent<Button>();

    if (buttonText != null)
        buttonText.text = choice.choiceText ?? "[No choice text]";

    if (button != null)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => {
            ApplyChoiceEffects(choice);
            MoveToNextNode(choice.nextNode);
        });
    }

    _activeChoiceButtons.Add(buttonObj);

    // EK OPTİMİZASYON (Layout'u güncelle)
    LayoutRebuilder.ForceRebuildLayoutImmediate(choicesParent.GetComponent<RectTransform>());
}

    private void ApplyChoiceEffects(DialogueChoice choice)
    {
        if (choice == null) return;

        // Stres güncelleme
        currentStress = Mathf.Clamp(currentStress + choice.stressChange, 0, 100);
        UpdateStressUI();

        // İlişki güncelleme
        if (choice.relationshipChange != 0 && _currentNode != null && _currentNode.character != null)
        {
            UpdateRelationship(choice);
        }

        // Dedikodu yayma
        if (choice.gossipInfo != null)
        {
            SpreadGossip(choice.gossipInfo);
        }
    }

    private void UpdateRelationship(DialogueChoice choice)
    {
        if (_currentNode == null || _currentNode.character == null) return;

        CharacterSO character = _currentNode.character;
        if (!_relationships.ContainsKey(character))
        {
            _relationships[character] = character.initialRelationship;
        }
        _relationships[character] += choice.relationshipChange;
        UpdateRelationshipUI(character);
    }

    private void SpreadGossip(GossipNode gossip)
    {
        if (gossip == null || gossip.aboutCharacter == null) return;

        currentStress += gossip.stressOnSpread;
        UpdateStressUI();

        if (!_relationships.ContainsKey(gossip.aboutCharacter))
        {
            _relationships[gossip.aboutCharacter] = gossip.aboutCharacter.initialRelationship;
        }
        _relationships[gossip.aboutCharacter] -= (100 - gossip.credibility) / 10;
        UpdateRelationshipUI(gossip.aboutCharacter);
    }

    private void MoveToNextNode(DialogueNode nextNode)
    {
        if (nextNode != null)
        {
            _currentNode = nextNode;
            UpdateDialogueUI();
        }
        else
        {
            EndDialogue();
        }
    }

    private void UpdateStressUI()
    {
        if (stressMeter == null) return;

        stressMeter.value = currentStress;
        if (stressMeter.fillRect != null)
        {
            Image fillImage = stressMeter.fillRect.GetComponent<Image>();
            if (fillImage != null)
            {
                fillImage.color = Color.Lerp(Color.green, Color.red, currentStress / 100f);
            }
        }
    }

    private void UpdateRelationshipUI(CharacterSO character)
    {
        if (relationshipText == null) return;

        try
        {
            if (character == null)
            {
                relationshipText.text = "";
                return;
            }

            int score = _relationships.ContainsKey(character) ? 
                       _relationships[character] : character.initialRelationship;
            relationshipText.text = $"{character.characterName}: {score}";
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Relationship update error: {e.Message}");
            relationshipText.text = "";
        }
    }

    private void EndDialogue()
    {
        if (dialogueText != null)
            dialogueText.text = "Görüşme sona erdi.";
        
        if (characterPortrait != null)
            characterPortrait.sprite = null;

        foreach (var button in _activeChoiceButtons)
        {
            if (button != null) Destroy(button);
        }
        _activeChoiceButtons.Clear();
    }
}