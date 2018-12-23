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

    if (whileUnitMove && !battle.isMovingUnit)
      OnMoveFinish();

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
  public bool onUnitMove = false;
  public bool whileUnitMove = false;
  public bool onUnitActions = false;

  private HashSet<TileNode> targetTiles;
  public GameObject tilePrefab;
  public Transform tileTransform;

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

  public void OnAct() {
    onUnitActions = true;
    onUnitCommands = false;
  }

  public void OnActCancel() {
    onUnitActions = false;
    onUnitCommands = true;
  }

  public void OnMove() {
    cam.SetMenuMode(false);
    onUnitMove = true;
    onUnitCommands = false;

    // Move tiles
    targetTiles = battle.GetMoveTiles(battle.currentUnit);

    foreach (TileNode tile in targetTiles)
      Instantiate(tilePrefab, tile.GetPos(), Quaternion.identity, tileTransform);
  }

  public void OnMoveConfirm() {
    cam.SetMenuMode(true);
    whileUnitMove = true;
    onUnitMove = false;

    // Move unit
    TileNode end = battle.GetTile(transform.position);
    List<TileNode> path = battle.GetMovePath(battle.currentUnit, end);
    battle.MoveUnit(battle.currentUnit, path);

    // Delete tiles
    foreach (Transform tile in tileTransform)
      Destroy(tile.gameObject);
  }

  public void OnMoveFinish() {
    onUnitCommands = true;
    whileUnitMove = false;
  }

  public void OnMoveCancel() {
    cam.SetMenuMode(true);
    onUnitMove = false;
    onUnitCommands = true;

    // Move wand
    Vector2Int pos = battle.currentUnit.position;
    transform.position = new Vector3(pos.x, 0, pos.y);

    // Delete tiles
    foreach (Transform tile in tileTransform)
      Destroy(tile.gameObject);
  }

  public void KeyInputs() {
    if (Input.GetKeyDown("x")) {

      if (onUnitLook && targetUnit != null)
        OnStatus();

      if (onUnitMove && targetTiles.Contains(battle.GetTile(transform.position)))
        OnMoveConfirm();
    }

    if (Input.GetKeyDown("z")) {

      if (onUnitLook)
        OnLookCancel();

      if (onStatusScreen)
        OnStatusCancel();

      if (onUnitMove)
        OnMoveCancel();
    }
  }



  /* Wand UI */
  [Header("UI: Unit Panel Left")]
  public GameObject unitPanelL;

  [Header("UI: Unit Menu")]
  public GameObject unitCommandsMenu;
  public GameObject unitWait;
  public GameObject unitStatusKey;
  public GameObject unitCancelKey;
  public GameObject unitMoveKey;

  [Header("UI: Unit Status")]
  public GameObject unitStatus;

  [Header("UI: Unit Actions")]
  public GameObject unitActions;
  public GameObject unitActionPrefab;

  private void StartUI() {
    unitPanelL.GetComponent<Animator>().Play("SlideOut", -1, 1f);
    unitCommandsMenu.GetComponent<Animator>().Play("SlideOut", -1, 1f);
    unitWait.GetComponent<Animator>().Play("SlideOut", -1, 1f);
    unitStatusKey.GetComponent<Animator>().Play("SlideOut", -1, 1f);
    unitCancelKey.GetComponent<Animator>().Play("SlideOut", -1, 1f);
    unitMoveKey.GetComponent<Animator>().Play("SlideOut", -1, 1f);
    unitStatus.GetComponent<Animator>().Play("SlideOut", -1, 1f);
    unitActions.GetComponent<Animator>().Play("SlideOut", -1, 1f);
  }

  private void UpdateUI() {

    // Update targetUnit and targetTile
    BattleUnit newTargetUnit = battle.GetUnit(transform.position);
    TileNode targetTile = battle.GetTile(transform.position);

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
          targetUnit.unit.SetPanelUI(unitPanelL);
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
      unitCommandsMenu.transform.Find("BackImage/MoveButton").gameObject.GetComponent<Button>().interactable = targetUnit.unit.apCur > 0;
      unitCommandsMenu.transform.Find("BackImage/ActButton").gameObject.GetComponent<Button>().interactable = targetUnit.unit.apCur > 0;
      unitCommandsMenu.transform.Find("BackImage/APImage/Text").gameObject.GetComponent<Text>().text = targetUnit.unit.apCur.ToString();
      Color apColor = new Color(24f / 256, 115f / 256, 20f / 256);

      if (targetUnit.unit.apCur == 1)
        apColor = new Color(0.75f, 0.75f, 0.25f);

      if (targetUnit.unit.apCur == 0)
        apColor = new Color(0.75f, 0.15f, 0.15f);

      unitCommandsMenu.transform.Find("BackImage/APImage/Text").gameObject.GetComponent<Text>().color = apColor;

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

    // UI: Unit Look Status Key
    Animator uskAnim = unitStatusKey.GetComponent<Animator>();
    AnimatorStateInfo uskAnimState = uskAnim.GetCurrentAnimatorStateInfo(0);

    if (!(onUnitLook && targetUnit != null) && uskAnimState.IsName("SlideIn")) {
      float animTime = Mathf.Max(1f - uskAnimState.normalizedTime, 0f);
      uskAnim.Play("SlideOut", -1, animTime);
    }

    if ((onUnitLook && targetUnit != null) && uskAnimState.IsName("SlideOut") && uskAnimState.normalizedTime > 1f) {
      uskAnim.Play("SlideIn");
    }

    // UI: Cancel key
    Animator uckAnim = unitCancelKey.GetComponent<Animator>();
    AnimatorStateInfo uckAnimState = uckAnim.GetCurrentAnimatorStateInfo(0);

    if (!(onUnitLook || onStatusScreen || onUnitMove) && uckAnimState.IsName("SlideIn")) {
      float animTime = Mathf.Max(1f - uckAnimState.normalizedTime, 0f);
      uckAnim.Play("SlideOut", -1, animTime);
    }

    if ((onUnitLook || onStatusScreen || onUnitMove) && uckAnimState.IsName("SlideOut") && uckAnimState.normalizedTime > 1f) {
      uckAnim.Play("SlideIn");
    }

    // UI: Unit Move Key
    Animator umkAnim = unitMoveKey.GetComponent<Animator>();
    AnimatorStateInfo umkAnimState = umkAnim.GetCurrentAnimatorStateInfo(0);

    if (!(onUnitMove && targetTiles.Contains(targetTile)) && umkAnimState.IsName("SlideIn")) {
      float animTime = Mathf.Max(1f - umkAnimState.normalizedTime, 0f);
      umkAnim.Play("SlideOut", -1, animTime);
    }

    if ((onUnitMove && targetTiles.Contains(targetTile)) && umkAnimState.IsName("SlideOut") && umkAnimState.normalizedTime > 1f) {
      umkAnim.Play("SlideIn");
    }

    // UI: Unit Status Screen
    Animator usAnim = unitStatus.GetComponent<Animator>();
    AnimatorStateInfo usAnimState = usAnim.GetCurrentAnimatorStateInfo(0);

    if (!onStatusScreen && usAnimState.IsName("SlideIn")) {
      float animTime = Mathf.Max(1f - usAnimState.normalizedTime, 0f);
      usAnim.Play("SlideOut", -1, animTime);
    }

    if (onStatusScreen && usAnimState.IsName("SlideOut") && usAnimState.normalizedTime > 1f) {
      newTargetUnit.unit.SetStatusUI(unitStatus);
      usAnim.Play("SlideIn");
    }

    // UI: Unit Action Menu
    Animator uaAnim = unitActions.GetComponent<Animator>();
    AnimatorStateInfo uaAnimState = uaAnim.GetCurrentAnimatorStateInfo(0);

    if (!onUnitActions && uaAnimState.IsName("SlideIn")) {
      float animTime = Mathf.Max(1f - uaAnimState.normalizedTime, 0f);
      uaAnim.Play("SlideOut", -1, animTime);
    }

    if (onUnitActions && uaAnimState.IsName("SlideOut") && uaAnimState.normalizedTime > 1f) {
      uaAnim.Play("SlideIn");
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
