using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Button sprinkleButton;
    public Button feedButton;

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

    void EnableActions() {
        snackButton.interactable = true;
        yarnButton.interactable = true;
        sprinkleButton.interactable = true;
        feedButton.interactable = false;
    }

    void DisableActions(List<Cat> deadCats) {
        snackButton.interactable = false;
        yarnButton.interactable = false;
        sprinkleButton.interactable = false;
        feedButton.interactable = true;
    }

    void SelectSprinkler() {
        sprinklerCursor.gameObject.SetActive(true);
        sprinklerCursor.position = mousePos;

        if (Input.GetMouseButtonDown(0) && hoveringCat) {
            sprinklerCursor.gameObject.SetActive(false);
            ItemManager.Instance.sprinkler.Use(hoveringCat);

            selectedAction = null;
        }
    }

    void SelectSnack() {
        snacksCursor.gameObject.SetActive(true);
        snacksCursor.position = mousePos;
        ItemManager.Instance.snack.Move(hitPoint);

        if (ItemManager.Instance.snack.dropped) {
            snacksCursor.gameObject.SetActive(false);
            selectedAction = null;
        }

        if (Input.GetMouseButtonDown(0)) {
            snacksCursor.gameObject.SetActive(false);
            ItemManager.Instance.snack.Use();
            selectedAction = null;
        }
    }

    void SelectYarnBall() {
        yarnBallCursor.gameObject.SetActive(true);
        yarnBallCursor.position = mousePos;

        if (Input.GetMouseButtonDown(0)) {
            yarnBallCursor.gameObject.SetActive(false);
            ItemManager.Instance.yarnBall.Use(mousePos, hitPoint);
            selectedAction = null;
            StopCoroutine("DisableYarnBall");
            StartCoroutine("DisableYarnBall");
        }
    }

    IEnumerator DisableYarnBall() {
        yield return new WaitForSeconds(20f);
        ItemManager.Instance.yarnBall.Hide();
    }

    public void FeedCats() {
        GameManager.Instance.StartRound();
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

        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, cam.farClipPlane)) {
            hitPoint = new Vector3(hit.point.x, 0f, hit.point.z);

            if (hit.collider.tag == "Cat")
                hoveringCat = hit.collider.GetComponent<Cat>();

            return;
        }

        hoveringCat = null;
    }
}
