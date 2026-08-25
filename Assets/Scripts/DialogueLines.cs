using UnityEngine;
using TMPro;

public class DialogueLines : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;

    [SerializeField]
    private string[] lines =
    {
        "We're glad to have you here lieutenant. We could really use your help!",
        "Like shooting fish in a barrel!",
        "It's a trap! Get out of there.",
        "Crikey! Looks like you could use a hand.",
        "Never thought I'd see your face around here general...",
        "Righteo let's finish this once and for all.",
        "Struth! Looks like the galaxy just got a little more safe.",
        "Yeeeehaw!! You did it!"
    };

    private int currentLine = 0;

    private void Start()
    {
        // The TextMeshPro object already displays the first line,
        // so we don't need to set it here.
    }

    // Call this from the Signal Emitter
    public void NextLine()
    {
        // Move to the next line first
        currentLine++;

        // Make sure we haven't gone past the end
        if (currentLine >= lines.Length)
        {
            currentLine = lines.Length - 1;
        }

        // Update the TextMeshPro UI
        dialogueText.text = lines[currentLine];
    }
}