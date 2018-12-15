﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Network {

    public bool root;

    public Network parentNetwork;
    public List<Network> children;
    public Link link;
    public Node node;

    public Network(Network parentNetwork, Link link, Node node) {
        this.parentNetwork = parentNetwork; // null for root
        children = new List<Network>();

        // The root node has no link
        if (link) {
            this.link = link;
            link.SetNetwork(this);            
        }
        
        this.node = node;
        node.SetNetwork(this);
    }

    /** ~~~~~~~ COMMUNICATIONS ~~~~~~~ **/
    /** Every so often, outposts send messages regarding
     *  their status and surroundings.
     */

    // Called by parent Network on being placed
    public void BeginComms() {
        node.BeginComms();
    }

    /** ~~~~~~~ BEING CREATED ~~~~~~~ **/
    /** Probably plenty of things to be doing on creation, right?
     *  Start sending messages, for instance.
     */

    // called once on creation
    public void Placed() {
        // Take up the available grid spaces
        node.Place();

        // we only make the link a child of the node on being placed
        // as otherwise parent-child correlation conflicts with setting link position
        // during placement, and causes jerky movement
        link.transform.parent = node.transform;

        BeginComms();
    }

    /** ~~~~~~~ CREATING SUB NETWORKS ~~~~~~~ **/
    private Network tempNetwork;
    private Vector3 tempLinkScaleXY = new Vector3(0.1f, 0.1f, 1f);
   
    // Called by node once on start of mouse drag
    public void CreateChildNetwork() {
        GameObject childLinkObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Link childLink = childLinkObject.AddComponent<Link>();
        childLinkObject.name = "Link";

        GameObject childNodeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);        
        Node childNode = childNodeObject.AddComponent<Node>();
        childNodeObject.name = "Node";
        childNode.Created();

        tempNetwork = new Network(this, childLink, childNode);
    }

    // Called by node every frame during mouse drag
    public void PositionNewNetwork() {
        // SNAP TO GRID COORDS
        Vector3 position = CameraControl.MousePosToWorldPos();

        if (Settings.SNAP_TO_GRID) {
            position = Map.SnapToGrid(position);
        }

        Vector3 offset = new Vector3(0, node.scale.y * -0.5f, 0);
        Vector3 diff = position - (node.transform.position + offset);
        Vector3 scale = new Vector3(
            tempLinkScaleXY.x, 
            tempLinkScaleXY.y,
            Vector3.Magnitude(diff)
        );

        // Highlight accordingly
        tempNetwork.node.CanBePlaced();

        // Links point toward the parentNetwork Node
        // Check for zero difference to avoid "Look Rotation Viewing Vector is Zero" logs
        Quaternion rotation;
        if (diff == Vector3.zero) {
            rotation = Quaternion.Euler(Vector3.zero);
        }
        else {
            rotation = Quaternion.LookRotation(-diff, Vector3.up);
        }

        // link is placed halfway between new node and current node's base
        tempNetwork.link.SetTransform((position + node.transform.position) * 0.5f, scale, rotation);
        // Position adjusted by node
        tempNetwork.node.Placing(position);
    }

    // Temporary Network becomes an actual functioning Child Network
    // Called by node once at the end of mouse drag
    public void FinishChildNetwork() {
        children.Add(tempNetwork);
        tempNetwork.Placed();
        tempNetwork = null;
    }

}
