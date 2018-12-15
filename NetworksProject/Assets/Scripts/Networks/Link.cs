using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour {

    public Network parentNetwork;
    public void SetNetwork(Network network) {
        parentNetwork = network;
    }

    public void SetTransform(Vector3 pos, Vector3 scale, Quaternion rotation) {
        transform.position = pos;
        transform.localScale = scale;
        transform.rotation = rotation;
    }

}
