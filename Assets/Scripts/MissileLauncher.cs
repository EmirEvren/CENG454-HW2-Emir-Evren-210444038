using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform launchPoint;
    [SerializeField] private AudioClip launchSound; 
    private GameObject activeMissile;

    public void Launch(Transform targetPlane)
    {
        if (missilePrefab != null && launchPoint != null)
        {
            activeMissile = Instantiate(missilePrefab, launchPoint.position, launchPoint.rotation);
            MissileHoming homing = activeMissile.GetComponent<MissileHoming>();
            if (homing != null) homing.SetTarget(targetPlane);

            if (launchSound != null)
            {
                AudioSource.PlayClipAtPoint(launchSound, Camera.main.transform.position, 1f);
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