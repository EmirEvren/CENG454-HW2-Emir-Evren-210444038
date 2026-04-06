using UnityEngine;

public class ApproachZone : MonoBehaviour
{
    [SerializeField] private FlightExamManager examManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (examManager != null)
            {
                examManager.ShowApproachMessage();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (examManager != null)
            {
                examManager.ClearStatusHUD();
            }
        }
    }
}