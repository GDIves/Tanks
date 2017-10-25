using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int playerNum = 1;
    public Rigidbody shellRigidbody;
    public Transform fireTransform;
    public Slider aimSlider;
    public AudioSource shootingAudio;
    public AudioClip chargingClip;
    public AudioClip fireClip;
    public float minLaunchForce = 15f;
    public float maxLaunchForce = 30f;
    public float maxChargeTime = 0.75f;

    private string fireButtonName;
    private float currentLaunchForce;
    private float chargeSpeed;
    private bool isFired;

    private void OnEnable()
    {
        currentLaunchForce = minLaunchForce;
        aimSlider.value = minLaunchForce;
    }

    private void Start()
    {
        fireButtonName = "Fire" + playerNum;
        chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
    }

    private void Update()
    {
        aimSlider.value = minLaunchForce;
        if (currentLaunchForce >= maxLaunchForce && !isFired)
        {
            currentLaunchForce = maxLaunchForce;
            Fire();
        }
        //如果按下了发射按钮
        else if (Input.GetButtonDown(fireButtonName))
        {
            isFired = false;
            currentLaunchForce = minLaunchForce;
            shootingAudio.clip = chargingClip;
            shootingAudio.Play();
        }
        //如果发射按钮被按住且炮弹并没有发射
        else if (Input.GetButton(fireButtonName) && !isFired)
        {
            currentLaunchForce += chargeSpeed*Time.deltaTime;
            aimSlider.value = currentLaunchForce;
        }
        //如果发射键松开且炮弹没有发射
        else if (Input.GetButtonUp(fireButtonName) && !isFired)
        {
            Fire();
        }
    }

    private void Fire()
    {
        isFired = true;
        Rigidbody shellInstance = Instantiate(shellRigidbody, fireTransform.position, fireTransform.rotation) as Rigidbody;
        shellInstance.velocity = currentLaunchForce * shellInstance.transform.forward;
        shootingAudio.clip = fireClip;
        shootingAudio.Play();
        currentLaunchForce = minLaunchForce;
    }
}
