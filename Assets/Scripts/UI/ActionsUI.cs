using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsUI : MonoBehaviour {
    string selectedAction;
    Cat hoveringCat;
    Camera cam;
    Vector3 hitPoint;
    Transform snack;

    void Awake () {
        cam = Camera.main;
    }

    public void SelectAction(string name) {
        if (selectedAction == name) {
            selectedAction = null;
            return;
        }

        if (name == "Snack") ItemManager.Instance.snack.Show();

        selectedAction = name;
    }

    void Update() {
        if (selectedAction == null) return;

        RaycastCursor();

        if (selectedAction == "Sprinkler")
            UseSprinkler();
        if (selectedAction == "Snack")
            UseSnack();
    }

    void UseSprinkler() {
        if (Input.GetMouseButtonDown(0) && hoveringCat) {
            ItemManager.Instance.sprinkler.Use(hoveringCat);

            selectedAction = null;
        }
    }

    void UseSnack() {
        ItemManager.Instance.snack.Move(hitPoint);

        if (ItemManager.Instance.snack.dropped) selectedAction = null;

        if (Input.GetMouseButtonDown(0)) {
            ItemManager.Instance.snack.Use();
            selectedAction = null;
        }
    }

    void RaycastCursor() {
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, cam.farClipPlane)) {
            hitPoint = new Vector3(hit.point.x, 0f, hit.point.z);

            if (hit.collider.tag == "Cat")
                hoveringCat = hit.collider.GetComponent<Cat>();

            return;
        }

        hoveringCat = null;

        Debug.DrawRay(cam.transform.position, cam.ScreenPointToRay(Input.mousePosition).direction * cam.farClipPlane, Color.red);
    }
}
