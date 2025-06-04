using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource Music;
    [SerializeField] public AudioSource Sfx;

    public AudioClip musicbg;
    public AudioClip shootfx;
    public AudioClip pickupfx;
    public AudioClip Damagedfx;
    public AudioClip deathfx;
    public AudioClip enemyatkfx;

    private void Start()
    {
        Music.clip = musicbg;
        Music.Play();
    }

    public void playclip(AudioClip audio)
    {
        Sfx.PlayOneShot(audio); 
    }

    public void stopMusic()
    {
        Music.Stop();
    }
}
