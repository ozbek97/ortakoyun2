using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public DialogueNode startNode;
    
    void Start()
    {
        if(startNode == null) // Yeni eklenen
        {
            Debug.LogError("StartNode atanmamış! Inspector'da bir diyalog nodu seçin.");
            return;
        }

        var manager = FindObjectOfType<DialogueManager>();
        if(manager != null)
        {
            manager.StartDialogue(startNode);
        }
        else
        {
            Debug.LogError("DialogueManager bulunamadı!");
        }
    }
}