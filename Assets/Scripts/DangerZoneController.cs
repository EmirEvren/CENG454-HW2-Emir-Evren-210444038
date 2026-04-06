using UnityEngine;
using System.Collections;

public class DangerZoneController : MonoBehaviour
{
    [SerializeField] private FlightExamManager examManager;
    [SerializeField] private MissileLauncher missileLauncher;
    [SerializeField] private float missileDelay = 5f;
    [SerializeField] private AudioClip warningClip; 
    
    private AudioSource audioSource; 
    private Coroutine activeCountdown;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (activeCountdown != null) StopCoroutine(activeCountdown);

            if (warningClip != null && audioSource != null)
            {
                audioSource.clip = warningClip;
                audioSource.Play();
            }

            examManager.EnterDangerZone();
  
            activeCountdown = StartCoroutine(LaunchCountdownCoroutine(other.transform));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioSource != null) audioSource.Stop();

            
            if (activeCountdown != null)
            {
                StopCoroutine(activeCountdown);
                activeCountdown = null;
            }

       
            if (missileLauncher != null) missileLauncher.DestroyActiveMissile();

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
    
        activeCountdown = null;
    }
}