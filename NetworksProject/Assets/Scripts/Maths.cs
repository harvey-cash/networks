using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maths {

	public static Vector3 VectorRound(Vector3 vector) {
        return new Vector3(
            Mathf.Round(vector.x),
            Mathf.Round(vector.y),
            Mathf.Round(vector.z)
        );
    }

    public static Vector2 VectorRound(Vector2 vector) {
        return new Vector2(
            Mathf.Round(vector.x),
            Mathf.Round(vector.y)
        );
    }
}
