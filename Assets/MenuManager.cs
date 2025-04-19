using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public InputActionReference menubutton;
    public GameObject menu;
    public PlayerControls playerMovementScript;
    private bool isMenu;
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Start()
    {
        Time.timeScale = 1;
        menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (menubutton.action.WasPressedThisFrame())
        {
            PauseSystem();
        }
    }

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void PauseSystem()
    {
        if (!isMenu)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            menu.SetActive(true);
            playerMovementScript.enabled = false;
            Time.timeScale = 0;
            isMenu = true;
        }
        else if(isMenu)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            menu.SetActive(false);
            playerMovementScript.enabled = true;
            Time.timeScale = 1;
            isMenu = false;
        }
    }
}
