using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wand : MonoBehaviour {
  /*
  Controller that handles game inputs and behaviors
  */

  // Object refs
  private BattleGrid battle;
  public WandCamera cam;

  void Start() {
    StartUI();
  }

  void Update() {

    if (battle == null)
      battle = Object.FindObjectOfType<BattleGrid>();

    if (whileUnitMove || whileUnitAction || whileAIControl)
      ResetWandToUnit();

    if (whileUnitMove && !battle.isMovingUnit)
      OnMoveFinish();

    if (whileUnitAction && !battle.isActingUnit)
      OnActionFinish();

    if (whileAIControl && !battle.isControllingUnit)
      PassUnit();

    KeyInputs();
    UpdateUI();
  }

  /* Game State */
  private BattleUnit targetUnit;

  public bool onUnitCommands = false;
  public bool onUnitWait = false;
  public bool onUnitLook = false;
  public bool onStatusScreen = false;
  public bool onUnitMove = false;
  public bool whileUnitMove = false;
  public bool onUnitActions = false;
  public bool onUnitAction = false;
  public bool whileUnitAction = false;
  public bool whileAIControl = false;

  private HashSet<TileNode> targetTiles;
  public GameObject tilePrefab;
  public Transform tileTransform;

  private Action currentAction;

  private void PassUnit() {
    battle.NextCurrentUnit();

    if (battle.currentUnit.team == 0) {
      onUnitCommands = true;
      whileAIControl = false;
    } else {
      onUnitCommands = false;
      whileAIControl = true;
      battle.isControllingUnit = true;
    }

    cam.SetMenuMode(true);
    ResetWandToUnit();
  }

  public bool CanMoveWand() {
    return onUnitLook || onUnitMove || onUnitAction;
  }

  private void ResetWandToUnit() {
    transform.position = battle.currentUnit.GetPos();
  }

  private void ClearTargetTiles() {
    SetTargetTiles(new HashSet<TileNode>());
  }

  private void SetTargetTiles(HashSet<TileNode> tiles) {
    targetTiles = tiles;

    // Delete tiles
    foreach (Transform tile in tileTransform)
      Destroy(tile.gameObject);

    // Place tiles
    foreach (TileNode tile in targetTiles)
      Instantiate(tilePrefab, tile.GetPos(), Quaternion.identity, tileTransform);
  }


  /* Player OnInput Functions */
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
    onUnitLook = true;
    onUnitCommands = false;
    cam.SetMenuMode(false);
  }

  public void OnLookCancel() {
    onUnitLook = false;
    onUnitCommands = true;
    cam.SetMenuMode(true);
    ResetWandToUnit();
  }

  public void OnStatus() {
    onStatusScreen = true;
    onUnitLook = false;
    cam.SetMenuMode(true);
  }

  public void OnStatusCancel() {
    onStatusScreen = false;
    onUnitLook = true;
    cam.SetMenuMode(false);
  }

  public void OnAct() {
    onUnitActions = true;
    onUnitCommands = false;
  }

  public void OnActCancel() {
    onUnitActions = false;
    onUnitCommands = true;
  }

  public void OnAction(Action action) {
    onUnitAction = true;
    onUnitActions = false;
    cam.SetMenuMode(false);

    currentAction = action;
    SetTargetTiles(currentAction.GetSelectionTiles(battle.currentUnit, battle));
  }

  public void OnActionConfirm() {
    Debug.Log(currentAction);
    onUnitAction = false;
    whileUnitAction = true;

    TileNode target = battle.GetTile(transform.position);
    battle.ActUnit(battle.currentUnit, currentAction, target);

    cam.SetMenuMode(true);
    ResetWandToUnit();
    ClearTargetTiles();
  }

  public void OnActionFinish() {
    whileUnitAction = false;
    onUnitCommands = true;
  }

  public void OnActionCancel() {
    onUnitAction = false;
    onUnitActions = true;
    cam.SetMenuMode(true);
    ResetWandToUnit();
    ClearTargetTiles();
  }

  public void OnMove() {
    onUnitMove = true;
    onUnitCommands = false;
    cam.SetMenuMode(false);
    SetTargetTiles(battle.GetMoveTiles(battle.currentUnit));
  }

  public void OnMoveConfirm() {
    whileUnitMove = true;
    onUnitMove = false;

    // Move unit
    TileNode end = battle.GetTile(transform.position);
    List<TileNode> path = battle.GetMovePath(battle.currentUnit, end);
    battle.MoveUnit(battle.currentUnit, path);

    cam.SetMenuMode(true);
    ClearTargetTiles();
  }

  public void OnMoveFinish() {
    onUnitCommands = true;
    whileUnitMove = false;
  }

  public void OnMoveCancel() {
    onUnitMove = false;
    onUnitCommands = true;
    cam.SetMenuMode(true);
    ResetWandToUnit();
    ClearTargetTiles();
  }


  /* Key Inputs */
  public void KeyInputs() {

    if (Input.GetKeyDown("n")) {
      PassUnit();
    }

    if (Input.GetKeyDown("x")) {

      if (onUnitLook && targetUnit != null)
        OnStatus();

      if (onUnitMove && targetTiles.Contains(battle.GetTile(transform.position)))
        OnMoveConfirm();

      if (onUnitAction) {
        if (currentAction.GetActionType() == ActionType.Targeted && targetTiles.Contains(battle.GetTile(transform.position)))
          OnActionConfirm();

        if (currentAction.GetActionType() == ActionType.Fixed)
          OnActionConfirm();
      }
    }

    if (Input.GetKeyDown("z")) {

      if (onUnitLook)
        OnLookCancel();

      if (onStatusScreen)
        OnStatusCancel();

      if (onUnitMove)
        OnMoveCancel();

      if (onUnitAction)
        OnActionCancel();
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
  public Transform unitActionsContent;

  public GameObject unitActionKey;

  private void StartUI() {
    unitPanelL.GetComponent<Animator>().Play("SlideOut", -1, 1f);
    unitCommandsMenu.GetComponent<Animator>().Play("SlideOut", -1, 1f);
    unitWait.GetComponent<Animator>().Play("SlideOut", -1, 1f);
    unitStatusKey.GetComponent<Animator>().Play("SlideOut", -1, 1f);
    unitCancelKey.GetComponent<Animator>().Play("SlideOut", -1, 1f);
    unitMoveKey.GetComponent<Animator>().Play("SlideOut", -1, 1f);
    unitStatus.GetComponent<Animator>().Play("SlideOut", -1, 1f);
    unitActions.GetComponent<Animator>().Play("SlideOut", -1, 1f);
    unitActionKey.GetComponent<Animator>().Play("SlideOut", -1, 1f);
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

      if (!(whileUnitMove || whileUnitAction) && uplAnimState.IsName("SlideOut") && uplAnimState.normalizedTime > 1f) {

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

    if (!(onUnitLook || onStatusScreen || onUnitMove || onUnitAction) && uckAnimState.IsName("SlideIn")) {
      float animTime = Mathf.Max(1f - uckAnimState.normalizedTime, 0f);
      uckAnim.Play("SlideOut", -1, animTime);
    }

    if ((onUnitLook || onStatusScreen || onUnitMove || onUnitAction) && uckAnimState.IsName("SlideOut") && uckAnimState.normalizedTime > 1f) {
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

    // UI: Unit Action Key
    Animator uakAnim = unitActionKey.GetComponent<Animator>();
    AnimatorStateInfo uakAnimState = uakAnim.GetCurrentAnimatorStateInfo(0);
    bool canConfirmAction = (onUnitAction && (currentAction.GetActionType() == ActionType.Fixed
                             || (currentAction.GetActionType() == ActionType.Targeted && targetTiles.Contains(targetTile))));

    if (!canConfirmAction && uakAnimState.IsName("SlideIn")) {
      float animTime = Mathf.Max(1f - uakAnimState.normalizedTime, 0f);
      uakAnim.Play("SlideOut", -1, animTime);
    }

    if (canConfirmAction && uakAnimState.IsName("SlideOut") && uakAnimState.normalizedTime > 1f) {
      uakAnim.Play("SlideIn");
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

      foreach (Transform action in unitActionsContent)
        Destroy(action.gameObject);

      foreach (Action action in targetUnit.unit.actions) {
        GameObject go = Instantiate(unitActionPrefab, unitActionsContent);
        go.GetComponent<Text>().text = action.GetName();
        go.GetComponent<Button>().onClick.AddListener(() => OnAction(action));
      }

      uaAnim.Play("SlideIn");
    }
  }
}
