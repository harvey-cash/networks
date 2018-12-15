using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : Viewable {

    public Network network;
	public void SetNetwork (Network network) {
        this.network = network;
    }

    /** ~~~~~~~ PHYSICAL SIZE ETC ~~~~~~~ **/
    /** Every so often, outposts send messages regarding
     *  their status and surroundings.
     */
    public Vector3 scale = new Vector3(1, 0.5f, 1);
    // Relative to object position, which unit cells get filled?
    public Vector3[] fillSpaces = new Vector3[] {
        Vector3.zero,
        Vector3.forward,
        Vector3.forward + Vector3.right,
        Vector3.right,
        Vector3.right + Vector3.back,
        Vector3.back,
        Vector3.back + Vector3.left,
        Vector3.left,
        Vector3.left + Vector3.forward
    };
    
    // Called by own network on its creation
    public void Place() {

        Vector3 position = Map.SnapToGrid(gameObject.transform.position);
        bool success = Map.SetWorldCells(
            gameObject.transform.position,
            fillSpaces,
            Map.SPACE_LAYER,
            Map.SPACE_FULL
        );

        if (success) {
            gameObject.GetComponent<Renderer>().material.color = Color.white;
            StartCoroutine(MakeCash());

            // We don't need to be able to ghostify the root node, but
            // we do need to ghostify all else!
            if (!network.root) {
                gameObject.layer = LayerMask.NameToLayer("Viewable");
            }
        }
        else {
            throw new System.Exception("Node Place() fucked up!");
        }
    }

    private IEnumerator MakeCash() {
        while(communicate) {
            yield return new WaitForSeconds(Settings.cashTime);
            Player.SumCash(Settings.cashPerTime);
        }        
    }

    public int CalculateCost(Node parentNode) {
        int linkLength = (int)Mathf.Ceil(Vector3.Magnitude(transform.position - parentNode.transform.position));
        return Settings.townCost + (Settings.linkPricePerUnit * linkLength);
    }

    // Called on destruction / movement?
    // ~~~~ DO NOT MOVE NODES WITHOUT UPDATING GRID ELSEWHERE ~~~~~ //
    public void UnPlace() {
        bool success = Map.SetWorldCells(
            gameObject.transform.position, 
            fillSpaces,
            Map.SPACE_LAYER, 
            Map.SPACE_EMPTY
        );

        if (!success) {
            throw new System.Exception("Node UnPlace fucked up!");
        }
    }

    // Return whether can be placed or not
    // Highlight accordingly whilst being placed
    // Called by network when being placed
    public bool CanBePlaced() {
        int[] cells = Map.GetWorldCells(
            gameObject.transform.position, 
            fillSpaces,
            Map.SPACE_LAYER
        );

        // Check through the values. If any are occupied, can't place!
        bool occupied = false;
        for (int i = 0; i < cells.Length; i++) {
            if (cells[i] == Map.SPACE_FULL) {
                occupied = true;
            }
        }

        // Highlight correctly
        if (!occupied) {
            gameObject.GetComponent<Renderer>().material = (Material)Resources.Load("Materials/CanPlaceNode");
            return true;
        }
        else {
            gameObject.GetComponent<Renderer>().material = (Material)Resources.Load("Materials/CanNotPlaceNode");
            return false;
        }
    }

    // Called once when tempNetwork is first created
    public void Created() {
        
        gameObject.layer = LayerMask.NameToLayer("OnTop");
        gameObject.GetComponent<Renderer>().material = (Material)Resources.Load("Materials/PlacedNode");
        gameObject.transform.localScale = scale;

        // When placed, we don't actually know it's there until
        // we receive a Message (and Ghost) back, so it's invisible!
        gameObject.GetComponent<Renderer>().enabled = false;
    }

    // Called every frame by Network during mouse drag
    public void Placing(Vector3 pos) {
        gameObject.transform.position = pos + (new Vector3(0, scale.y / 2f, 0));
    }

    /** ~~~~~~~ COMMUNICATIONS ~~~~~~~ **/
    /** Every so often, outposts send messages regarding
     *  their status and surroundings.
     */
    bool communicate = true;
    float commsIntervalSeconds = 10f;
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

    // If we receive a message, pass it up to our Network
    public void ReceiveMessage(Message message) {
        network.ReceiveMessage(message);
    }

    private Messenger CreateMessenger() {
        GameObject messengerObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        messengerObject.transform.position = gameObject.transform.position;
        messengerObject.transform.localScale = messageScale;
        messengerObject.name = "Messenger";

        // Material!
        Material mat = (Material)Resources.Load("Materials/Messenger");
        messengerObject.GetComponent<Renderer>().material = mat;
        messengerObject.GetComponent<Renderer>().enabled = false;

        Messenger messenger = messengerObject.AddComponent<Messenger>();
        messenger.SetColor(mat.color);

        return messenger;
    }

    // Create a messenger object which moves towards the parentNetwork's node
    // Until it gets close enough to be received
    // Called during the CommsLoop
    private void SendMessageUp() {
        Messenger messenger = CreateMessenger();
        messenger.CollectData();
        messenger.SetDestination(network.parentNetwork, network.parentNetwork.node.transform.position);
    }

    // publicly accessible version, for
    // passing along existing messages
    public void SendMessageUp(Message message) {
        Messenger messenger = CreateMessenger();
        messenger.SetMessage(message);
        messenger.SetDestination(network.parentNetwork, network.parentNetwork.node.transform.position);
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
        network.PlaceChildNetwork();
    }

    // Called on Cancel by network
    public void Destroy() {
        Destroy(this.gameObject);
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
