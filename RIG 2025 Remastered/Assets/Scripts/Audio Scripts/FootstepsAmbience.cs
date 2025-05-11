using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FootstepsAmbience : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private AudioSource runningSrc;
    [SerializeField] private AudioSource walkingSrc;
    [SerializeField] private AudioSource crouchWalkSrc;
    [SerializeField] private AudioSource drainedWalkSrc;
    [SerializeField] private AudioSource drainedCrouchWalkSrc;



   [SerializeField] private float maxCrouchTime;
   [SerializeField] private float maxDrainedCrouchTime;
   private float crouchTimer;

    private void Start()
    {
        crouchTimer = maxCrouchTime;
    }

    public void Update()
    {
        if (playerMovement.IsMoving() && playerMovement.IsGrounded() && Time.deltaTime != 0)
        {
            if (playerMovement.IsRunning())
                Running();
            else if (playerMovement.IsDrainedCrouchWalking())
                DrainedCrouchWalking();
            else if (playerMovement.IsSneaking())
                Sneaking();
            else if (playerMovement.IsDrained())
                DrainedWalking();
            else
                Walking();
        }
        else if (runningSrc.isPlaying || walkingSrc.isPlaying || crouchWalkSrc.isPlaying || drainedWalkSrc.isPlaying || drainedCrouchWalkSrc.isPlaying)
        {
            StopMovementSounds();
        }

        if (crouchTimer >= 0)
            crouchTimer -= Time.deltaTime;
    }

    private void StopMovementSounds()
    {
        crouchTimer = -1f;
        runningSrc.Stop();
        walkingSrc.Stop();
        crouchWalkSrc.Stop();
        drainedWalkSrc.Stop();
        drainedCrouchWalkSrc.Stop();
    }

    private void Sneaking()
    {
        if (!crouchWalkSrc.isPlaying && crouchTimer < 0)
        {
            StopMovementSounds();
            crouchTimer = maxCrouchTime;
            crouchWalkSrc.Play();
        }    
    }

    private void Walking()
    {
        if (!walkingSrc.isPlaying)
        {
            StopMovementSounds();
            walkingSrc.Play();
        }
    }

    private void Running()
    {
        if (!runningSrc.isPlaying)
        {
            StopMovementSounds();
            runningSrc.Play();
        }
    }

    private void DrainedWalking()
    {
        if (!drainedWalkSrc.isPlaying)
        {
            StopMovementSounds();
            drainedWalkSrc.Play();
        }
    }

    private void DrainedCrouchWalking()
    {
        if (!drainedCrouchWalkSrc.isPlaying && crouchTimer < 0)
        {
            StopMovementSounds();
            crouchTimer = maxDrainedCrouchTime;
            drainedCrouchWalkSrc.Play();
        }
    }
}
