using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PointRatatioAction : MonoBehaviour
{
    private Camera MainCam;
    private Vector3 mousePos;
    public bool isSkillActive;

    // Start is called before the first frame update
    void Start()
    {
        GameObject camObject = GameObject.FindGameObjectWithTag("MainCamera");
        if (camObject != null)
        {
            MainCam = Camera.main;
        }
        else
        {
            Debug.LogError("MainCamera not found! Make sure your camera is tagged as 'MainCamera'.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (MainCam == null) return;

        if (!isSkillActive)
        {
            // Get mouse position in world space
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;

            Vector3 worldMousePos = MainCam.ScreenToWorldPoint(mousePos);

            // Calculate direction from object to mouse
            Vector3 direction = worldMousePos - transform.position;

            // Calculate angle and apply rotation (Z-axis only)
            float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }

        if(isSkillActive)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f; // Adjust this based on the camera distance

            Vector3 worldPosition = MainCam.ScreenToWorldPoint(mousePos);
            worldPosition.z = 0f; // Ensure it's at the correct depth

            transform.position = worldPosition;
        }
    }

}
