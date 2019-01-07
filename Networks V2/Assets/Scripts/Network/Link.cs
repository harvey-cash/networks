using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    private Node parentNode, childNode;

    bool planning = true;
    Transform childTransform; // used for positioning

    public static Link CreateLink(Node parentNode, Builder builder) {
        Link link = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<Link>();
        link.gameObject.name = "Link";
        link.Setup(parentNode, builder);
        return link;
    }

    public void Setup(Node parentNode, Builder builder) {
        this.parentNode = parentNode;
        childTransform = builder.transform;
    }

    private void Update() {
        if (planning) {
            Position();
        }
    }

    private void Position() {
        Vector3 forward = childTransform.position - parentNode.transform.position;
        float linkZ = Vector3.Magnitude(forward);
        Vector3 pos = (childTransform.position + parentNode.transform.position) * 0.5f;

        transform.position = pos;
        transform.localScale = new Vector3(SETTINGS.linkX, SETTINGS.linkY, linkZ);
        transform.forward = forward;
    }

    public void Complete(Node childNode) {
        planning = false;
        this.childNode = childNode;
        childTransform = childNode.transform;
        Position();
    }
}
