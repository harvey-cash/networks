using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    Vector3 lastMousePos;

    // Middle mouse camera controls
    private void Update() {
        if (Input.GetMouseButtonDown(2)) {
            OnMiddleMouseDown();
        }
        if (Input.GetMouseButton(2)) {
            OnMiddleMouseDrag();
        }
    }

    // Called once when middle mouse is pressed down
    private void OnMiddleMouseDown() {
        lastMousePos = MouseToWorldCoords();
    }

    // Called each frame mouse is dragged
    private void OnMiddleMouseDrag() {
        gameObject.transform.position += lastMousePos - MouseToWorldCoords();
        lastMousePos = MouseToWorldCoords();
    }

    // Used by Network to raycast to ground
    public static Vector3 MouseToWorldCoords() {
        Vector3 mousePos = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            0
        );
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
