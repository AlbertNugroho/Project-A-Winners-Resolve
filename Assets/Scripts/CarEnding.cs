using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEnding : MonoBehaviour
{
    public GameObject interactPrompt;
    public GameObject EndingScene;
    private bool interactable = false;

    private CarSoundManager cfm;

    private void Start()
    {
        EndingScene.SetActive(false);
        cfm = GetComponent<CarSoundManager>();
    }
    private void Update()
    {
        if (interactable && Input.GetKeyDown(KeyCode.E))
        {
            
            EndGame();
        }
    }

    private void EndGame()
    {
        EndingScene.SetActive(true);
        cfm.PlayCarAudio();
    }

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
}
