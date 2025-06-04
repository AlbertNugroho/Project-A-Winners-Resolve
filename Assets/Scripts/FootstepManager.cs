using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    public AudioSource Sfx;
    public AudioClip Grass;
    public AudioClip Steel;
    public AudioClip Wood;
    public AudioClip Default;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public void PlayFootstep()
    {
        int groundLayer = GetGroundLayer();

        switch (groundLayer)
        {
            case 3:
                Sfx.PlayOneShot(Grass);
                break;
            case 8:
                Sfx.PlayOneShot(Steel);
                break;
            case 9:
                Sfx.PlayOneShot(Wood);
                break;
            default:
                Sfx.PlayOneShot(Default);
                break;
        }
    }

    public void StopFootStep()
    {
        Sfx.Stop();
    }

    private int GetGroundLayer()
    {
        if (Physics.Raycast(groundCheck.position, Vector3.down, out RaycastHit hit, groundDistance + 0.1f, groundMask))
        {
            return hit.collider.gameObject.layer;
        }

        return -1;
    }
}
