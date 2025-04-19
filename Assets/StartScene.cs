using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    public CanvasGroup fadeOverlay; 
    public float fadeDuration = 1f;

    private bool hasStarted = false;

    void Start()
    {
        Time.timeScale = 0f; // Pause the game
        if (fadeOverlay != null)
            fadeOverlay.alpha = 1f; // Make sure it's fully visible
    }

    void Update()
    {
        if (!hasStarted && Input.GetKeyUp(KeyCode.Q))
        {
            hasStarted = true;
            StartCoroutine(FadeAndStartGame());
        }
    }

    IEnumerator FadeAndStartGame()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = 1f - (timer / fadeDuration);
            if (fadeOverlay != null)
                fadeOverlay.alpha = t;

            yield return null;
        }

        if (fadeOverlay != null)
            fadeOverlay.alpha = 0f;

        Time.timeScale = 1f; // Resume game
    }
}
