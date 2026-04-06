using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class FlightExamManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text status_txt_HUD; 
    [SerializeField] private TMP_Text objectivePanel_text; 

    [Header("Audio Clips")]
    [SerializeField] private AudioClip win_sound_fx;
    [SerializeField] private AudioClip explode_fail_sfx;

    [Header("Mission States")]
    public bool hasTakenOff = false;
    public bool threatCleared = false;
    public bool missionComplete = false;
    public bool is_player_dead = false; 

    void Start()
    {
        Time.timeScale = 1f; 
        objectivePanel_text.text = "Objective: Take off from the runway.";
        status_txt_HUD.text = ""; 
    }

    void Update()
    {    

        if(Input.GetKeyDown(KeyCode.R)) RestartGame();
    }


    public void EnterDangerZone()
    {
        if(!is_player_dead && !missionComplete) 
        {
            CancelInvoke("ClearStatusHUD"); 
            status_txt_HUD.text = "Entered a Dangerous Zone!";
            status_txt_HUD.color = Color.red;
            objectivePanel_text.text = "Objective: Survive and Escape the Zone!";
        }
    }

    public void ExitDangerZone()
    {
        if (!is_player_dead && !missionComplete) 
        {
            threatCleared = true; 
            objectivePanel_text.text = "Objective: Return to base and land.";
            ClearStatusHUD(); 
        }
    }

    public void ShowApproachMessage()
    {
        if (!is_player_dead && !missionComplete)
        {
            CancelInvoke("ClearStatusHUD");
            if (threatCleared)
            {
                status_txt_HUD.text = "Zone Cleared. Safe to Land!";
                status_txt_HUD.color = Color.green;
            }
            else
            {
                status_txt_HUD.text = "WARNING: Threat active! Clear the zone first!";
                status_txt_HUD.color = Color.yellow;
            }
        }
    }

    public void ClearStatusHUD()
    {
        if (!is_player_dead && !missionComplete)
            status_txt_HUD.text = "";
    }


    public void TryToLand()
    {
        if (!is_player_dead && threatCleared && !missionComplete) 
        {
            missionComplete = true;
            is_player_dead = true; 
            
            status_txt_HUD.text = "MISSION ACCOMPLISHED!";
            status_txt_HUD.color = Color.cyan;
            objectivePanel_text.text = "Success! Press R to Play Again.";
            
            ClosingBackgroundSounds();
            AudioSource.PlayClipAtPoint(win_sound_fx, Camera.main.transform.position);

            LockPlayerControls();
        }
        else if (!is_player_dead && !threatCleared)
        {
            status_txt_HUD.text = "Cannot land yet! Clear the threat zone first!";
            status_txt_HUD.color = Color.red;
            Invoke("ClearStatusHUD", 3f);
        }
    }


    public void FailMission()
    {
        if (!is_player_dead) 
        {
            is_player_dead = true; 
            status_txt_HUD.text = "MISSION FAILED!";
            status_txt_HUD.color = Color.red;
            objectivePanel_text.text = "Aircraft Destroyed. Press R to Restart."; 
            
            ClosingBackgroundSounds(); 
            AudioSource.PlayClipAtPoint(explode_fail_sfx, Camera.main.transform.position);
            LockPlayerControls(); // Otomatik restart atmaz
        }
    }


    public void OutOfBoundsFail()
    {
        if (is_player_dead) return;

        is_player_dead = true;
        status_txt_HUD.text = "OUT OF BOUNDS!";
        status_txt_HUD.color = Color.red;
        objectivePanel_text.text = "Returning to start point in 3s...";

        ClosingBackgroundSounds();
        AudioSource.PlayClipAtPoint(explode_fail_sfx, Camera.main.transform.position);
        
        LockPlayerControls();
        StartCoroutine(AutoRestartRoutine()); 
    }

    private void LockPlayerControls()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            MonoBehaviour[] scripts = playerObj.GetComponentsInChildren<MonoBehaviour>();
            foreach (var s in scripts)
            {

                if (s.GetType() != typeof(FlightExamManager)) 
                {
                    s.enabled = false;
                }
            }


            Rigidbody rb = playerObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true; 
            }
        }
    }

    private IEnumerator AutoRestartRoutine()
    {

        yield return new WaitForSecondsRealtime(3f);
        RestartGame();
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ClosingBackgroundSounds()
    {

        DangerZoneController danger = Object.FindFirstObjectByType<DangerZoneController>();
        if(danger != null && danger.GetComponent<AudioSource>() != null) 
            danger.GetComponent<AudioSource>().Stop();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null && player.GetComponent<AudioSource>() != null) 
            player.GetComponent<AudioSource>().Stop();
    }
}