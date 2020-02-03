using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintAndCrouch : MonoBehaviour
{
    private PlayerMovement playerMovement;

    [SerializeField] float sprint_Speed = 8f;
    [SerializeField] float move_Speed = 4f;
    [SerializeField] float crouch_Speed = 1.5f;
    [SerializeField] float pose_Speed = 3.5f;

    private Transform look_Root;
    private Vector3 look_RootLocalPos = new Vector3();
    private float current_Height = 1.6f;
    private float target_Height = 1.6f;
    private float stand_Height = 1.6f;
    private float crouch_Height = 1f;
    private bool is_Crouching;
    private float t = 0f;

    private PlayerFootsteps player_FootSteps;
    private float sprint_Volume = 1f;
    private float crouch_Volume = 0.1f;
    private float walk_Volume_Min = 0.2f, walk_Volume_Max = 0.6f;

    private float walk_Step_Distance = 0.5f;
    private float sprint_Step_Distance = 0.33f;
    private float crouch_Step_Distance = 0.9f;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        look_Root = transform.GetChild(0);

        player_FootSteps = GetComponentInChildren<PlayerFootsteps>();
    }

    private void Start()
    {
        player_FootSteps.volume_Min = walk_Volume_Min;
        player_FootSteps.volume_Max = walk_Volume_Max;
        player_FootSteps.step_Distance = walk_Step_Distance;
    }

    // Update is called once per frame
    void Update()
    {
        Sprint();
        Crouch();

       

    }

    void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !is_Crouching)
        {
            playerMovement.speed = sprint_Speed;

            player_FootSteps.step_Distance = sprint_Step_Distance;
            player_FootSteps.volume_Min = sprint_Volume;
            player_FootSteps.volume_Max = sprint_Volume;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && !is_Crouching)
        {
            playerMovement.speed = move_Speed;

            player_FootSteps.volume_Min = walk_Volume_Min;
            player_FootSteps.volume_Max = walk_Volume_Max;
            player_FootSteps.step_Distance = walk_Step_Distance;
        }
    }

    void Crouch()
    {
        
        /* Below is for a non-toggable crouch
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            look_Root.localPosition = new Vector3(0f, crouch_Height, 0f);            
            playerMovement.speed = crouch_Speed;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {            
            look_Root.localPosition = new Vector3(0f, stand_Height, 0f);            
            playerMovement.speed = move_Speed;
        }*/


        // below code is for a toggable crouch
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            //reset the interpolator every time LCtrl is pressed, so that it starts at zero, keeping it between it 0-1 scale requirement
            t = 0f;

            if(is_Crouching)
            {                
                current_Height = 1f;
                target_Height = 1.6f;
                playerMovement.speed = move_Speed;

                player_FootSteps.volume_Min = walk_Volume_Min;
                player_FootSteps.volume_Max = walk_Volume_Max;
                player_FootSteps.step_Distance = walk_Step_Distance;

                is_Crouching = false;
            }
            else
            {                
                current_Height = 1.6f;
                target_Height = 1f;
                playerMovement.speed = crouch_Speed;

                player_FootSteps.step_Distance = crouch_Step_Distance;
                player_FootSteps.volume_Min = crouch_Volume;
                player_FootSteps.volume_Max = crouch_Volume;

                is_Crouching = true;
            }
        }

        t += pose_Speed * Time.deltaTime;

        look_Root.localPosition = new Vector3(0f, Mathf.SmoothStep(current_Height, target_Height, t), 0f);        
    }
}
