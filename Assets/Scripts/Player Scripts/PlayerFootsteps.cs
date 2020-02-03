using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{

    private AudioSource footStep_Sound;

    [SerializeField] private AudioClip[] footStep_Clip;

    private CharacterController character_Controller;

    [HideInInspector] public float volume_Min, volume_Max;
    private float accumulated_Distance;
    [HideInInspector] public float step_Distance;

    // Start is called before the first frame update
    void Awake()
    {
        character_Controller = GetComponentInParent<CharacterController>();
        footStep_Sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckToPlaySound();
    }

    void CheckToPlaySound()
    {
        if (!character_Controller.isGrounded)
        {
            return;
        }

        //if player is moving play sound
        if (character_Controller.velocity.sqrMagnitude > 0)
        {
            //is the value how far we can go
            //e.g. make a step or sprint or move while crouoching
            //until we play the footstep sound
            accumulated_Distance += Time.deltaTime;

            if (accumulated_Distance > step_Distance)
            {
                footStep_Sound.volume = Random.Range(volume_Min, volume_Max);
                footStep_Sound.clip = footStep_Clip[Random.Range(0, footStep_Clip.Length)];
                footStep_Sound.Play();

                accumulated_Distance = 0f;
            }
        }
        else 
        {
            accumulated_Distance = 0f;
        }
    }
}
