using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class pickupletter : MonoBehaviour
{
    public GameObject collectTextObj, objectiveTextobj;
    public AudioSource pickupSound;
    public static int pagesCollected;
    public TextMeshProUGUI collectText;
    public TextMeshProUGUI objectiveText;
    public GameObject interactPrompt;
    public GameObject fence;
    public List<AudioSource> ambianceLayers;
    private bool interactable = false;
    public AIChase AI;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            interactPrompt.SetActive(true);
            interactable = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            interactPrompt.SetActive(false);
            interactable = false;
        }
    }

    void Update()
    {
        // Check if player presses E while near the letter
        if (interactable && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
    }

    void PickUp()
    {
        AI.lettersPicked++;
        pagesCollected++;
        collectText.text = pagesCollected + "/5 pages";
        collectTextObj.SetActive(true);
        pickupSound.Play();

        // Only activate ambience at specific pickup numbers
        if (pagesCollected == 2)
        {
            PlayAmbienceLayer(0); // Heartbeat maybe?
        }
        else if (pagesCollected == 4)
        {
            PlayAmbienceLayer(1); // Whisper or tension drone
        }
        else if (pagesCollected == 5)
        {
            PlayAmbienceLayer(2); // Final intense ambience
            objectiveText.text = "Get Out Now...\nGo To Your Car Beyond The Fence And Leave";
            fence.SetActive(false);
            objectiveTextobj.SetActive(true);
        }

        interactPrompt.SetActive(false);
        gameObject.SetActive(false);
        interactable = false;
    }
    void PlayAmbienceLayer(int index)
    {
        if (index < ambianceLayers.Count)
        {
            ambianceLayers[index].enabled = true;
        }
    }

}
