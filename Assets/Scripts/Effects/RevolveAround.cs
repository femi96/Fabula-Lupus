using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolveAround : MonoBehaviour {

  private Vector3 center;
  private float time;

  public float speed = 1f;
  public float radius = 1f;

  void Start() {
    center = transform.position;
  }

  void Update() {
    time += Time.deltaTime * speed;

    float period = Mathf.PI * 2f;

    if (time > period)
      time = time - period;

    transform.position = center + radius * new Vector3(Mathf.Sin(time), 0, Mathf.Cos(time));
  }
}
