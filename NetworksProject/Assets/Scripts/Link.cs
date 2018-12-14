using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : Selectable {

    public Network parent;

    public Link(Network parent) {
        this.parent = parent;
    }

    // On click
    public override void MouseDown() {
        Debug.Log(gameObject.name + ": \"You clicked on me! How could you!?\"");
    }
    public override void MouseHeld() {

    }
    public override void MouseUp() {

    }
}
