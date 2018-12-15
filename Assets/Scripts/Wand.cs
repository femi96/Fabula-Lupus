using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : MonoBehaviour {
  /*
  Controller that handles wand movement, inputs, and behaviors
  */

  private float moveTime = 0f;
  private const float moveToTileCooldown = 1f;
  private float moveSpeed = 4f;
  private float moveToTileSpeed = 4f;
  private float moveToTileSpeedMax = 2f;
  private float moveToTileSpeedMin = 0.5f;

  void Start() {

  }

  void Update() {

    moveTime += Time.deltaTime;
    MoveWand();

    // If not moving, move to lock
    if (moveTime > moveToTileCooldown) {
      MoveWandToTile();
    }
  }

  // Move wand each frame
  private void MoveWand() {

    // Transform input direction based on camera forward
    float spd = moveSpeed * Time.deltaTime;

    Vector3 moveDirection = Camera.main.transform.forward;
    moveDirection.y = 0;

    float x = Input.GetAxis("Horizontal");
    float z = Input.GetAxis("Vertical");

    moveDirection = Vector3.Normalize(moveDirection);   // Don't normalize your inputs
    Vector3 moveDirectionF = moveDirection * z;         // Project z onto forward direction vector
    Vector3 moveDirectionR = new Vector3(moveDirection.z, 0, -moveDirection.x); // Create right vector
    moveDirectionR *= x;                                // Project x onto right direction vector

    moveDirection = moveDirectionF + moveDirectionR;
    moveDirection *= spd;

    // Update move timer
    if (x != 0 || z != 0) {
      moveTime = 0;
    }

    // Apply move direction to transform
    transform.Translate(moveDirection.x, 0, moveDirection.z);

    // Limit movement
    // LimitWandPosition();
  }

  // If not moving, move to lock
  private void MoveWandToTile() {

    float deltaX = Mathf.RoundToInt(transform.position.x) - transform.position.x;
    float deltaZ = Mathf.RoundToInt(transform.position.z) - transform.position.z;
    Vector3 deltaV = new Vector3(deltaX, 0, deltaZ);

    float spd = Mathf.Min(Mathf.Max(deltaV.magnitude * moveToTileSpeed, moveToTileSpeedMin), moveToTileSpeedMax);

    if (deltaV.magnitude > Time.deltaTime)
      deltaV = deltaV.normalized * spd * Time.deltaTime;

    transform.position += deltaV;
  }
}
