﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    public float sprintAdd;
    public bool sprinting;
    public float sprintTimer;
    public bool bsprintCD;
    public float sprintCD = 5;
    public bool canSprint;
    public bool trapped;
    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidBody;
    int floorMask;
    float camRayLength = 100f;

    public void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody>();
        sprinting = false;
        sprintTimer = 3;
        sprintCD = 5;
        canSprint = true;
        trapped = false;
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Move(h, v);
        Turning();
        Animating(h, v);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canSprint)
        {
            StartCoroutine(nameof(Sprint));
        }
    }

    IEnumerator Sprint()
    {
        canSprint = false;
        sprinting = true;
        sprintAdd = 6;
        yield return new WaitForSeconds(sprintTimer);
        sprinting = false;
        sprintAdd = 0;
        yield return new WaitForSeconds(sprintCD);
        canSprint = true;
    }

    private void Move(float h, float v)
    {
        if (!trapped)
        {
        movement.Set(h, 0f, v);
        movement = movement.normalized * (speed+sprintAdd) * Time.deltaTime;
        playerRigidBody.MovePosition(transform.position + movement);
        }
        
    }
    void Turning()
    {
        Ray  camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        if (Physics.Raycast (camRay,out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidBody.MoveRotation(newRotation);
        }

        
    }
        void Animating(float h, float v)
        {
            bool walking = h != 0f || v != 0f;
            anim.SetBool("IsWalking", walking);
        }

}
