using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class handles all gun behaviour: hit scaning for targets and applying forces to the correct rigidbodies, and launching grenades
public class FireGun : MonoBehaviour
{
    [SerializeField] private Transform gun;
    //[SerializeField] private Transform camera;
    [SerializeField] private float raycastDistance = 100.0f;
    //[SerializeField] private float gunPower = 1000.0f;
    [SerializeField] private Transform player;
    [SerializeField] private float gunRecoilAmount = 200.0f;
    [SerializeField] private LineRenderer bulletTrail;
    private EnemyAI enemyAIScript;

    private Rigidbody gunRB;
    private bool primareFireInput;

    //Audio
    private AudioSource primaryFireSound;

    //Initializes the class
    void Start()
    {
        gunRB = gun.transform.GetComponent<Rigidbody>();
        primaryFireSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        //Get mouse input
        primareFireInput = Input.GetButtonDown("Fire1");

        //When primary fire input is detected ...
        if (primareFireInput)
        {
            //play theweapon sound and apply recoil to the gun's spring joint
            primaryFireSound.Play();
            gunRB.AddForce(gun.up * -gunRecoilAmount);

            //cast a ray into the scene and add forces to rigidbodies; if the ray hits an enemy, enable their ragdoll physics 
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, raycastDistance))
            {
                SpawnBulletTrail(hit.point);
                    
                if (hit.collider.transform.CompareTag("EnemyHead"))
                {
                    //Debug.Log("Shot enemy's head");
                    enemyAIScript = hit.transform.GetComponentInParent<EnemyAI>();
                    enemyAIScript.TakeDamageHeadshot();
                }
                else if (hit.transform.CompareTag("Enemy"))
                {
                    //Debug.Log("Shot enemy");
                    enemyAIScript = hit.transform.GetComponent<EnemyAI>();
                    enemyAIScript.TakeDamage();
                }
                else if (hit.transform.CompareTag("Geometry"))
                {
                    //Debug.Log("You shot the wall.");
                }

                /*
                if (hit.rigidbody != null && !hit.transform.CompareTag("Geometry"))
                {
                    Debug.Log("BANG!");
                    Vector3 hitDirection = (hit.point - gun.position).normalized;
                    hit.transform.GetComponent<Rigidbody>().AddForce(hitDirection * gunPower);

                    
                    Vector3 hitDirection = (hit.transform.position - gun.position).normalized;
                    Debug.DrawRay(camera.position, camera.forward * hit.distance, Color.yellow);
                    //print("Bang!");

                    if (hit.transform.root.CompareTag("Enemy"))
                    {
                        Ragdoll ragdoll = hit.transform.root.GetComponent<Ragdoll>();

                        if (ragdoll != null)
                        {
                            ragdoll.RagdollOn = true;
                        }

                    }

                    hit.transform.GetComponent<Rigidbody>().AddForce(hitDirection * gunPower);              
                }*/
            }
            else
            {
                //Debug.DrawRay(camera.position, camera.forward * 1000, Color.white);
                print("Miss!");
            }
        }
    }

    //This block of code came from a tutorial (from which I also got the bullet trail prefab)
    private void SpawnBulletTrail(Vector3 hitPoint)
    {
        GameObject bulletTrailEffect = Instantiate(bulletTrail.gameObject, gun.position, Quaternion.identity);
        LineRenderer lineRenderer = bulletTrailEffect.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, gun.position);
        lineRenderer.SetPosition(1, hitPoint);
        Destroy(bulletTrailEffect, 1.0f);
    }
}