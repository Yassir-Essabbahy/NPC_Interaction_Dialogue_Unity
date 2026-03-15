using UnityEngine;
using TMPro;
using System.Collections;
using Cinemachine;
using UnityEngine.UI; // Using Unity 6 naming

public class InteractionText : MonoBehaviour
{
    [Header("UI & Settings")]
    public TextMeshProUGUI InteractText;
    public float InteractionDistance = 5f;
    private bool CanInteract = true;

    [Header("Cameras")]
    public CinemachineVirtualCamera PlayerVcam;
    public CinemachineVirtualCamera TalkZoomVcam;

    public GameObject TalkPanel;
    public GameObject ChoicePack;
    

    void Update()
    {
        if (!CanInteract) return;

        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, InteractionDistance))
        {
            if (hit.collider.CompareTag("InteractNPC"))
            {
                InteractText.text = "Press 'E' To Talk";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // GRAB SCRIPTS FROM THE NPC WE HIT
                    Conversation npcConv = hit.collider.GetComponent<Conversation>();
                    LookAt npcLook = hit.collider.GetComponent<LookAt>();

                    if (npcConv != null)
                        StartCoroutine(TalkSequence(npcConv, npcLook));
                }
            }
            else { InteractText.text = ""; }
        }
        else { InteractText.text = ""; }
    }

    IEnumerator TalkSequence(Conversation conv, LookAt look)
    {
        CanInteract = false;
        InteractText.text = "";

        // Setup NPC and Camera
        if (look != null) look.IKActive = true;
        
        PlayerVcam.Priority = 0;
        TalkZoomVcam.Priority = 10;

        yield return new WaitForSeconds(1f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Run the dialogue and WAIT for it to finish
        yield return StartCoroutine(conv.Play());

        // Reset
        PlayerVcam.Priority = 10;
        TalkZoomVcam.Priority = 0;
        if (look != null) look.IKActive = false;
        Cursor.lockState = CursorLockMode.Locked;


        CanInteract = true;
    }
}