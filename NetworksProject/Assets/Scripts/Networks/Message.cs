using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour {

    /** ~~~~~~~ MOVEMENT ~~~~~~~ **/
    /** Move to where we need you to go!
     */
    private float speed = 2f;
    private float minDistance = 0.1f;
    private Network network;
    private Vector3 destination;

	public void SetDestination(Network network, Vector3 destination) {
        this.network = network;
        this.destination = destination;
    }

    // Move toward destination until within minDistance
    // Then deposit message and self destruct
    public void MoveToDestination() {
        if (MoveTowardDestination()) {
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
	void Update () {
        MoveToDestination();
	}
}
