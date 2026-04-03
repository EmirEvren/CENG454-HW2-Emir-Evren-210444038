using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FlightExamManager : MonoBehaviour
{
    
    [SerializeField] private TMP_Text status_txt_HUD; 
    [SerializeField] private TMP_Text objectivePanel_text; 
    [SerializeField] private AudioClip win_sound_fx;
    [SerializeField] private AudioClip explode_fail_sfx;

    public bool hasTakenOff = false;
    public bool threatCleared = false;
    public bool missionComplete = false;
    private bool is_player_dead = false; 

    void Start()
    {
        // right side of game starting panel for showing first comment of game situation
        objectivePanel_text.text = "Objective: Take off from the runway.";
    }

    void Update()
    {    
        // for start again game r 
        if(Input.GetKeyDown(KeyCode.R)) 
        {
            int current_lvl_index = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(current_lvl_index);
        }
    }

    public void EnterDangerZone()
    {
        if(is_player_dead == false) 
        {
            status_txt_HUD.text = "Entered a Dangerous Zone!";
            status_txt_HUD.color = Color.red;
            objectivePanel_text.text = "Objective: Survive and Escape the Zone!";
        }
    }

    public void ExitDangerZone()
    {
        if (is_player_dead == false) 
        {
            threatCleared = true; 
            status_txt_HUD.text = "Zone Cleared. Safe to Land!";
            status_txt_HUD.color = Color.green;
            objectivePanel_text.text = "Objective: Land safely at the airstrip.";
        }
    }

    public void TryToLand()
    {
        if (is_player_dead == false) 
        {
            if (threatCleared == true) 
            {
                if (missionComplete == false) 
                {
                    missionComplete = true;
                    is_player_dead = true; 
                    
                    status_txt_HUD.text = "MISSION ACCOMPLISHED!";
                    status_txt_HUD.color = Color.blue;
                    objectivePanel_text.text = "You survived! Press 'R' to Restart.";
                    
                    ClosingBackgroundSounds(); //for not mixing all sound here

                    AudioSource.PlayClipAtPoint(win_sound_fx, Camera.main.transform.position);

                   
                    FlightController player_flight_script = GameObject.FindGameObjectWithTag("Player").GetComponent<FlightController>();   // Taking away the flight keys so they don't crash post-victory
                    player_flight_script.enabled = false; 
                }
            }
            else 
            {
                status_txt_HUD.text = "Cannot land yet! Clear the threat zone first!";
                status_txt_HUD.color = Color.yellow;
            }
        }
    }

    public void FailMission()
    {
        if (is_player_dead == false) 
        {
            is_player_dead = true; 
            missionComplete = false;
            
            status_txt_HUD.text = "MISSION FAILED!";
            status_txt_HUD.color = Color.red;
            objectivePanel_text.text = "Aircraft Destroyed. Press 'R' to Restart."; 
            
            //for hearing better losing sound
            ClosingBackgroundSounds(); 
            
            AudioSource.PlayClipAtPoint(explode_fail_sfx, Camera.main.transform.position);
            Debug.Log("aircraft destroy game over "); 
        }
    }

    void ClosingBackgroundSounds()
    {
        //in danger zone sounds came when we tryna here losing sound
        DangerZoneController danger_zone_script = FindObjectOfType<DangerZoneController>();
        if(danger_zone_script != null) 
        {
            danger_zone_script.GetComponent<AudioSource>().Stop();
        }

        // turning off plane controler
        GameObject player_plane_obj = GameObject.FindGameObjectWithTag("Player");
        if(player_plane_obj != null) 
        {
            player_plane_obj.GetComponent<AudioSource>().Stop();
        }
    }
}