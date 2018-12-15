using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour {

    public Network network;
    public static Root root;

    private void Start() {

        Node childNode = gameObject.AddComponent<Node>();
        network = new Network(null, null, childNode, true);

        root = this;

        // Placed by default
        childNode.SetNetwork(network);
        Debug.Log(childNode.network);
        childNode.Place();
    }

    /** ~~~~~~~~~~~ BUILDING THINGS ~~~~~~~~~~~ **/
    Vector3 builderScale = new Vector3(1f, 1f, 1f);

    private Builder CreateBuilder() {
        GameObject builderObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        builderObject.transform.position = gameObject.transform.position;
        builderObject.transform.localScale = builderScale;
        builderObject.name = "Builder";

        // Material!
        Material mat = (Material)Resources.Load("Materials/Messenger");
        builderObject.GetComponent<Renderer>().material = mat;
        builderObject.GetComponent<Renderer>().enabled = false;

        Builder builder = builderObject.AddComponent<Builder>();
        builder.SetColor(mat.color);

        return builder;
    }

    // publicly accessible version, for
    // passing along existing messages
    public void DispatchBuilder(Network destination) {
        Builder builder = CreateBuilder();
        builder.SetDestination(destination, destination.node.transform.position);
    }
}
