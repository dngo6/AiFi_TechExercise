using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Very simple script to allow the user to zoom in and out using the mouse wheel.
public class CameraControl3D : MonoBehaviour
{
    public Camera mainCamera;
    private Vector3 mousePos;
    private Transform camera;
    public float panSpeed = 4.0f;
    // Use this for initialization
    void Start()
    {
        camera = mainCamera.gameObject.transform;
        mousePos = camera.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        float zoom = -Input.GetAxis("Mouse ScrollWheel") * 5;
        float deltaX = 0;
        float deltaY = 0;


        if (Input.GetMouseButton(2))
        {
            mousePos = camera.position;
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mousePos);
            deltaX = pos.x*panSpeed;
            deltaY = pos.y*panSpeed;
        }

        mainCamera.gameObject.transform.Translate(deltaX, deltaY, zoom, Space.Self);
        mousePos = camera.position;
    }
}
