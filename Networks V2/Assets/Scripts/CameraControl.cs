using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    private bool canMove = true;
    private Vector3 lastMousePos;
    private float scrollMultiplier = 4;
    private float slideMultiplier = 0.2f;

    private float orthSizeMin = 1, orthSizeMax = 12;

    // Middle mouse camera controls
    private void Update() {
        if (canMove) {
            ControlCamera();
            ControlZoom();
        }        
    }

    // Called each frame, if canMove
    private void ControlCamera() {
        // Button index two is mouse wheel
        if (Input.GetMouseButtonDown(2)) {
            OnMiddleMouseDown();
        }
        if (Input.GetMouseButton(2)) {
            OnMiddleMouseDrag();
        }

        // Cancel out vertical movement
        Vector3 x = Input.GetAxis("Horizontal") * gameObject.transform.right;
        Vector3 z = Input.GetAxis("Vertical") * gameObject.transform.up;
        Vector3 move = (x + z) * slideMultiplier;
        gameObject.transform.position += new Vector3(move.x, 0, move.z);
        
    }

    private void ControlZoom() {
        var deltaScroll = Input.GetAxis("Mouse ScrollWheel");

        // Perspective zoom is movement!
        if (!GetComponent<Camera>().orthographic) {
            gameObject.transform.position += gameObject.transform.forward * deltaScroll;
        }
        else {
            GetComponent<Camera>().orthographicSize -= deltaScroll * scrollMultiplier;

            // As orthographic size increases, the view plane clips the ground eventually
            if (GetComponent<Camera>().orthographicSize < orthSizeMin) {
                GetComponent<Camera>().orthographicSize = orthSizeMin;
            }
            if (GetComponent<Camera>().orthographicSize > orthSizeMax) {
                GetComponent<Camera>().orthographicSize = orthSizeMax;
            }
        }        
    }

    // Called once when middle mouse is pressed down
    private void OnMiddleMouseDown() {
        lastMousePos = Mouse.ToGroundPos();
    }

    // Called each frame mouse is dragged
    private void OnMiddleMouseDrag() {
        Vector3 diff = lastMousePos - Mouse.ToGroundPos();
        gameObject.transform.position += diff;

        lastMousePos = Mouse.ToGroundPos();
    }
}
