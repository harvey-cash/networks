using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour {

    public Network network;

    private void Start() {
        Node childNode = gameObject.AddComponent<Node>();
        network = new Network(null, null, childNode, true);

        // Placed by default
        childNode.SetNetwork(network);
        childNode.Place();
    }
}
