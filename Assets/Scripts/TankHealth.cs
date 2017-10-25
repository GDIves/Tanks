using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float startingHealth = 100f;
    public Slider healthSlider;
    public Image fillImage;
    public Color fullHealthColor = Color.green;
    public Color zeroHealthColor = Color.red;
    public GameObject explosionPrefab;

    private AudioSource explosionAudio;
    private ParticleSystem explosionParticle;
    private float currentHealth;
    private bool dead;

    private void Awake()
    {
        //GameObject go = Instantiate(explosionPrefab);
        //explosionAudio = go.GetComponent<AudioSource>();
        explosionParticle = Instantiate(explosionPrefab).GetComponent<ParticleSystem>();
        explosionAudio = explosionParticle.GetComponent<AudioSource>();
        explosionParticle.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        currentHealth = startingHealth;
        dead = false;
        SetHealthUI();
    }

    private void SetHealthUI()
    {
        healthSlider.value = currentHealth;
        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, currentHealth / startingHealth);   
    }

    public void TakeDamege(float attack)
    {
        currentHealth -= attack;
        SetHealthUI();
        if (currentHealth <= 0f && !dead)
        {
            dead = true;
            explosionParticle.transform.position = transform.position;
            explosionParticle.gameObject.SetActive(true);
            explosionParticle.Play();
            explosionAudio.Play();
            gameObject.SetActive(false);
        }
    }
}
