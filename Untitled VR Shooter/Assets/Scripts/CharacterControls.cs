using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControls : MonoBehaviour
{
    [SerializeField]
    private float playerHealth = 5;

    [SerializeField] 
    private AudioClip clip;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private Transform gunBarrelTransform;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
    }

    private void Update()
    {
        Input();
    }

    private void Input()
    {
        if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
        {
            audioSource.Play();
            RaycastFromGunBarrel();
        }
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

    public float PlayerTakeDamage(float damage)
    {
        playerHealth = playerHealth - damage;
        return playerHealth;
    }
}
