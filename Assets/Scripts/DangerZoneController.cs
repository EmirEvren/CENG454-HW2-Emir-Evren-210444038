using UnityEngine;
using System.Collections;

public class DangerZoneController : MonoBehaviour
{
    [SerializeField] private FlightExamManager examManager;
    [SerializeField] private MissileLauncher missileLauncher;
    [SerializeField] private float missileDelay = 5f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip warningClip; 
    
    private AudioSource audioSource; 

    private Coroutine activeCountdown;

    private void Start()
    {

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        audioSource.playOnAwake = false; 
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (warningClip != null && audioSource != null)
            {
                audioSource.clip = warningClip;
                audioSource.Play();
            }

            examManager.EnterDangerZone();
            activeCountdown = StartCoroutine(LaunchCountdownCoroutine(collision.transform));
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            if (activeCountdown != null)
            {
                StopCoroutine(activeCountdown);
                activeCountdown = null;
            }
            if (missileLauncher != null)
            {
                missileLauncher.DestroyActiveMissile();
            }
            examManager.ExitDangerZone();
        }
    }

    private IEnumerator LaunchCountdownCoroutine(Transform playerTransform)
    {
        yield return new WaitForSeconds(missileDelay);
        
        if (missileLauncher != null)
        {
            missileLauncher.Launch(playerTransform);
        }
    }
}