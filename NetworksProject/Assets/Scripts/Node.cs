using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public Network network;
	public void SetNetwork (Network network) {
        this.network = network;
    }

    /** ~~~~~~~ COMMUNICATIONS ~~~~~~~ **/
    /** Every so often, outposts send messages regarding
     *  their status and surroundings.
     */
    bool communicate = true;
    float commsIntervalSeconds = 3f;
    Vector3 messageScale = new Vector3(0.5f, 0.5f, 0.5f);

    // Called by Network on being placed
    public void BeginComms() {
        StartCoroutine(CommsLoop());
    }

    // Beware of while loops, they can completely crash Unity.
    private IEnumerator CommsLoop() {
        while(communicate) {
            SendMessageUp();
            yield return new WaitForSeconds(commsIntervalSeconds);
        }        
    }

    // Create a message object which moves towards the parentNetwork's node
    // Until it gets close enough to be received
    // Called during the CommsLoop
    private void SendMessageUp() {
        Debug.Log("hello!?");

        GameObject messageObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        messageObject.transform.position = gameObject.transform.position;
        messageObject.transform.localScale = messageScale;
        messageObject.name = "Message";

        Message message = messageObject.AddComponent<Message>();
        message.SetDestination(network.parentNetwork, network.parentNetwork.node.transform.position);
    }

    /** ~~~~~~~ PLAYER INTERACTION ~~~~~~~ **/
    /** Players can click and inspect nodes and whatnot
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
        network.FinishChildNetwork();
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
                network.CreateChildNetwork();
            }

            // Called every frame from now on until MouseUp
            network.PositionNewNetwork();            
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
