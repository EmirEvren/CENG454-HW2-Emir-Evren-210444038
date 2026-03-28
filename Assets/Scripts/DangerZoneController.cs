// DangerZoneController.cs
using UnityEngine;
using System.Collections;

public class DangerZoneController : MonoBehaviour
{
    [SerializeField] private FlightExamManager examManager;
    [SerializeField] private MissileLauncher missileLauncher;
    [SerializeField] private float missileDelay = 5f;

    private Coroutine activeCountdown;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            examManager.EnterDangerZone();
            activeCountdown = StartCoroutine(LaunchCountdownCoroutine(collision.transform));
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
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