using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Messenger : MonoBehaviour {
    /** Messages are the most important aspect of the game
     * The player can only learn, see, find out anything by receiving a message
     * Messages collect data about their surroundings on creation,
     * and relay the data directly to the destination Network.
     * 
     * It is a network's job to forward messages onwards towards the capital.
     * By the time messages arrive, they may well present out of date data.
     */

    private Network targetNetwork;

    /** ~~~~~~~ DATA COLLECTION ~~~~~~~ **/
    /** What objects are in the area?
     */
    public float visibilityRadius = 5f;
    public Message message;

    // Called when a new messenger is created by a node
    // In order to pass along a received message
    public void SetMessage(Message message) {
        this.message = message;
    }

    // Avoid being collected by self and other messengers
    private void Start() {
        gameObject.layer = LayerMask.NameToLayer("Messages");
    }

    // Get all the objects within the visibility radius (need to have a collider!)
    // and create ghost copies of them.
    public void CollectData() {
        Collider[] visibleColliders = Physics.OverlapSphere(
            gameObject.transform.position,
            visibilityRadius,
            LayerMask.GetMask(new string[] { "Viewable" })
        );
        Ghost[] ghosts = new Ghost[visibleColliders.Length];

        for (int i = 0; i < visibleColliders.Length; i++) {
            GameObject ghostObject = Instantiate(visibleColliders[i].gameObject, null);
            ghosts[i] = ghostObject.AddComponent<Ghost>();

            // Ensure the ghost knows which object it is representing
            ghosts[i].representing = visibleColliders[i].GetComponent<Viewable>();
        }

        SetMessage(new Message(ghosts));
    }

    /** ~~~~~~~ MOVEMENT ~~~~~~~ **/
    /** Move to where we need you to go!
     */
    private float speed = 5f;
    private float minDistance = 0.1f;
    private Vector3 destination;

    public void SetDestination(Network network, Vector3 destination) {
        this.targetNetwork = network;
        this.destination = destination;
    }

    // Move toward destination until within minDistance
    // Then deposit message and self destruct
    public void MoveToDestination() {
        if (MoveTowardDestination()) {
            DepositMessage();
            Destroy(this.gameObject);
        }
    }

    // Make progress towards the destination
    // Returns true once close enough
    public bool MoveTowardDestination() {
        Vector3 forward = destination - transform.position;
        if (Vector3.Magnitude(forward) < minDistance) {
            return true;
        }
        transform.position += Vector3.Normalize(forward) * speed * Time.deltaTime;
        return false;
    }

    // Update is called once per frame
    void Update() {
        MoveToDestination();
    }

    // Once at destination, we deposit our message
    // and self destruct!
    private void DepositMessage() {
        targetNetwork.node.ReceiveMessage(message);
    }
}
