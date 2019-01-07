using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private bool mouseOver;
    private void OnMouseEnter() { mouseOver = true; }
    private void OnMouseExit() { mouseOver = false; }    
    
    private void Update() {
        ManageBuilder();
    }

    /* ~~~~ NEW NODE ~~~~ */

    Node parentNode;
    List<Node> childNodes;

    public static Node CreateNode(Node parentNode, Vector3 position) {
        Node node = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<Node>();
        node.gameObject.name = "Node";
        node.gameObject.transform.position = position;
        node.Setup(parentNode);
        return node;
    }

    public void Setup(Node parentNode) {
        this.parentNode = parentNode;
        childNodes = new List<Node>();
    }

    /* ~~~~ BUILDERS ~~~~ */

    Builder plannedBuilder = null;

    // Mouse driven, called per frame
    private void ManageBuilder() {
        // Position plan
        if (plannedBuilder) {
            plannedBuilder.SetPlannedPos(Mouse.ToGroundPos());

            // Plan
            if (Input.GetMouseButtonDown(Mouse.LEFT)) {
                plannedBuilder.FinishPlan();
                plannedBuilder = null;
            }
            // Cancel
            else if (Input.GetMouseButtonDown(Mouse.RIGHT)) {
                Destroy(plannedBuilder.gameObject);
            }
        }
        // Create planner builder
        else {
            if (mouseOver && Input.GetMouseButtonDown(Mouse.LEFT)) {                
                plannedBuilder = Builder.CreateBuilder(this, this.transform.position);
            }
        }
    }

}
