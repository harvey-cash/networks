using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewable : MonoBehaviour {
    /** Ensure viewable objects are on layer "Viewable" **/

    // Base class for all Ghost-able objects
    // We keep track of them in this class rather than having to
    // manage a centralised "Library" - which gets messy when there 
    // are many possible ways a Ghost object might be otherwise destroyed

    // Better to instead forget about destroyed Ghosts, and be tolerant
    // To missing Message Ghosts when attempting to view them

    private Ghost mostRecentGhost;

    // We delete old ghosts in favour of more recent ghosts
    // "More recent" means the message was SENT most recently - most accurate
    // return true if given ghost is the most recent
    public bool SetGhost(Ghost ghost) {
        if (mostRecentGhost) {
            // larger time is more recent
            if (ghost.timeSent > mostRecentGhost.timeSent) {
                Destroy(mostRecentGhost.gameObject);
                mostRecentGhost = ghost;
                return true;
            }
            else {
                // Aggressively purge out of date ghosts
                Destroy(ghost.gameObject);
                return false;
            }
        }
        else {
            mostRecentGhost = ghost;
            return true;
        }        
    }
}
