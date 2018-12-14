using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public Network parentNetwork;
	public void SetNetwork (Network network) {
        parentNetwork = network;
    }

    /** ~~~~~~~ PLAYER INTERACTION ~~~~~~~ **/
    /** 
     */

    // Zoom in and show more detail, etc etc.
    // Called once on click (not drag)
    private void Clicked() {
        Debug.Log("Showing info!");
        // <-- ZOOM IN
        // <-- DETAILS
    }

    // Complete new link and whatnot
    // Called once on mouse up, if dragged
    private void NewNetwork() {
        parentNetwork.FinishChildNetwork();
    }

    /** ~~~~~~~ MOUSE INTERACTION ~~~~~~~ **/
    /** One click without mouse drag is to inspect an object.
     *  A click and drag is to create a child object.
     */
    Vector3 startMousePosition;
    bool beingDragged = false;
    float dragDistance = 10f;

    // Highlight, show name and stats perhaps, etc etc.
    // Called once on mouse over
    private void OnMouseEnter() {
        // <-- HIGHLIGHT
        // <-- POPUP STATS
    }

    // Un-Highlight, hide any popups
    // Called once on mouse exit
    private void OnMouseExit() {
        // <-- HIGHLIGHTING
        // <-- POPUP STATS
    }

    // Are we being clicked, or dragged?
    // Called once on mouse click
    private void OnMouseDown() {
        startMousePosition = Input.mousePosition;
        beingDragged = false; // reset to false for check
    }

    // Decide whether a click or a drag
    // Called every frame on mouse drag
    private void OnMouseDrag() {        
        if (beingDragged || Vector3.SqrMagnitude(startMousePosition - Input.mousePosition) > dragDistance) {

            // Gets called once
            if (!beingDragged) {
                beingDragged = true;
                parentNetwork.CreateChildNetwork();
            }

            // Called every frame from now on until MouseUp
            parentNetwork.PositionNewNetwork();            
        }
    }

    // If beingDragged is false on MouseUp, it must've been a click
    // Called once on mouse up
    private void OnMouseUp() {
        if (!beingDragged) {
            Clicked();
        }
        else {
            NewNetwork();
        }
    }

}
