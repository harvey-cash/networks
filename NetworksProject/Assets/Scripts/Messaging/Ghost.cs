using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {
    /** Representation of an object as it was when collected
     * by a Message.
     */
    public Viewable representing;
    public float timeSent; // For determining most recent ghost version

    private float fadeInTime = 1f, viewTime = 1f, fadeOutTime = 3f;
    private float lightIntensity = 1f;
    private Light ghostLight;

    // When created, ghostify!
    // Called at moment of Message being sent (leaving origin)
    private void Start() {
        timeSent = Time.time;

        // ghosts don't have kids
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }

        Destroy(GetComponent<Collider>());
        Destroy(GetComponent<Viewable>());
        GetComponent<Renderer>().material = (Material)Resources.Load("Materials/Ghost");

        gameObject.name = "Ghost of " + gameObject.name;
        GetComponent<Renderer>().enabled = false;        
    }

    // Called by the root network on message arrival
    public void View() {
        bool mostRecent = representing.SetGhost(this);

        // theoretically unreachable, as out of date ghosts are
        // destroyed by the Viewable they represent,
        // but here just in case
        if (!mostRecent) {
            Destroy(this.gameObject);
            return; 
        }

        GetComponent<Renderer>().enabled = true;
        ghostLight = new GameObject("Ghost Light").AddComponent<Light>();
        ghostLight.transform.parent = this.gameObject.transform;
        ghostLight.transform.localPosition = Vector3.up;
        ghostLight.type = LightType.Point;
        ghostLight.intensity = 0;

        StartCoroutine(FadeInThenOut());
    }

    // When revealed, fade in then eventually out
    // We don't destroy ghosts when they go dark - only when
    // a more recent representation of their object is received
    private IEnumerator FadeInThenOut() {
        // Fade in
        float time = 0;
        while (time < fadeInTime) {
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
            float u = time / fadeInTime;

            ghostLight.intensity = u * lightIntensity;
        }

        // View
        yield return new WaitForSeconds(viewTime);

        // Fade out
        time = 0;
        while (time < fadeOutTime) {
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
            float u = time / fadeOutTime;

            ghostLight.intensity = (1 - u) * lightIntensity;
        }
    }
}
