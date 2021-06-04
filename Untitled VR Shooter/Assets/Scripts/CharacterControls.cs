using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControls : MonoBehaviour
{
    [SerializeField]
    private int playerHealth = 5;
    
    [SerializeField] 
    private AudioClip clip;
    private AudioSource audioSource;
    [SerializeField]
    private Transform gunBarrelTransform;
    [SerializeField]
    private LineRenderer lineRenderer;

    private HealthBar healthBar;
    private Ray ray;

    private void Start()
    {
        healthBar.SetMaxHealth(playerHealth);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        Input();
    }

    private void Input()
    {
        Color colour = Color.green;
        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {
            colour = Color.red;
            audioSource.Play();
            RaycastFromGunBarrel();
        }
        ray = new Ray(gunBarrelTransform.position, gunBarrelTransform.forward);

        lineRenderer.SetPosition(0, ray.origin);
        lineRenderer.SetPosition(1, ray.origin + 100 * ray.direction);
        lineRenderer.material.color = colour;
       // lineRenderer.SetColors(colour, colour);
        //Debug.DrawRay(gunBarrelTransform.position, gunBarrelTransform.forward, colour);
    }

    private void RaycastFromGunBarrel()
    {
        
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Enemy")
            {
                //Call Enemy hit function here

                //Temporary Code
                Destroy(hit.collider.gameObject);
            }
        }

    }

    public float PlayerTakeDamage(int damage)
    {
        playerHealth = playerHealth - damage;
        healthBar.SetHealth(playerHealth);
        return playerHealth;
    }
}
