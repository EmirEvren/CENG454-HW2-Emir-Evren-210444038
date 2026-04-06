using UnityEngine;
using TMPro;

public class GameAreaBoundary : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private TMP_Text warningText;
    [SerializeField] private string baseWarningMessage = "WARNING: RETURN TO GAME AREA!";

    [Header("Game Over Settings")]
    [SerializeField] private float timeLimit = 5f; // Sınır dışında kalma süresi
    [SerializeField] private FlightExamManager examManager; // Patlama ve game over için
    
    private float timer;
    private bool isOutOfBounds = false;

    private void Start()
    {
        if (warningText != null)
        {
            warningText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Eğer oyuncu alanın dışındaysa geri sayımı başlat
        if (isOutOfBounds)
        {
            timer -= Time.deltaTime; // Zamanı geriye doğru akıt

            if (warningText != null)
            {
                // Mathf.Ceil ile küsuratlı sayıları (4.2 gibi) yukarı yuvarlayıp (5) ekrana yazdırıyoruz.
                warningText.text = $"{baseWarningMessage}\nReset in: {Mathf.Ceil(timer)}";
            }

            // Süre dolduğunda yapılacaklar
            if (timer <= 0f)
            {
                isOutOfBounds = false; // Sayacı durdur
                
                if (warningText != null)
                {
                    warningText.gameObject.SetActive(false); // Yazıyı gizle
                }

                // FlightExamManager üzerinden görevi başarısız say (patlama + restart yazısı)
                if (examManager != null)
                {
                    examManager.FailMission();
                }
            }
        }
    }

    // Oyuncu kutunun DIŞINA çıkarsa
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOutOfBounds = true;
            timer = timeLimit; // Zamanlayıcıyı 5 saniyeden başlat

            if (warningText != null)
            {
                warningText.color = Color.red;
                warningText.gameObject.SetActive(true);
            }
        }
    }

    // Oyuncu kutunun İÇİNE geri dönerse
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOutOfBounds = false; // Geri sayımı iptal et

            if (warningText != null)
            {
                warningText.gameObject.SetActive(false); // Yazıyı gizle
            }
        }
    }
}