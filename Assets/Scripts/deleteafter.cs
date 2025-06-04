using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class deleteafter : MonoBehaviour
{
    public GameObject text;       // The UI text GameObject to fade
    public float activeTime;      // Time before the fade starts
    public float fadeDuration;    // Time it takes for the fade to complete

    private TextMeshProUGUI t;               // The Text component
    private Color originalColor;  // Original color of the text
    private bool isFading = false; // To prevent multiple coroutines

    // Start is called before the first frame update
    void Start()
    {
        if (text != null)
        {
            t = text.GetComponent<TextMeshProUGUI>();  // Get the Text component
            if (t != null)
            {
                originalColor = t.color;        // Save the original color
            }
        }
    }

    private void Update()
    {
        // Start the fade process if the text is active and not already fading
        if (text != null && text.activeSelf && !isFading)
        {
            StartCoroutine(FadeOut());
        }
    }

    // Coroutine to fade out the text gradually
    IEnumerator FadeOut()
    {
        isFading = true;

        // Wait for the specified active time before starting the fade
        yield return new WaitForSeconds(activeTime);

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            if (t != null)
            {
                // Gradually change the alpha value of the color
                float alpha = Mathf.Lerp(originalColor.a, 0f, elapsedTime / fadeDuration);
                t.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the text is fully transparent at the end of the fade
        if (t != null)
        {
            t.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        }

        // Disable the text GameObject after fading out
        text.SetActive(false);
        t.color = new Color(originalColor.r, originalColor.g, originalColor.b, 100f);
        isFading = false;
    }
}
