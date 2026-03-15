using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject talkPanel;   // The main dialogue box
    public GameObject choicePack;  // The container for buttons
    public TextMeshProUGUI dialogueText;
    public int lastChoiceIndex;
    
    private bool choiceMade = false;

    void Awake()
    {
        Instance = this;
        talkPanel.SetActive(false);
        choicePack.SetActive(false);
    }

    public IEnumerator ShowDialogue(string[] lines, bool hasChoice)
    {
        talkPanel.SetActive(true); // Show the overlay

        foreach (string line in lines)
        {
            yield return StartCoroutine(TypeLine(line));
            yield return WaitForInput();
        }

        if (hasChoice)
        {
            yield return StartCoroutine(HandleChoices());
        }

        talkPanel.SetActive(false); // Hide overlay when done
    }

    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.04f);
        }
    }

    IEnumerator WaitForInput()
    {
        while (!Input.GetMouseButtonDown(0)) yield return null;
    }

    IEnumerator HandleChoices()
    {
        choicePack.SetActive(true); // Show the buttons
        choiceMade = false;

        // Wait here until a button calls our "MakeChoice" function
        while (!choiceMade)
        {
            yield return null;
        }

        choicePack.SetActive(false);
    }

    // This function is called by your UI Buttons (OnClick)
    public void MakeChoice()
    {
        choiceMade = true;
        Debug.Log("Choice Selected!");
    }
    public void MakeChoice(int index)
{
    lastChoiceIndex = index;
    choiceMade = true;
}
}