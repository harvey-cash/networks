using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkEditor : MonoBehaviour {

    public static Selectable mouseSelection;

    // Interact with existing thing, or make new thing
    private void OnMouseDown() {
        if (mouseSelection == null) {
            // Clicked elsewhere
        }
        else {
            mouseSelection.MouseDown();
        }
    }
    
    // Called per frame of mouse held
    private void OnMouseDrag() {
        if (mouseSelection == null) {

        }
        else {
            mouseSelection.MouseHeld();
        }
    }

}
