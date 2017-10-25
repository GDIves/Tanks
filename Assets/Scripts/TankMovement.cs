using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int playerNum = 1;
    public float moveSpeed = 12f;
    public float turnSpeed = 180f;
    public AudioSource audioSource;
    public AudioClip idleClip;
    public AudioClip drivingClip;
    public float pitchRange = 0.2f;

    private Rigidbody tankRigidbody;
    private string moveAxisName;
    private string turnAxisName;
    private float moveInput;
    private float turnInput;
    private float originalPitch;
    private ParticleSystem[] particleSystems;

    private void Awake()
    {
        tankRigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        tankRigidbody.isKinematic = false;
        moveInput = 0f;
        turnInput = 0f;
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < particleSystems.Length; ++i)
        {
            particleSystems[i].Play();
        }
    }

    private void OnDisable()
    {
        tankRigidbody.isKinematic = true;
        for (int i = 0; i < particleSystems.Length; ++i)
        {
            particleSystems[i].Stop();
        }
    }

    private void Start()
    {
        moveAxisName = "Vertical" + playerNum;
        turnAxisName = "Horizontal" + playerNum;
        originalPitch = audioSource.pitch;
    }

    private void FixedUpdate()
    {
        //Move
        moveInput = Input.GetAxis(moveAxisName);
        Vector3 movement = transform.forward * moveInput * moveSpeed * Time.deltaTime;
        tankRigidbody.MovePosition(transform.position + movement);
        //Turn
        turnInput = Input.GetAxis(turnAxisName);
        float turn = turnInput*turnSpeed*Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0, turn, 0);
        tankRigidbody.MoveRotation(transform.rotation * turnRotation);
        //Effect
        if (moveInput != 0 || turnInput != 0)
        {
            if (audioSource.clip == idleClip)
            {
                audioSource.clip = drivingClip;
                audioSource.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.clip == drivingClip)
            {
                audioSource.clip = idleClip;
                audioSource.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
                audioSource.Play();
            }
        }
    }
}
