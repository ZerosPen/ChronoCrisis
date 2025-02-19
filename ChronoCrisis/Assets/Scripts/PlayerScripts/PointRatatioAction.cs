using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointRatatioAction : MonoBehaviour
{
    private Camera MainCam;
    private Vector3 mousePos;
    public bool isSkillActive = false;

    void Start()
    {
        MainCam = Camera.main;

        if (MainCam == null)
        {
            Debug.LogError("MainCamera not found! Make sure your camera is tagged as 'MainCamera'.");
        }
    }

    void Update()
    {
        if (MainCam == null) return;

        Vector3 worldMousePos = GetCursorPosition();

        if (isSkillActive)
        {
            transform.position = worldMousePos;
        }
        else
        {
            Vector3 direction = worldMousePos - transform.position;
            float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }
    }

    public Vector3 GetCursorPosition()
    {
        if (MainCam == null) return Vector3.zero;

        mousePos = Input.mousePosition;
        mousePos.z = 10f;
        Vector3 worldMousePos = MainCam.ScreenToWorldPoint(mousePos);
        worldMousePos.z = 0f;
        return worldMousePos;
    }
}
