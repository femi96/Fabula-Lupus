using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarParticle : MonoBehaviour {

  private float jumpTime = 0f;
  public float jumpPeriod = 0.1f;
  public float jumpMin = 0.1f;
  public float jumpMax = 0.3f;

  public float goalDist = 0.1f;
  public Transform goal;

  public int numParticles = 5;

  public GameObject prefab;

  private Vector3[] pos;
  private GameObject[] gos;

  public LineRenderer line;

  void Start() {
    line = GetComponent<LineRenderer>();

    pos = new Vector3[numParticles];
    gos = new GameObject[numParticles];

    for (int i = 0; i < numParticles; i++) {
      pos[i] = goal.position;
      gos[i] = Instantiate(prefab, pos[i], Quaternion.identity, transform);
      gos[i].transform.localScale = gos[i].transform.localScale / (i + 1);
    }

    line.positionCount = numParticles;
    line.SetPositions(pos);
  }

  void Update() {
    jumpTime += Time.deltaTime;

    if (jumpTime >= jumpPeriod) {

      jumpTime -= jumpPeriod;

      Vector3 delta = goal.position - pos[0];
      Vector3 newPos = pos[0];

      Vector3 dir = (delta.normalized * 2f + Random.onUnitSphere).normalized;
      delta = dir * Mathf.Max(Mathf.Min(delta.magnitude, jumpMax), jumpMin);

      if (delta.magnitude > goalDist)
        newPos += delta;

      for (int i = numParticles - 1; i > 0; i--) {
        pos[i] = pos[i - 1];
      }

      pos[0] = newPos;

      // If change, update positions
      for (int i = 0; i < numParticles; i++) {
        gos[i].transform.position = pos[i];
      }

      line.SetPositions(pos);
    }
  }
}