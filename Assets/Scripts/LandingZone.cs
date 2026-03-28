using UnityEngine;

public class LandingZone : MonoBehaviour
{
    [SerializeField] private FlightExamManager examManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            examManager.TryToLand();
        }
    }
}