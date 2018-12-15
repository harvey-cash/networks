using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

    public static int cash = 1000;

    public static void SumCash(int delta) {
        cash += delta;
        Debug.Log("BALANCE: " + cash);
    }

}
