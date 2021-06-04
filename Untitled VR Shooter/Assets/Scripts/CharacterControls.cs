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

    private HealthBar healthBar;

    private void Start()
    {
        healthBar.SetMaxHealth(playerHealth);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
    }

    private void Update()
    {
        Input();
    }

    private void Input()
    {
        Color colour = new Vector4(0, 0, 1, 1);
        if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
        {
            colour = new Vector4(1, 0, 0, 1);
            audioSource.Play();
            RaycastFromGunBarrel();
        }
        Debug.DrawRay(gunBarrelTransform.position, gunBarrelTransform.forward, colour);
    }

    private void RaycastFromGunBarrel()
    {
        RaycastHit hit;

        if (Physics.Raycast(gunBarrelTransform.position, gunBarrelTransform.forward, out hit))
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
