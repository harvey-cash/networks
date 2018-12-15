using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {
    /** Representation of an object as it was when collected
     * by a Message.
     */
    private float fadeTime = 3f;
    private float lightIntensity = 2f;
    private Light ghostLight;

    // When created, ghostify!
    private void Start() {
        // ghosts don't have kids
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }

        Destroy(GetComponent<Collider>());
        Destroy(GetComponent<Node>());

        gameObject.name = "Ghost of " + gameObject.name;
        GetComponent<Renderer>().enabled = false;        
    }

    // Called by the root network on message arrival
    public void View() {
        GetComponent<Renderer>().enabled = true;
        ghostLight = new GameObject("Ghost Light").AddComponent<Light>();
        ghostLight.transform.parent = this.gameObject.transform;
        ghostLight.transform.localPosition = Vector3.up;
        ghostLight.type = LightType.Point;
        ghostLight.intensity = lightIntensity;
        StartCoroutine(FadeAndDie());
    }

    // After being revealed, fade over time then disappear into the ether.
    private IEnumerator FadeAndDie() {
        float time = 0;
        while(time < fadeTime) {
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
            float u = time / fadeTime;

            ghostLight.intensity = (1 - u) * lightIntensity;
        }
        Destroy(this.gameObject);
    }
}
