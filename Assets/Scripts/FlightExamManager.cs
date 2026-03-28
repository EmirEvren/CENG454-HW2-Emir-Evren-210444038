using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FlightExamManager : MonoBehaviour
{
    [SerializeField] private TMP_Text statusText; 
    [SerializeField] private TMP_Text missionText; 
    [SerializeField] private AudioClip successSound;

    public bool hasTakenOff = false;
    public bool threatCleared = false;
    public bool missionComplete = false;
    private bool isGameOver = false;

    void Start()
    {
        if (missionText != null) missionText.text = "Objective: Take off from the runway.";
    }

    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    public void EnterDangerZone()
    {
        statusText.text = "Entered a Dangerous Zone!";
        statusText.color = Color.red;
        if (missionText != null) missionText.text = "Objective: Survive and Escape the Zone!";
    }

    public void ExitDangerZone()
    {
        threatCleared = true; 
        statusText.text = "Zone Cleared. Safe to Land!";
        statusText.color = Color.green;
        if (missionText != null) missionText.text = "Objective: Land safely at the airstrip.";
    }

    public void TryToLand()
    {
        if (threatCleared && !missionComplete)
        {
            missionComplete = true;
            isGameOver = true; 
            statusText.text = "MISSION ACCOMPLISHED!";
            statusText.color = Color.blue;
            if (missionText != null) missionText.text = "You survived! Press 'R' to Restart.";
            
            if (successSound != null)
            {
                AudioSource.PlayClipAtPoint(successSound, Camera.main.transform.position, 1f);
            }

            GameObject playerPlane = GameObject.FindGameObjectWithTag("Player");
            if (playerPlane != null)
            {
                FlightController ucusKodu = playerPlane.GetComponent<FlightController>();
                if (ucusKodu != null) ucusKodu.enabled = false;
            }
        }
        else if (!threatCleared)
        {
            statusText.text = "Cannot land yet! Clear the threat zone first!";
            statusText.color = Color.yellow;
        }
    }

    public void FailMission()
    {
        missionComplete = false;
        isGameOver = true; 
        statusText.text = "MISSION FAILED!";
        statusText.color = Color.red;
        if (missionText != null) missionText.text = "Aircraft Destroyed. Press 'R' to Restart.";
        Debug.Log("Mission failed.");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}