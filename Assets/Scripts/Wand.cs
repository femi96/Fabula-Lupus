using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wand : MonoBehaviour {
  /*
  Controller that handles wand movement, inputs, and behaviors
  */

  // Object refs
  private BattleGrid battle;
  public WandCamera cam;
  public Transform shape;

  void Start() {
    StartUI();
  }

  void Update() {

    if (battle == null)
      battle = Object.FindObjectOfType<BattleGrid>();

    MoveWand();
    KeyInputs();
    UpdateUI();

    if (Input.GetKeyDown("n")) {
      PassUnit();
    }
  }

  /* Game Change */
  private BattleUnit targetUnit;

  public bool onUnitCommands = false;
  public bool onUnitWait = false;
  public bool onUnitLook = false;
  public bool onStatusScreen = false;

  private void PassUnit() {
    onUnitCommands = true;
    battle.NextCurrentUnit();

    // Camera mode
    cam.SetMenuMode(true);

    // Move wand
    Vector2Int pos = battle.currentUnit.position;
    transform.position = new Vector3(pos.x, 0, pos.y);
  }

  public void OnWait() {
    onUnitWait = true;
    onUnitCommands = false;
  }

  public void OnWaitConfirm() {
    onUnitWait = false;
    PassUnit();
  }

  public void OnWaitCancel() {
    onUnitWait = false;
    onUnitCommands = true;
  }

  public void OnLook() {
    cam.SetMenuMode(false);
    onUnitLook = true;
    onUnitCommands = false;
  }

  public void OnLookCancel() {
    cam.SetMenuMode(true);
    onUnitLook = false;
    onUnitCommands = true;

    // Move wand
    Vector2Int pos = battle.currentUnit.position;
    transform.position = new Vector3(pos.x, 0, pos.y);
  }

  public void OnStatus() {
    cam.SetMenuMode(true);
    onStatusScreen = true;
    onUnitLook = false;
  }

  public void OnStatusCancel() {
    cam.SetMenuMode(false);
    onStatusScreen = false;
    onUnitLook = true;
  }

  public void KeyInputs() {
    if (onUnitLook && Input.GetKeyDown("z")) {
      OnLookCancel();
    }

    if ((onUnitLook && targetUnit == battle.currentUnit) && Input.GetKeyDown("x")) {
      OnStatus();
    }

    if (onStatusScreen && Input.GetKeyDown("z")) {
      OnStatusCancel();
    }
  }

  /* Wand UI */
  [Header("UI: Unit Panel Left")]
  public GameObject unitPanelL;
  public Text uplNameText;
  public Text uplLevelText;
  public Text uplHealthText;
  public Text uplManaText;

  [Header("UI: Unit Menu")]
  public GameObject unitCommandsMenu;
  public GameObject unitWait;
  public GameObject unitStatusKey;
  public GameObject unitLookCancel;

  [Header("UI: Unit Status")]
  public GameObject unitStatus;

  private void StartUI() {
    unitPanelL.GetComponent<Animator>().Play("SlideOut", -1, 1f);
    unitCommandsMenu.GetComponent<Animator>().Play("SlideOut", -1, 1f);
    unitWait.GetComponent<Animator>().Play("SlideOut", -1, 1f);
    unitStatusKey.GetComponent<Animator>().Play("SlideOut", -1, 1f);
    unitLookCancel.GetComponent<Animator>().Play("SlideOut", -1, 1f);
    unitStatus.GetComponent<Animator>().Play("SlideOut", -1, 1f);
  }

  private void UpdateUI() {

    // Update targetUnit
    BattleUnit newTargetUnit = battle.GetUnit(transform.position);

    // Update UI for targetUnit

    // UI: Unit Panel L
    if (newTargetUnit != targetUnit || onStatusScreen) {

      Animator uplAnim = unitPanelL.GetComponent<Animator>();
      AnimatorStateInfo uplAnimState = uplAnim.GetCurrentAnimatorStateInfo(0);

      if (uplAnimState.IsName("SlideIn")) {
        targetUnit = null;
        float animTime = Mathf.Max(1f - uplAnimState.normalizedTime, 0f);
        uplAnim.Play("SlideOut", -1, animTime);
      }

      if (uplAnimState.IsName("SlideOut") && uplAnimState.normalizedTime > 1f) {

        targetUnit = newTargetUnit;

        if (targetUnit != null) {
          uplNameText.text = targetUnit.unit.name;
          uplLevelText.text = targetUnit.unit.level.ToString();
          uplHealthText.text = targetUnit.unit.healthCur.ToString() + "/" + targetUnit.unit.healthMax.ToString();
          uplManaText.text = targetUnit.unit.manaCur.ToString() + "/" + targetUnit.unit.manaMax.ToString();

          uplAnim.Play("SlideIn");
        }
      }
    }

    // UI: Unit Commands Menu
    Animator ucmAnim = unitCommandsMenu.GetComponent<Animator>();
    AnimatorStateInfo ucmAnimState = ucmAnim.GetCurrentAnimatorStateInfo(0);

    if (!onUnitCommands && ucmAnimState.IsName("SlideIn")) {
      float animTime = Mathf.Max(1f - ucmAnimState.normalizedTime, 0f);
      ucmAnim.Play("SlideOut", -1, animTime);
    }

    if (onUnitCommands && targetUnit == battle.currentUnit && ucmAnimState.IsName("SlideOut") && ucmAnimState.normalizedTime > 1f) {
      ucmAnim.Play("SlideIn");
    }

    // UI: Unit Wait
    Animator uwAnim = unitWait.GetComponent<Animator>();
    AnimatorStateInfo uwAnimState = uwAnim.GetCurrentAnimatorStateInfo(0);

    if (!onUnitWait && uwAnimState.IsName("SlideIn")) {
      float animTime = Mathf.Max(1f - uwAnimState.normalizedTime, 0f);
      uwAnim.Play("SlideOut", -1, animTime);
    }

    if (onUnitWait && targetUnit == battle.currentUnit && uwAnimState.IsName("SlideOut") && uwAnimState.normalizedTime > 1f) {
      uwAnim.Play("SlideIn");
    }

    // UI: Unit Look
    Animator uskAnim = unitStatusKey.GetComponent<Animator>();
    Animator ulcAnim = unitLookCancel.GetComponent<Animator>();
    AnimatorStateInfo uskAnimState = uskAnim.GetCurrentAnimatorStateInfo(0);
    AnimatorStateInfo ulcAnimState = ulcAnim.GetCurrentAnimatorStateInfo(0);

    if (!(onUnitLook && targetUnit == battle.currentUnit) && uskAnimState.IsName("SlideIn")) {
      float animTime = Mathf.Max(1f - uskAnimState.normalizedTime, 0f);
      uskAnim.Play("SlideOut", -1, animTime);
    }

    if ((onUnitLook && targetUnit == battle.currentUnit) && uskAnimState.IsName("SlideOut") && uskAnimState.normalizedTime > 1f) {
      uskAnim.Play("SlideIn");
    }

    if (!(onUnitLook || onStatusScreen) && ulcAnimState.IsName("SlideIn")) {
      float animTime = Mathf.Max(1f - ulcAnimState.normalizedTime, 0f);
      ulcAnim.Play("SlideOut", -1, animTime);
    }

    if ((onUnitLook || onStatusScreen) && ulcAnimState.IsName("SlideOut") && ulcAnimState.normalizedTime > 1f) {
      ulcAnim.Play("SlideIn");
    }

    // UI: Unit Status
    Animator usAnim = unitStatus.GetComponent<Animator>();
    AnimatorStateInfo usAnimState = usAnim.GetCurrentAnimatorStateInfo(0);

    if (!onStatusScreen && usAnimState.IsName("SlideIn")) {
      float animTime = Mathf.Max(1f - usAnimState.normalizedTime, 0f);
      usAnim.Play("SlideOut", -1, animTime);
    }

    if (onStatusScreen && usAnimState.IsName("SlideOut") && usAnimState.normalizedTime > 1f) {
      usAnim.Play("SlideIn");
    }
  }

  /* Wand Movement */
  private float moveTime = 0f;
  private const float moveToTileCooldown = 2.5f;
  private float moveSpeed = 4f;
  private float moveToTileSpeed = 4f;
  private float moveToTileSpeedMax = 2f;
  private float moveToTileSpeedMin = 0.0f;

  // Move wand each frame
  private void MoveWand() {
    moveTime += Time.deltaTime;

    if (!onUnitCommands && !onUnitWait)
      MoveWandFromInput();

    // If not moving, move to lock
    if (moveTime > moveToTileCooldown)
      MoveWandToTile();

    // Limit movement
    LimitWandPosition();

  }

// Move wand with inputs
  private void MoveWandFromInput() {

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
  }

// If not moving, move to lock
  private void MoveWandToTile() {

    float deltaX = Mathf.RoundToInt(transform.position.x) - transform.position.x;
    float deltaZ = Mathf.RoundToInt(transform.position.z) - transform.position.z;
    Vector3 deltaV = new Vector3(deltaX, 0, deltaZ);

    float time = Mathf.Min(1f, moveTime - moveToTileCooldown);
    float spd = Mathf.Min(Mathf.Max(deltaV.magnitude * moveToTileSpeed * time, moveToTileSpeedMin), moveToTileSpeedMax);

    deltaV = deltaV.normalized * spd * Time.deltaTime;

    if (deltaV.magnitude > Time.deltaTime)
      deltaV = deltaV.normalized * Time.deltaTime;

    transform.position += deltaV;
  }

  private void LimitWandPosition() {
    if (transform.position.x < battle.xMin)
      transform.position -= Vector3.right * (transform.position.x - battle.xMin);

    if (transform.position.x > battle.xMax)
      transform.position -= Vector3.right * (transform.position.x - battle.xMax);

    if (transform.position.z < battle.zMin)
      transform.position -= Vector3.forward * (transform.position.z - battle.zMin);

    if (transform.position.z > battle.zMax)
      transform.position -= Vector3.forward * (transform.position.z - battle.zMax);

    shape.position -= Vector3.up * (shape.position.y - battle.GetHeight(transform.position));
  }
}
