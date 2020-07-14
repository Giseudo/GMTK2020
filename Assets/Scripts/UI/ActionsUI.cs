using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class ActionsUI : MonoBehaviour {
    string selectedAction;
    Cat hoveringCat;
    Camera cam;
    Vector3 hitPoint;
    Vector3 mousePos;
    Transform snack;
    public Canvas parentCanvas;
    public Transform snacksCursor;
    public Transform sprinklerCursor;
    public Transform yarnBallCursor;
    public Button snackButton;
    public Button yarnButton;
    public Button sprinklerButton;

    void Awake () {
        cam = Camera.main;
    }

    void Start () {
        GameManager.Instance.onRoundStart += EnableActions;
        GameManager.Instance.onRoundEnd += DisableActions;
    }

    public void SelectAction(string name) {
        if (!GameManager.Instance.IsPlaying) return;

        if (selectedAction == name) {
            selectedAction = null;
            return;
        }

        if (name == "Snack") {
            ItemManager.Instance.snack.Show(hitPoint);
        }

        selectedAction = name;
    }

    void Update() {
        RaycastWorld();

        if (selectedAction == null) return;

        MouseCursor();

        if (selectedAction == "Sprinkler")
            SelectSprinkler();
        if (selectedAction == "Snack")
            SelectSnack();
        if (selectedAction == "YarnBall")
            SelectYarnBall();
    }

    void LateUpdate() {
        UpdateItemsAmount();
    }

    void EnableActions() {
        snackButton.interactable = true;
        yarnButton.interactable = true;
        sprinklerButton.interactable = true;
    }

    void DisableActions(List<Cat> deadCats) {
        snackButton.interactable = false;
        yarnButton.interactable = false;
        sprinklerButton.interactable = false;
    }

    void SelectSprinkler() {
        if (ItemManager.Instance.sprinkler.amount == 0) return;

        sprinklerCursor.gameObject.SetActive(true);
        sprinklerCursor.position = mousePos;

        if (Input.GetMouseButtonDown(0) && hitPoint.magnitude > 0f) {
            sprinklerCursor.gameObject.SetActive(false);
            ItemManager.Instance.sprinkler.Use(hitPoint);

            selectedAction = null;
        }
    }

    void SelectSnack() {
        if (ItemManager.Instance.snack.amount == 0) return;

        snacksCursor.gameObject.SetActive(true);
        snacksCursor.position = mousePos;
        ItemManager.Instance.snack.Move(hitPoint);

        if (ItemManager.Instance.snack.dropped) {
            snacksCursor.gameObject.SetActive(false);
            selectedAction = null;
        }

        if (Input.GetMouseButtonDown(0) && hitPoint.magnitude > 0f) {
            snacksCursor.gameObject.SetActive(false);
            ItemManager.Instance.snack.Use();
            selectedAction = null;
        }
    }

    void SelectYarnBall() {
        if (ItemManager.Instance.yarnBall.amount == 0) return;

        yarnBallCursor.gameObject.SetActive(true);
        yarnBallCursor.position = mousePos;

        if (Input.GetMouseButtonDown(0) && hitPoint.magnitude > 0f) {
            yarnBallCursor.gameObject.SetActive(false);
            ItemManager.Instance.yarnBall.Use(mousePos, hitPoint);
            selectedAction = null;
        }
    }

    void MouseCursor() {
        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            Input.mousePosition, parentCanvas.worldCamera,
            out movePos);

        mousePos = parentCanvas.transform.TransformPoint(movePos);
    }

    void RaycastWorld() {
        Debug.DrawRay(cam.transform.position, cam.ScreenPointToRay(Input.mousePosition).direction * cam.farClipPlane, Color.red);

        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 50f)) {
            NavMeshHit navmeshHit;

            if (NavMesh.SamplePosition(hit.point, out navmeshHit, 1f, 1)) {
                hitPoint = navmeshHit.position;
                return;
            }
        }

        hitPoint = Vector3.zero;
    }

    void UpdateItemsAmount() {
        int yarnAmount = ItemManager.Instance.yarnBall.amount;
        int snackAmount = ItemManager.Instance.snack.amount;
        int sprinklerAmount = ItemManager.Instance.sprinkler.amount;

        if (yarnAmount == 0)
            yarnButton.interactable = false;

        if (snackAmount == 0)
            snackButton.interactable = false;

        if (sprinklerAmount == 0)
            sprinklerButton.interactable = false;

        yarnButton.GetComponentInChildren<Text>().text = yarnAmount.ToString();
        snackButton.GetComponentInChildren<Text>().text = snackAmount.ToString();
        sprinklerButton.GetComponentInChildren<Text>().text = sprinklerAmount.ToString();
    }
}
