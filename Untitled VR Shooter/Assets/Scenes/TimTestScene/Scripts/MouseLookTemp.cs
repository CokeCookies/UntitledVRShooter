using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class comes from an online tutorial:
//https://www.youtube.com/watch?v=_QajrabyTJc
//It handles the first-person camera functionality. I added the cursor visibility setting.

public class MouseLookTemp : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 100.0f;
    [SerializeField] Transform player;
    private float xRotation = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Allows clamping of rotation (i.e. not 'flipping) camera upside down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);
        transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);

        player.Rotate(Vector3.up * mouseX);
    }
}