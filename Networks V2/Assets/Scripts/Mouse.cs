using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    public const int LEFT = 0, RIGHT = 1, MIDDLE = 2;

    // Used by Network to raycast to ground
    public static Vector3 ToWorldCoords() {
        Vector3 mousePos = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            0
        );
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    // Convert mouse on screen to position in space
    // Casted down to the terrain!
    public static Vector3 ToGroundPos() {
        Vector3 worldPos = ToWorldCoords();
        int layerMask = LayerMask.GetMask("Ground");

        RaycastHit hit;
        // ~~~~~ Using Camera transform forward here breaks on Perspective Cameras ~~~~~ //
        Physics.Raycast(worldPos, Camera.main.transform.forward, out hit, Mathf.Infinity, layerMask);

        return hit.point;
    }
}
