using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingQuit : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
    }

}
