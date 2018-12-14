using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Network {

    public bool root;

    public List<Network> children;
    public Link link;
    public Node node;

    public Network(Link link, Node node) {
        children = new List<Network>();

        // The root node has no link
        if (link) {
            this.link = link;
            link.SetNetwork(this);
        }
        
        this.node = node;
        node.SetNetwork(this);
    }

    /** ~~~~~~~ CREATING SUB NETWORKS ~~~~~~~ **/
    Network tempNetwork;
    Vector3 tempLinkScaleXY = new Vector3(0.1f, 0.1f, 1f);
   
    // Called by node once on start of mouse drag
    public void CreateChildNetwork() {
        GameObject childLinkObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Link childLink = childLinkObject.AddComponent<Link>();

        GameObject childNodeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);        
        Node childNode = childNodeObject.AddComponent<Node>();

        tempNetwork = new Network(childLink, childNode);
    }

    // Called by node every frame during mouse drag
    public void PositionNewNetwork() {
        Vector3 position = MousePosToWorldPos();

        Vector3 diff = position - node.transform.position;
        Vector3 scale = new Vector3(
            tempLinkScaleXY.x, 
            tempLinkScaleXY.y,
            Vector3.Magnitude(diff)
        );
        // Links point toward the parentNetwork Node
        Quaternion rotation = Quaternion.LookRotation(-diff, Vector3.up);

        // link is placed halfway between new node and current node
        tempNetwork.link.SetTransform((position + node.transform.position) * 0.5f, scale, rotation);
        tempNetwork.node.transform.position = position;
    }

    // Temporary Network becomes an actual functioning Child Network
    // Called by node once at the end of mouse drag
    public void FinishChildNetwork() {
        Debug.Log("Placed!");
        children.Add(tempNetwork);
        tempNetwork = null;
    }

    // Convert mouse on screen to position in space
    private Vector3 MousePosToWorldPos() {
        // Need to provide distance to the ground in Z
        Vector3 mousePos = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.transform.position.y
        );
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // Ensure for now that Y is exactly 0
        return new Vector3(worldPos.x, 0, worldPos.z);
    }

}
