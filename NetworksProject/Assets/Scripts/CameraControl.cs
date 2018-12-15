using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    bool canMove = true;
    Vector3 lastMousePos;
    float scrollMultiplier = 4;

    // Middle mouse camera controls
    private void Update() {
        if (canMove) {
            ControlCamera();
        }        
    }

    private void ControlCamera() {
        if (Input.GetMouseButtonDown(2)) {
            OnMiddleMouseDown();
        }
        if (Input.GetMouseButton(2)) {
            OnMiddleMouseDrag();
        }
        ControlZoom();
    }

    private void ControlZoom() {
        var deltaScroll = Input.GetAxis("Mouse ScrollWheel");

        // Perspective zoom is movement!
        if (!GetComponent<Camera>().orthographic) {
            gameObject.transform.position += gameObject.transform.forward * deltaScroll;
        }
        else {
            GetComponent<Camera>().orthographicSize -= deltaScroll * scrollMultiplier;
        }        
    }

    // Called once when middle mouse is pressed down
    private void OnMiddleMouseDown() {
        lastMousePos = MouseToWorldCoords();
    }

    // Called each frame mouse is dragged
    // ~~~~~~~~~~~ DAMN ROTATIONS ~~~~~~~~~~~~
    private void OnMiddleMouseDrag() {
        Vector3 diff = lastMousePos - MouseToWorldCoords();
        diff = new Vector3(
            diff.x,
            0,
            diff.z * (1 / Mathf.Cos(gameObject.transform.eulerAngles.x))
        );
        gameObject.transform.position += diff;

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
