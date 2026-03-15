using System.Collections;
using TMPro;
using UnityEngine;

public class Conversation : MonoBehaviour
{
    public string[] lines;
    public bool needsChoiceAtEnd;

    public IEnumerator Play()
    {
        // Tell the manager to show our lines and handle choices
    yield return StartCoroutine(DialogueManager.Instance.ShowDialogue(lines, needsChoiceAtEnd));
    }
}