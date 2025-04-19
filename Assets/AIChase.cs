using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.StandaloneInputModule;
using UnityEngine.UI;

public class AIChase : MonoBehaviour
{
    public Transform player;
    public Transform playerCamera;
    public Transform[] teleportPoints;
    public Transform scareFocusPoint;
    public float detectionDistance = 10f;
    public float detectionTime = 2f;
    public float forgetTime = 3f;
    public float chaseSpeed = 12f;
    public float scareTriggerDistance = 5f;
    public float teleportCooldown = 8f;
    public float yRotationOffset = 90f;
    public AudioClip scareSound;
    public AudioClip boneBreaksfx;
    public PlayerControls playerMovementScript;
    private bool isRotatingToScare = false;
    private Quaternion scareTargetRotation;
    public RawImage alertImage; // Drag your RawImage here in the inspector
    public float fadeSpeed = 0.2f;
    private bool isChasing = false;
    private bool isGameOverTriggered = false;
    private float lookTimer = 0f;
    private float loseSightTimer = 0f;
    private float teleportTimer = 0f;
    private NavMeshAgent agent;
    public AudioSource bonebreak;
    private AudioSource audioSource;
    private float currentAlpha = 0f;
    public int lettersPicked;
    public LayerMask obstacleMask; // Assign in Inspector (e.g. Buildings, Walls)

    public AudioClip alertAudioClip; // New Audio Clip for the alert sound
    private float alertAudioVolume = 0f; // Used to control the volume during the alert phase

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = chaseSpeed;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        if (alertAudioClip != null)
        {
            audioSource.clip = alertAudioClip;
            audioSource.loop = true; // Ensure it loops during the alert
            audioSource.volume = 0f; // Start muted
            audioSource.Play();
        }

        TeleportToRandomPoint();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        teleportTimer += Time.deltaTime;

        RotateTowardsPlayer(); // Always rotate toward the player

        if (IsPlayerLookingAtAI())
        {
            lookTimer += Time.deltaTime;
            loseSightTimer = 0f;

            if (lookTimer >= detectionTime && !isChasing && distance <= detectionDistance)
            {
                StartChase();
            }
        }
        else
        {
            loseSightTimer += Time.deltaTime;
            lookTimer = 0f;

            if (loseSightTimer >= forgetTime && isChasing)
            {
                StopChase();
            }
        }

        if (alertImage != null)
        {
            float targetAlpha;

            if (isGameOverTriggered)
            {
                targetAlpha = 0f; // Fade out after game over
            }
            else if (IsPlayerLookingAtAI())
            {
                targetAlpha = 0.03f; // Fade in when player is looking
            }
            else
            {
                targetAlpha = 0f; // Fade out when not looking
            }

            float speed = isGameOverTriggered ? fadeSpeed * 10f : fadeSpeed;
            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, Time.deltaTime * speed);

            Color color = alertImage.color;
            color.a = currentAlpha;
            alertImage.color = color;

            fadeInOutAudio(currentAlpha);
        }

        if (!isChasing && teleportTimer >= teleportCooldown)
        {
            TeleportToRandomPoint();
            teleportTimer = 0f;
        }

        if (isChasing)
        {
            agent.SetDestination(player.position);
        }

        if (distance <= scareTriggerDistance && !isGameOverTriggered)
        {
            TriggerGameOver();
        }

        if (isRotatingToScare)
        {
            float rotationSpeed = 10f; // You can tweak this for faster/slower turning
            player.rotation = Quaternion.Slerp(player.rotation, scareTargetRotation, Time.deltaTime * rotationSpeed);

            // Optional: Stop rotating when close enough
            if (Quaternion.Angle(player.rotation, scareTargetRotation) < 1f)
            {
                player.rotation = scareTargetRotation;
                isRotatingToScare = false;
            }
        }
    }

    void fadeInOutAudio(float alpha)
    {
        alertAudioVolume = Mathf.Lerp(0f, 0.7f, alpha); // Linear interpolation from 0 to 1
        audioSource.volume = alertAudioVolume;
    }

    bool IsPlayerLookingAtAI()
    {
        Vector3 dir = (transform.position - playerCamera.position).normalized;
        float angle = Vector3.Angle(playerCamera.forward, dir);

        if (angle > 30f) return false;

        Ray ray = new Ray(playerCamera.position, dir);
        RaycastHit hit;

        // Cast against everything
        if (Physics.Raycast(ray, out hit, detectionDistance))
        {
            if (hit.transform == transform)
            {
                return true; // Player can see the AI
            }
            else if (((1 << hit.collider.gameObject.layer) & obstacleMask) != 0)
            {
                return false; // Blocked by something in obstacleMask
            }
        }

        return false; // Hit something else (not AI or valid obstacle), or nothing at all
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f; // Lock vertical rotation

        if (direction == Vector3.zero) return;

        // Calculate base rotation toward the player
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Apply -90 degrees on the Y axis
        targetRotation *= Quaternion.Euler(0, -0f, 0);

        // Smoothly rotate toward the adjusted target
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    void StartChase()
    {
        isChasing = true;
        agent.isStopped = false;
    }

    void StopChase()
    {
        isChasing = false;
        agent.isStopped = true;
    }

    void TeleportToRandomPoint()
    {
        if (teleportPoints.Length == 0) return;

        float minRange = 10f;

        float maxRange = 50f; 

        if (lettersPicked == 1)
        {
            minRange = 25f;
            maxRange = 50f;
        }
        else if (lettersPicked == 2)
        {
            minRange = 20f;
            maxRange = 45f;
        }
        else if (lettersPicked == 3)
        {
            minRange = 15f;
            maxRange = 35f;
        }
        else if (lettersPicked == 4)
        {
            minRange = 10f;
            maxRange = 25f;
        }
        else if (lettersPicked >= 5)
        {
            minRange = 10f;
            maxRange = 20f;
        }

        Vector3 playerPosition = player.position;

        Vector3 randomDirection = Random.insideUnitSphere * maxRange;
        randomDirection.y = 0f;

        Vector3 teleportPosition = playerPosition + randomDirection;

        float distanceFromPlayer = Vector3.Distance(teleportPosition, player.position);
        if (distanceFromPlayer < minRange)
        {
            // Adjust position if it's too close
            teleportPosition = playerPosition + randomDirection.normalized * minRange;
        }

        NavMeshHit hit;
        if (NavMesh.SamplePosition(teleportPosition, out hit, maxRange, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
        else
        {
            teleportPosition = playerPosition + randomDirection.normalized * minRange;
            transform.position = teleportPosition;
        }
    }

    void TriggerGameOver()
    {
        isGameOverTriggered = true;
        isChasing = false;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.ResetPath();

        if (playerMovementScript != null)
        {
            playerMovementScript.hands.SetFloat("Speed", 0);
            playerMovementScript.enabled = false;
        }

        Vector3 direction = (scareFocusPoint.position - player.position).normalized;
        scareTargetRotation = Quaternion.LookRotation(direction);
        isRotatingToScare = true;

        if (scareSound != null)
        {
            audioSource.PlayOneShot(scareSound);
            bonebreak.PlayOneShot(boneBreaksfx);
        }

        Invoke(nameof(ReturnToMenu), 5f);
    }

    void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
