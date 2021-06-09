using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControls : MonoBehaviour
{
    [SerializeField]
    private int playerHealth = 5;
    [SerializeField]
    private float timeBetweenShots = 1.0f;

    private bool wasKeyDown = true;
    private bool canShoot = true;

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
        
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        
        //healthBar.SetMaxHealth(playerHealth);
    }

    private void Update()
    {
        InputHolder();
    }

    private void InputHolder()
    {
        ray = new Ray(gunBarrelTransform.position, gunBarrelTransform.forward);
        Color colour = Color.green;

        bool triggerDown = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
       
        
        if (!wasKeyDown && triggerDown && canShoot)
        {
            colour = Color.red;
            audioSource.Play();
            RaycastFromGunBarrel();
            canShoot = false;
            StartCoroutine(TimeBetweenShots(timeBetweenShots));
        }
        wasKeyDown = triggerDown;
        
        //if (Input.GetMouseButton(0))
        //{
        //    colour = Color.red;
        //    audioSource.Play();
        //    RaycastFromGunBarrel();
        //}



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

    private IEnumerator TimeBetweenShots(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);
        canShoot = true;
    }
}
