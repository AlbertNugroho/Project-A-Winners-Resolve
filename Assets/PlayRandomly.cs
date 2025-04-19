using System.Collections;
using UnityEngine;

public class PlayRandomly : MonoBehaviour
{
    public AudioSource audioSource; // The audio source component
    public float minTime = 10f; // Minimum time interval before the next audio play
    public float maxTime = 60f; // Maximum time interval before the next audio play

    private float randomTimer; // Timer to control the random intervals

    // Start is called before the first frame update
    void Start()
    {
        randomTimer = Random.Range(minTime, maxTime); // Initialize with a random time
    }

    // Update is called once per frame
    void Update()
    {
        randomTimer -= Time.deltaTime; // Decrease the timer by the time passed since last frame

        if (randomTimer <= 0f) // If the timer reaches 0, play a random audio clip
        {
            audioSource.Play();
            randomTimer = Random.Range(minTime, maxTime); // Reset the timer to a new random time
        }
    }
}
