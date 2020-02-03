using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController character_Controller;

    private Vector3 move_Direction;

    [SerializeField] public float speed = 4f;
    private float gravity = 20f;

    [SerializeField] float jump_Force = 9f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2.2f;
    private float vertical_Velocity;

    private void Awake()
    {
        character_Controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveThePlayer();
    }

    void MoveThePlayer()    //move the player
    {
        move_Direction = new Vector3(Input.GetAxis(Axis.HORIZONTAL), 0f, Input.GetAxis(Axis.VERTICAL));

        move_Direction = transform.TransformDirection(move_Direction);
        move_Direction *= speed * Time.deltaTime;

        ApplyGravity();

        character_Controller.Move(move_Direction);
    }

    void ApplyGravity()     //apply gravity
    {
        vertical_Velocity -= gravity * (fallMultiplier - 1) * Time.deltaTime;

        PlayerJump();

        move_Direction.y = vertical_Velocity * Time.deltaTime;
    }

    void PlayerJump()   //jump on command
    {
        if (character_Controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            vertical_Velocity = jump_Force;
        }
        else if (!character_Controller.isGrounded && !Input.GetKey(KeyCode.Space))
        {
            vertical_Velocity -= gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
