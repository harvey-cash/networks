using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour {

    public Network parentNetwork;
    public void SetNetwork(Network network) {
        parentNetwork = network;
    }

    private void Start() {
        gameObject.GetComponent<Renderer>().material = (Material)Resources.Load("Materials/MessageLink");
    }

    public void SetTransform(Vector3 pos, Vector3 scale, Quaternion rotation) {
        transform.position = pos;
        transform.localScale = scale;
        transform.rotation = rotation;
    }

    // Called on Cancel by network
    public void Destroy() {
        Destroy(this.gameObject);
    }

}
