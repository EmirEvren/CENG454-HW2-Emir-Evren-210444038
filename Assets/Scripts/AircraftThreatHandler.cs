using UnityEngine;

public class AircraftThreatHandler : MonoBehaviour
{
    [SerializeField] private FlightExamManager examManager;
    [SerializeField] private AudioClip explosionSound;
    
    [Header("Visual Effects")]
    [SerializeField] private GameObject explosionEffectPrefab; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Missile"))
        {
            Debug.Log("BOOM! Missile hit the aircraft!");
            if (explosionEffectPrefab != null)
            {
                GameObject fx = Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
                Destroy(fx, 3f);
            }
            if (explosionSound != null)
            {
                AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, 1f);
            }
            if (examManager != null) examManager.FailMission();

            FlightController ucusKodu = GetComponent<FlightController>();
            if (ucusKodu != null) ucusKodu.enabled = false;
            Destroy(other.gameObject);
        }
    }
}