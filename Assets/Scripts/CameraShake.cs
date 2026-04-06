using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
  
    private Vector3 initialLocalPos; 

    void Start()
    {
 
        initialLocalPos = transform.localPosition;
    }

    public void TriggerShake(float duration, float magnitude)
    {
        StopAllCoroutines(); 
        StartCoroutine(Shake(duration, magnitude));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {

            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;


            transform.localPosition = initialLocalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null; 
        }


        transform.localPosition = initialLocalPos;
    }
}