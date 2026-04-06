using UnityEngine;

public class AircraftThreatHandler : MonoBehaviour
{
    [SerializeField] private FlightExamManager examManager;
    [SerializeField] private AudioClip explosionSound;
    
    [Header("Visual Effects")]
    [SerializeField] private GameObject explosionEffectPrefab; 

    [Header("Shake Settings")]
    [SerializeField] private float shakeDuration = 0.5f;   
    [SerializeField] private float shakeMagnitude = 0.7f;  

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Missile"))
        {
            Debug.Log("BOOM! Missile hit the aircraft!");
            
    
            CameraShake shaker = Camera.main.GetComponent<CameraShake>();
            if (shaker != null) 
            {
                shaker.TriggerShake(shakeDuration, shakeMagnitude); 
            }

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

        
            Destroy(other.gameObject);
        }
    }
}