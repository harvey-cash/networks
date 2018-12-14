using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Network : Selectable {

    public Node node;
    public Link link;
    public List<Network> children;

    private Vector3 startPos;    

    public void AddChild(Network network) {
        children.Add(network);
    }

    public Network() {        
        node = new Node();
        link = new Link(this);
        children = new List<Network>();
    }
    public Network(Node node) {
        this.node = node;
        children = new List<Network>();
    }

    Network tempNetwork = new Network();
    // Called per frame as a new network is created    
    private void NewNetwork() {
        // "Do we have funds? How long is this being?" Etc etc.
    }
    // Called once on mouse up if making a new network
    private void FinishNewNetwork() {

    }

    // Called while unclear if mouse is clicking or dragging
    private void AboutToClick() {

    }
    // Called once on Mouse Up if clicking
    private void Clicked() {

    }

    // Set mouse pos for click / drag comparison
    public override void MouseDown() {
        startPos = Input.mousePosition;
        makingNewNetwork = false;
    }

    // If static click, then open menu
    // If drag, create new network?
    bool makingNewNetwork = false;
    public override void MouseHeld() {
        // Once we've dragged enough, we're committed to making a new network
        if (makingNewNetwork || Vector3.SqrMagnitude(Input.mousePosition - startPos) > 10) {
            makingNewNetwork = true;
            NewNetwork();
        }
        else {
            AboutToClick();
        }

    }

    // Called once on mouse up
    public override void MouseUp() {
        if (makingNewNetwork) {
            FinishNewNetwork();
            makingNewNetwork = false;
        }
        else {
            Clicked();
        }
    }

}
