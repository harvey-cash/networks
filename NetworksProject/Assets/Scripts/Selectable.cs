using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Selectable : MonoBehaviour {

    // Select us
    private void OnMouseEnter() {
        // <-- Highlight
        NetworkEditor.mouseSelection = this;
    }

    // We're no longer the selection
    private void OnMouseExit() {
        if (NetworkEditor.mouseSelection == this) {
            NetworkEditor.mouseSelection = null;
        }
    }

    // Called by NetworkEditor
    public abstract void MouseDown();
    public abstract void MouseHeld();
    public abstract void MouseUp();

}
