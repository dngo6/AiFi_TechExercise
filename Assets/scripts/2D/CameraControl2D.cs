using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Very simple script to allow the user to zoom in and out using the mouse wheel.
public class CameraControl2D : MonoBehaviour {
    public Camera mainCamera;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float zoom = Input.GetAxis("Mouse ScrollWheel")*5;

        mainCamera.orthographicSize += zoom;
	}
}
