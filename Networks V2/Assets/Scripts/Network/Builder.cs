using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : Package
{
    private Node node;
    private Link link;
    private Vector3 planPosition, nodePosition;

    /* ~~~~~~ BUILDING HUB ~~~~~~ */

    private void Update() {
        if (!planning) {
            Vector3 forward = planPosition - nodePosition;
            
            transform.position += forward * SETTINGS.builderSpeed * Time.deltaTime;
            if (Vector3.Magnitude(transform.position - planPosition) < SETTINGS.buildDistance) {
                BuildNode();
            }
        }
    }

    private void BuildNode() {
        Node newNode = Node.CreateNode(node, planPosition);
        link.Complete(newNode);
        Destroy(this.gameObject);
    }

    /* ~~~~~~ PLANNING ~~~~~~ */

    public bool planning { get; private set; } // true by default

    public static Builder CreateBuilder(Node node, Vector3 nodePosition) {
        Builder builder = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<Builder>();
        builder.Setup(node, nodePosition);
        return builder;
    }

    public void Setup(Node node, Vector3 nodePosition) {
        this.node = node;
        this.nodePosition = nodePosition;
        link = Link.CreateLink(node, this);
    }

    private void Awake() {
        planning = true; // true by default
    }

    public void SetPlannedPos(Vector3 planPosition) {
        this.planPosition = planPosition;
        transform.position = planPosition;
    }

    public void FinishPlan() {
        planning = false;
        transform.position = nodePosition;
    }
}
