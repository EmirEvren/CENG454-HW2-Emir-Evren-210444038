using UnityEngine;

public class MissileHoming : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 40f;
    [SerializeField] private float turnSpeed = 5f;
    private Transform target;
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        if (target == null) return;
        Vector3 direction = target.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}