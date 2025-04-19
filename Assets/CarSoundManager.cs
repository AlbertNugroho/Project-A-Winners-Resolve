using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSoundManager : MonoBehaviour
{
    public AudioSource Sfx;
    public AudioClip CarOpen;
    public AudioClip CarClose;
    public AudioClip Break;
    public AudioClip CarStart;
    public AudioClip CarGo;
    public AudioClip Carstop;

    public void PlayCarAudio()
    {
        StartCoroutine(CarAudio());
    }

    IEnumerator CarAudio()
    {
        Sfx.PlayOneShot(CarOpen);
        yield return new WaitForSeconds(CarOpen.length);

        Sfx.PlayOneShot(CarClose);
        yield return new WaitForSeconds(CarClose.length);

        Sfx.PlayOneShot(Break);
        yield return new WaitForSeconds(Break.length);

        Sfx.PlayOneShot(CarStart);
        yield return new WaitForSeconds(CarStart.length);

        Sfx.PlayOneShot(CarGo);
        StartCoroutine(FadeOut(Sfx, 5f)); // Fade out after it's done
        yield return new WaitForSeconds(CarGo.length);
        Time.timeScale = 0;
    }
    IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0f)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // Reset for next time
    }
}
