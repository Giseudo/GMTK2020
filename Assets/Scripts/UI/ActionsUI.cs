using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsUI : MonoBehaviour {
    string selectedAction;
    Camera cam;

    void Awake () {
        cam = Camera.main;
    }

    public void SelectAction(string name) {
        if (selectedAction == name) {
            selectedAction = null;
            return;
        }

        selectedAction = name;
    }

    void Update() {
        if (selectedAction == "Sprinkler")
            RaycastCursor();
    }

    void RaycastCursor() {
        if (Input.GetMouseButtonDown(0)) {
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit)) {
                if (hit.collider.tag == "Cat") {
                    Cat cat = hit.collider.GetComponent<Cat>();

                    ItemManager.Sprinkler.Use(cat);

                    selectedAction = null;
                }
            }
        }

        // Debug.DrawRay(cam.transform.position, cam.ScreenPointToRay(Input.mousePosition).direction * cam.farClipPlane, Color.red);
    }
}
