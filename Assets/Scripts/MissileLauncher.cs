using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform launchPoint;
    
    [Header("Effects & Sound")]
    [SerializeField] private GameObject launchEffectPrefab;
    [SerializeField] private AudioClip launchSound; 
    [SerializeField] [Range(0f, 2f)] private float launchVolume = 1.5f; 

    private GameObject activeMissile;

    public void Launch(Transform targetPlane)
    {
        if (missilePrefab != null && launchPoint != null)
        {
      
            activeMissile = Instantiate(missilePrefab, launchPoint.position, launchPoint.rotation);
            MissileHoming homing = activeMissile.GetComponent<MissileHoming>();
            if (homing != null) homing.SetTarget(targetPlane);

          
            if (launchEffectPrefab != null)
            {
                GameObject effect = Instantiate(launchEffectPrefab, launchPoint.position, launchPoint.rotation);
                Destroy(effect, 3f); 
            }

            if (launchSound != null)
            {
            
                AudioSource.PlayClipAtPoint(launchSound, Camera.main.transform.position, launchVolume);
            }
        }
    }

    public void DestroyActiveMissile()
    {
        if (activeMissile != null)
        {
            Destroy(activeMissile);
            activeMissile = null;
        }
    }
}