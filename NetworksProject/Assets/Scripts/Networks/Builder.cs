using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour {

    /** When attempting to create a network,
     * builders need to be sent out first to finish planned Nodes
     */
    private Network targetNetwork;
    private Network rootNetwork;
    private float visibilityRadius = 10f;
    private float speed = 5f;
    private float minDistance = 0.1f;
    private Vector3 destination;

    private Color color;
    public void SetColor(Color color) {
        this.color = color;
    }

    // Called on creation
    public void SetDestination(Network network, Vector3 destination) {
        rootNetwork = Root.root.network;
        this.targetNetwork = network;
        this.destination = destination;
    }

    // Move toward destination until within minDistance
    // Then deposit message and self destruct
    public void MoveToDestination() {
        if (MoveTowardDestination()) {
            targetNetwork.CompleteNode();
            Destroy(this.gameObject);
        }
    }

    // Make progress towards the destination
    // Assumes straight line
    // Returns true once close enough
    public bool MoveTowardDestination() {
        Vector3 forward = destination - transform.position;
        float distance = Vector3.Magnitude(forward);
        float distanceFromRoot = Vector3.Magnitude(rootNetwork.node.transform.position - transform.position);

        // Builders become invisible as they leave the root
        if (distanceFromRoot < visibilityRadius) {
            GetComponent<Renderer>().enabled = true;

            float u = 1 - (distanceFromRoot / visibilityRadius);
            Color newColor = new Color(color.r, color.g, color.b, u);
            GetComponent<Renderer>().material.color = newColor;
        } else {
            GetComponent<Renderer>().enabled = false;
        }

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
}
