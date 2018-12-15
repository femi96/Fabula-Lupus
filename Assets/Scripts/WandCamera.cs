using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandCamera : MonoBehaviour {
  /*
  Controller that handles camera inputs and behaviors
  */

  public bool menuMode;

  private float x = 0.0f; // Current camera angles
  private float y = 30.0f;

  private float distance = 7f;  // Current distance from wand

  private float xSpeed = 9.0f;    // Angular change rate
  private float ySpeed = 12.0f;
  private float yMinLimit = 5f;   // Angle bounds
  private float yMaxLimit = 80f;

  private float distanceSpeed = 0.5f; // Distance change rate
  private float distanceMin = 3f;     // Distance bounds
  private float distanceMax = 10f;

  void Start() {

    // Lock cursor to screen
    SetMenuMode(false);

    // Camera positioning
    Vector3 angles = transform.eulerAngles;
    x = angles.y;
    y = angles.x;
  }

  void Update() {

    // Lock cursor to screen on input
    if (menuMode) {
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
    } else {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    }
  }

  void LateUpdate() {

    // Don't move camera if in menu
    if (menuMode)
      return;

    // Update camera position based on mouse movement
    x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
    y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
    x = (x + 360f) % 360f;
    y = Mathf.Clamp(y, yMinLimit, yMaxLimit);

    Quaternion rotation = Quaternion.Euler(y, x, 0);

    distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * distanceSpeed, distanceMin, distanceMax);

    Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
    Vector3 position = rotation * negDistance;

    transform.rotation = rotation;
    transform.position = position + transform.parent.position;
  }

  public void SetMenuMode(bool mode) {
    menuMode = mode;
  }
}
