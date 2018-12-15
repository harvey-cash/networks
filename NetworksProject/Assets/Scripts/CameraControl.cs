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
        lastMousePos = MousePosToWorldPos();
    }

    // Called each frame mouse is dragged
    private void OnMiddleMouseDrag() {
        Vector3 diff = lastMousePos - MousePosToWorldPos();
        gameObject.transform.position += diff;

        lastMousePos = MousePosToWorldPos();
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

    // Convert mouse on screen to position in space
    // Casted down to the terrain!
    public static Vector3 MousePosToWorldPos() {
        Vector3 worldPos = MouseToWorldCoords();

        // Bit shift the index of the layer (9) to get a bit mask
        int layerMask = 1 << 9;

        RaycastHit hit;
        // ~~~~~ Using Camera transform forward here breaks on Perspective Cameras ~~~~~ //
        Physics.Raycast(worldPos, Camera.main.transform.forward, out hit, Mathf.Infinity, layerMask);

        return hit.point;
    }
}
